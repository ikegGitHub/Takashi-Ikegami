using System.IO;
using System.Linq;
using UnityEditor;

namespace XFlag.Alter3SimulatorEditor
{
    public static class BatchBuild
    {
        [MenuItem("Window/Alter3/Build/Test Client (Win64)")]
        private static void BuildTestClient_Win64()
        {
            BuildSingleScene("Client", "TestClient", BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Window/Alter3/Build/Test Client (OSX)")]
        private static void BuildTestClient_OSX()
        {
            BuildSingleScene("Client", "TestClient", BuildTarget.StandaloneOSX);
        }

        [MenuItem("Window/Alter3/Build/Server (Win64)")]
        private static void BuildServer_Win64()
        {
            BuildSingleScene("Server", "Alter3Simulator", BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Window/Alter3/Build/Server (OSX)")]
        private static void BuildServer_OSX()
        {
            BuildSingleScene("Server", "Alter3Simulator", BuildTarget.StandaloneOSX);
        }

        private static void BuildSingleScene(string sceneName, string outputName, BuildTarget target)
        {
            var scene = EditorBuildSettings.scenes.First(s => Path.GetFileNameWithoutExtension(s.path) == sceneName);
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { scene.path },
                targetGroup = BuildTargetGroup.Standalone,
                target = target,
                locationPathName = $"Builds/{target}/{outputName}/{outputName}{GetExtension(target)}",
                options = BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
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
