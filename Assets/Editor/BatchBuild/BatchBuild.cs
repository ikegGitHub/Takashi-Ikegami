using System.IO;
using System.Linq;
using UnityEditor;

namespace XFlag.Alter3SimulatorEditor
{
    public static class BatchBuild
    {
        [MenuItem("Window/Alter3/Build Test Client")]
        private static void BuildTestClient()
        {
            var scene = EditorBuildSettings.scenes.First(s => Path.GetFileNameWithoutExtension(s.path) == "Client");
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { scene.path },
                targetGroup = BuildTargetGroup.Standalone,
                target = BuildTarget.StandaloneWindows64,
                locationPathName = "Builds/Windows/TestClient.exe",
                options = BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }
}
