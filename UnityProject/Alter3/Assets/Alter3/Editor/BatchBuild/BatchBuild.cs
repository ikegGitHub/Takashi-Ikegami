using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XFlag.Alter3SimulatorEditor
{
    public static class BatchBuild
    {
        private const BuildOptions DefaultBuildOptions = BuildOptions.ShowBuiltPlayer;

        private static readonly string[] SimulatorScenes = { "Splash", "Server" };
        private static readonly string[] TestClientScenes = { "Client" };

        [MenuItem("Window/Alter3/Build (Win64)/All", priority = 0)]
        private static void BuildAll_Win64()
        {
            BuildServer_Win64();
            BuildHeadlessServer_Win64();
            BuildTestClient_Win64();
        }

        [MenuItem("Window/Alter3/Build (OSX)/All", priority = 0)]
        private static void BuildAll_OSX()
        {
            BuildServer_OSX();
            BuildHeadlessServer_OSX();
            BuildTestClient_OSX();
        }

        [MenuItem("Window/Alter3/Build (Win64)/Test Client", priority = 11)]
        private static void BuildTestClient_Win64()
        {
            BuildScenes(TestClientScenes, "TestClient", BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Window/Alter3/Build (OSX)/Test Client", priority = 11)]
        private static void BuildTestClient_OSX()
        {
            BuildScenes(TestClientScenes, "TestClient", BuildTarget.StandaloneOSX);
        }

        [MenuItem("Window/Alter3/Build (Win64)/Server", priority = 12)]
        private static void BuildServer_Win64()
        {
            BuildScenes(SimulatorScenes, "Alter3Simulator", BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Window/Alter3/Build (OSX)/Server", priority = 12)]
        private static void BuildServer_OSX()
        {
            BuildScenes(SimulatorScenes, "Alter3Simulator", BuildTarget.StandaloneOSX);
        }

        [MenuItem("Window/Alter3/Build (Win64)/Server (Headless)", priority = 13)]
        private static void BuildHeadlessServer_Win64()
        {
            BuildScenes(SimulatorScenes, "Alter3Simulator-Headless", BuildTarget.StandaloneWindows64, BuildOptions.EnableHeadlessMode);
        }

        [MenuItem("Window/Alter3/Build (OSX)/Server (Headless)", priority = 13)]
        private static void BuildHeadlessServer_OSX()
        {
            BuildScenes(SimulatorScenes, "Alter3Simulator-Headless", BuildTarget.StandaloneOSX, BuildOptions.EnableHeadlessMode);
        }

        private static void BuildScenes(string[] sceneNames, string outputName, BuildTarget target, BuildOptions options = BuildOptions.None)
        {
            Debug.Log($"Starting build {outputName}");
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(sceneNames),
                targetGroup = BuildTargetGroup.Standalone,
                target = target,
                locationPathName = $"Builds/{target}/{outputName}/{outputName}{GetExtension(target)}",
                options = DefaultBuildOptions | options,
            };
            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            Debug.Log($"Build {outputName} {buildReport.summary.result}");
        }

        private static string[] GetScenePaths(string[] names)
        {
            return EditorBuildSettings.scenes
                .Where(scene => names.Contains(Path.GetFileNameWithoutExtension(scene.path)))
                .Select(scene => scene.path)
                .ToArray();
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
