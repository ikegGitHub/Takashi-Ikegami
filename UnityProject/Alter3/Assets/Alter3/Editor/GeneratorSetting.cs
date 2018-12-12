using UnityEditor;
using UnityEngine;

namespace XFlag.Alter3SimulatorEditor
{
    [CreateAssetMenu(menuName = "Generator Setting")]
    public class GeneratorSetting : ScriptableObject
    {
        [SerializeField]
        private MonoScript _updateTriggerScript = null;

        [SerializeField]
        private DefaultAsset _outputFolder = null;

        public string UpdateTriggerScriptPath => AssetDatabase.GetAssetPath(_updateTriggerScript);

        public string FolderPath => AssetDatabase.GetAssetPath(_outputFolder);

        private void OnValidate()
        {
            if (!AssetDatabase.IsValidFolder(FolderPath))
            {
                _outputFolder = null;
            }
        }
    }
}
