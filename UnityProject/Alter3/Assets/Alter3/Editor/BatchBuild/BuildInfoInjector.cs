using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using XFlag.Alter3Simulator;

namespace XFlag.Alter3SimulatorEditor
{
    public class BuildInfoInjector : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        int IOrderedCallback.callbackOrder => 0;

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            var buildInfoPlaceholder = FindBuildInfoPlaceholderAsset();
            if (buildInfoPlaceholder == null)
            {
                return;
            }

            buildInfoPlaceholder.BuildDateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buildInfoPlaceholder.OperatingSystem = Environment.OSVersion.ToString();
            EditorUtility.SetDirty(buildInfoPlaceholder);
            AssetDatabase.SaveAssets();
        }

        void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report)
        {
            var buildInfoPlaceholder = FindBuildInfoPlaceholderAsset();
            if (buildInfoPlaceholder == null)
            {
                return;
            }

            var assetPath = AssetDatabase.GetAssetPath(buildInfoPlaceholder);
            buildInfoPlaceholder = ScriptableObject.CreateInstance<BuildInfoPlaceholder>();
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(buildInfoPlaceholder, assetPath);
            AssetDatabase.SaveAssets();
        }

        private BuildInfoPlaceholder FindBuildInfoPlaceholderAsset()
        {
            var guid = AssetDatabase.FindAssets($"t:{nameof(BuildInfoPlaceholder)}").First();
            if (guid == null)
            {
                return null;
            }
            return AssetDatabase.LoadAssetAtPath<BuildInfoPlaceholder>(AssetDatabase.GUIDToAssetPath(guid));
        }
    }
}
