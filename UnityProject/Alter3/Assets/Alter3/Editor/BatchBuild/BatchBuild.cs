using System;
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

        [MenuItem("Alter3/ビルド/Win64/All", priority = 0)]
        private static void BuildAll_Win64()
        {
            BuildServer_Win64();
            BuildHeadlessServer_Win64();
            BuildTestClient_Win64();
        }

        [MenuItem("Alter3/ビルド/macOS/All", priority = 0)]
        private static void BuildAll_OSX()
        {
            BuildServer_OSX();
            BuildHeadlessServer_OSX();
            BuildTestClient_OSX();
        }

        [MenuItem("Alter3/ビルド/Win64/Test Client", priority = 11)]
        private static void BuildTestClient_Win64()
        {
            BuildTestClient(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Alter3/ビルド/macOS/Test Client", priority = 11)]
        private static void BuildTestClient_OSX()
        {
            BuildTestClient(BuildTarget.StandaloneOSX);
        }

        [MenuItem("Alter3/ビルド/Win64/Server", priority = 12)]
        private static void BuildServer_Win64()
        {
            BuildSimulator(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Alter3/ビルド/macOS/Server", priority = 12)]
        private static void BuildServer_OSX()
        {
            BuildSimulator(BuildTarget.StandaloneOSX);
        }

        [MenuItem("Alter3/ビルド/Win64/Server (Headless)", priority = 13)]
        private static void BuildHeadlessServer_Win64()
        {
            BuildHeadlessSimulator(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Alter3/ビルド/macOS/Server (Headless)", priority = 13)]
        private static void BuildHeadlessServer_OSX()
        {
            BuildHeadlessSimulator(BuildTarget.StandaloneOSX);
        }

        private static void BuildTestClient(BuildTarget target)
        {
            PreserveSettings(() =>
            {
                PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
                PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
                BuildScenes(TestClientScenes, "TestClient", target);
            });
        }

        private static void BuildSimulator(BuildTarget target)
        {
            PreserveSettings(() =>
            {
                PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Enabled;
                BuildScenes(SimulatorScenes, "Alter3Simulator", target);
            });
        }

        private static void BuildHeadlessSimulator(BuildTarget target)
        {
            BuildScenes(SimulatorScenes, "Alter3Simulator-Headless", target, BuildOptions.EnableHeadlessMode);
        }

        private static void PreserveSettings(Action action)
        {
            var savedFullScreenMode = PlayerSettings.fullScreenMode;
            var savedResolutionDialogSetting = PlayerSettings.displayResolutionDialog;
            try
            {
                action();
            }
            finally
            {
                PlayerSettings.fullScreenMode = savedFullScreenMode;
                PlayerSettings.displayResolutionDialog = savedResolutionDialogSetting;
            }
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
