using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [CreateAssetMenu(menuName = "Alter3/BuildInfoPlaceholder", fileName = "BuildInfoPlaceholder")]
    public class BuildInfoPlaceholder : ScriptableObject
    {
        [HideInInspector]
        public string BuildDateString;

        [HideInInspector]
        public string OperatingSystem;
    }
}
