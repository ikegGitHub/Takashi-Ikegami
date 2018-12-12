using SyntaxTree.VisualStudio.Unity.Bridge;
using System;
using System.Linq;
using UnityEditor;

namespace XFlag.Alter3SimulatorEditor
{
    [InitializeOnLoad]
    public class AutoAddEditorConfigToSolution
    {
        static AutoAddEditorConfigToSolution()
        {
            ProjectFilesGenerator.SolutionFileGeneration += (name, content) =>
            {
                var appendLines = new string[]
                {
                    "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Solution Items\", \"Solution Items\", \"{04F35B82-4D12-42F3-B1F7-1606772B4136}\"",
                    "\tProjectSection(SolutionItems) = preProject",
                    "\t\t.editorconfig = .editorconfig",
                    "\tEndProjectSection",
                    "EndProject"
                };
                var lines = content.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
                var index = lines.IndexOf("Global");
                lines.InsertRange(index, appendLines);
                return lines.Aggregate((a, b) => $"{a}\r\n{b}");
            };
        }
    }
}
