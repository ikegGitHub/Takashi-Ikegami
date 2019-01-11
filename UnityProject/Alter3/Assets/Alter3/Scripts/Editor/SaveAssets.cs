/*-------------------------------------------------


Date:
Description:
Assetの強制保存
-------------------------------------------------*/

using UnityEditor;

public static class SaveAssets
{
	[MenuItem("File/SaveAssets %&s")]
	public static void ForceSaveAssets()
	{
		AssetDatabase.SaveAssets();
	}
}
