using UnityEditor;
using UnityEditor.Callbacks;

namespace XFlag.Alter3SimulatorEditor
{
    public class AutoGenerator : AssetPostprocessor
    {
        private static bool IsGenerateTriggered
        {
            get
            {
                var key = typeof(AutoGenerator).FullName;
                return EditorUserSettings.GetConfigValue(key) == "1";
            }
            set
            {
                var key = typeof(AutoGenerator).FullName;
                EditorUserSettings.SetConfigValue(key, value ? "1" : "0");
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var generatorSetting = Generator.FindGeneratorSetting();
            if (generatorSetting == null)
            {
                return;
            }

            foreach (var assetPath in importedAssets)
            {
                if (assetPath == generatorSetting.UpdateTriggerScriptPath)
                {
                    IsGenerateTriggered = true;
                    break;
                }
            }
        }

        [DidReloadScripts]
        private static void OnCompileFinished()
        {
            if (IsGenerateTriggered)
            {
                IsGenerateTriggered = false;
                Generator.Generate();
            }
        }
    }
}
