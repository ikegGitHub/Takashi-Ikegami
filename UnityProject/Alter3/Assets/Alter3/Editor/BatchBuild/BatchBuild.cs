using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XFlag.Alter3SimulatorEditor
{
    public static class BatchBuild
    {
        [MenuItem("Window/Alter3/Build (Win64)/All")]
        private static void BuildAll_Win64()
        {
            BuildServer_Win64();
            BuildTestClient_Win64();
        }

        [MenuItem("Window/Alter3/Build (OSX)/All")]
        private static void BuildAll_OSX()
        {
            BuildServer_OSX();
            BuildTestClient_OSX();
        }

        [MenuItem("Window/Alter3/Build (Win64)/Test Client")]
        private static void BuildTestClient_Win64()
        {
            BuildSingleScene("Client", "TestClient", BuildTarget.StandaloneWindows64, BuildOptions.None);
        }

        [MenuItem("Window/Alter3/Build (OSX)/Test Client")]
        private static void BuildTestClient_OSX()
        {
            BuildSingleScene("Client", "TestClient", BuildTarget.StandaloneOSX, BuildOptions.None);
        }

        [MenuItem("Window/Alter3/Build (Win64)/Server")]
        private static void BuildServer_Win64()
        {
            BuildSingleScene("Server", "Alter3Simulator", BuildTarget.StandaloneWindows64, BuildOptions.None);
        }

        [MenuItem("Window/Alter3/Build (OSX)/Server")]
        private static void BuildServer_OSX()
        {
            BuildSingleScene("Server", "Alter3Simulator", BuildTarget.StandaloneOSX, BuildOptions.None);
        }

        private static void BuildSingleScene(string sceneName, string outputName, BuildTarget target, BuildOptions options)
        {
            Debug.Log($"Starting build {outputName}");
            var scene = EditorBuildSettings.scenes.First(s => Path.GetFileNameWithoutExtension(s.path) == sceneName);
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { scene.path },
                targetGroup = BuildTargetGroup.Standalone,
                target = target,
                locationPathName = $"Builds/{target}/{outputName}/{outputName}{GetExtension(target)}",
                options = options
            };
            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            Debug.Log($"Build {buildReport.summary.result}");
        }

        private static string GetExtension(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneOSX:
                    return ".app";
                case BuildTarget.StandaloneWindows64:
                    return ".exe";
            }
            return "";
        }
    }
}
