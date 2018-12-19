using TMPro;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class BuildInfoView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _buildInfoText = null;

        private void Awake()
        {
            var buildInfoData = Resources.Load<BuildInfoPlaceholder>("BuildInfoData");
            if (buildInfoData != null)
            {
                _buildInfoText.text = $"{buildInfoData.BuildDateString} {buildInfoData.OperatingSystem}";
            }
        }
    }
}
