/*-------------------------------------------------


Date:
Description:
Assetの強制保存
-------------------------------------------------*/
using UnityEditor;

namespace XFlag.Alter3SimulatorEditor
{
    public static class SaveAssets
    {
        [MenuItem("File/SaveAssets %&s")]
        private static void ForceSaveAssets()
        {
            AssetDatabase.SaveAssets();
        }
    }
}
