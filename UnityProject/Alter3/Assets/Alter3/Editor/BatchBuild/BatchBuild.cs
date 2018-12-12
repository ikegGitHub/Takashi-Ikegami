using System.IO;
using System.Linq;
using UnityEditor;

namespace XFlag.Alter3SimulatorEditor
{
    public static class BatchBuild
    {
        [MenuItem("Window/Alter3/Build/Test Client")]
        private static void BuildTestClient()
        {
            BuildSingleScene("Client", "TestClient");
        }

        [MenuItem("Window/Alter3/Build/Server")]
        private static void BuildServer()
        {
            BuildSingleScene("Server", "Alter3Simulator");
        }

        private static void BuildSingleScene(string sceneName, string outputName)
        {
            var scene = EditorBuildSettings.scenes.First(s => Path.GetFileNameWithoutExtension(s.path) == sceneName);
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { scene.path },
                targetGroup = BuildTargetGroup.Standalone,
                target = BuildTarget.StandaloneWindows64,
                locationPathName = $"Builds/Windows/{outputName}/{outputName}.exe",
                options = BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }
}
