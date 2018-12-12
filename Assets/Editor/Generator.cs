using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using XFlag.Alter3Simulator;

namespace XFlag.Alter3SimulatorEditor
{
    public static class Generator
    {
        public static GeneratorSetting FindGeneratorSetting()
        {
            return AssetDatabase.FindAssets($"t:{nameof(GeneratorSetting)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GeneratorSetting>)
                .First();
        }

        [MenuItem("Window/Alter3/Generate Command Classes")]
        public static void Generate()
        {
            var setting = FindGeneratorSetting();
            if (setting == null)
            {
                return;
            }

            var outputPath = setting.FolderPath;
            var autoGenPath = Path.Combine(outputPath, "Generated");

            AssetDatabase.StartAssetEditing();

            if (!Directory.Exists(autoGenPath))
            {
                Directory.CreateDirectory(autoGenPath);
            }

            var commandTypes = Enum.GetNames(typeof(CommandType));
            foreach (var type in commandTypes)
            {
                var code = $@"// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{{
    public partial class {type}Command : ICommand
    {{
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }}

    public partial interface ICommandVisitor
    {{
        void Visit({type}Command command);
    }}

    public partial interface ICommandVisitor<T>
    {{
        T Visit({type}Command command);
    }}

    public partial class CommandVisitorBase : ICommandVisitor
    {{
        public virtual void Visit({type}Command command) => Default(command);
    }}

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {{
        public virtual T Visit({type}Command command) => Default(command);
    }}
}}
";
                var file = Path.Combine(autoGenPath, $"{type}Command.generated.cs");
                File.WriteAllText(file, code.Replace("\n", "\r\n"), Encoding.UTF8);
                AssetDatabase.ImportAsset(file);

                file = Path.Combine(outputPath, $"{type}Command.cs");
                if (File.Exists(file))
                {
                    continue;
                }
                code = $@"namespace XFlag.Alter3Simulator
{{
    public partial class {type}Command
    {{
    }}
}}
";
                File.WriteAllText(file, code.Replace("\n", "\r\n"), Encoding.UTF8);
                AssetDatabase.ImportAsset(file);
            }

            var commandFileNames = commandTypes.Select(t => $"{t}Command.cs").ToArray();
            foreach (var file in Directory.EnumerateFiles(outputPath))
            {
                var name = Path.GetFileName(file);
                if (!commandFileNames.Contains(name))
                {
                    AssetDatabase.DeleteAsset(file);
                }
            }
            var generatedFileNames = commandTypes.Select(t => $"{t}Command.generated.cs").ToArray();
            foreach (var file in Directory.EnumerateFiles(autoGenPath))
            {
                var name = Path.GetFileName(file);
                if (!generatedFileNames.Contains(name))
                {
                    AssetDatabase.DeleteAsset(file);
                }
            }

            AssetDatabase.StopAssetEditing();
        }
    }
}
