using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XFlag.Alter3Simulator;

namespace XFlag.Alter3SimulatorEditor
{
    public class AxisTableConverter
    {
        private const string DestPath = "Assets/Alter3/Res/Resources/Table";

        [MenuItem("Assets/Alter3/軸テーブル変換")]
        private static void Convert()
        {
            var excelPaths = Selection.objects
                .Where(obj => obj is DefaultAsset)
                .Select(obj => AssetDatabase.GetAssetPath(obj))
                .Where(assetPath => assetPath.EndsWith(".xlsx"));

            foreach (var excelPath in excelPaths)
            {
                GenerateJointTable(excelPath);
            }
        }

        private static void GenerateJointTable(string excelFilePath)
        {
            var jointTableAsset = ScriptableObject.CreateInstance<JointTable>();
            jointTableAsset.list = new List<JointTableEntity>();

            using (var fileSteam = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var book = WorkbookFactory.Create(fileSteam);
                var sheet = book.GetSheetAt(0);

                var rowIndex = 2;
                JointTableEntityData entityData = null;
                while (true)
                {
                    var row = sheet.GetRow(rowIndex++);

                    // 行が無い、またはJoint項目が空だったら終了
                    if (row?.GetCell(6) == null)
                    {
                        // 現在の軸テーブルエントリ終了
                        if (entityData != null)
                        {
                            jointTableAsset.list.Add(entityData.ToJointTableEntity());
                        }
                        break;
                    }

                    // AxisNumberに値が入っていたら新しい軸テーブルエントリ開始
                    if (row.GetCell(0) != null)
                    {
                        // 現在の軸テーブルエントリ終了
                        if (entityData != null)
                        {
                            jointTableAsset.list.Add(entityData.ToJointTableEntity());
                        }

                        var axisNumber = (int)row.GetCell(0).NumericCellValue;
                        var summary = row.GetCell(1)?.StringCellValue ?? "";
                        var description = row.GetCell(2)?.StringCellValue ?? "";
                        var isSpring = row.GetCell(3)?.StringCellValue == "O";
                        var spring = (float)row.GetCell(4).NumericCellValue;
                        var dumper = (float)row.GetCell(5).NumericCellValue;
                        entityData = new JointTableEntityData
                        {
                            AxisNumber = axisNumber,
                            Summary = summary,
                            Description = description,
                            IsSpring = isSpring,
                            Spring = spring,
                            Dumper = dumper
                        };
                    }

                    if (entityData == null)
                    {
                        Debug.LogError("エラーが発生しました");
                        break;
                    }

                    var jointName = row.GetCell(6).StringCellValue;
                    var axisX = (float)row.GetCell(7).NumericCellValue;
                    var axisY = (float)row.GetCell(8).NumericCellValue;
                    var axisZ = (float)row.GetCell(9).NumericCellValue;
                    var angleMin = (int)row.GetCell(10).NumericCellValue;
                    var angleMax = (int)row.GetCell(11).NumericCellValue;

                    entityData.JointItems.Add(new JointItem { JointName = jointName, Axis = new Vector3(axisX, axisY, axisZ), rangeMin = angleMin, rangeMax = angleMax });
                }

                var saveAssetPath = Path.Combine(DestPath, $"{sheet.SheetName}.asset");
                if (File.Exists(saveAssetPath))
                {
                    Debug.Log($"Updating {saveAssetPath}");
                    var tmpAssetPath = AssetDatabase.GenerateUniqueAssetPath(DestPath);
                    AssetDatabase.CreateAsset(jointTableAsset, tmpAssetPath);
                    AssetDatabase.ImportAsset(tmpAssetPath);
                    FileUtil.ReplaceFile(saveAssetPath, tmpAssetPath);
                    AssetDatabase.DeleteAsset(tmpAssetPath);
                    AssetDatabase.ImportAsset(saveAssetPath);
                }
                else
                {
                    Debug.Log($"Creating {saveAssetPath}");
                    AssetDatabase.CreateAsset(jointTableAsset, saveAssetPath);
                    AssetDatabase.SaveAssets();
                }

                EditorGUIUtility.PingObject(jointTableAsset);

                book.Close();
            }

            Debug.Log("done");
        }

        private class JointTableEntityData
        {
            public int AxisNumber;
            public string Summary;
            public string Description;
            public bool IsSpring;
            public float Spring;
            public float Dumper;
            public List<JointItem> JointItems = new List<JointItem>();

            public JointTableEntity ToJointTableEntity()
            {
                return new JointTableEntity
                {
                    id = AxisNumber,
                    Name = Summary,
                    Content = Description,
                    IsSpring = IsSpring,
                    Spring = Spring,
                    Dumper = Dumper,
                    JointItems = JointItems.ToArray()
                };
            }
        }
    }
}
