using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ClientConnectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _idText = null;

        [SerializeField]
        private TMP_Text _addressText = null;

        public void Initialize(uint id, EndPoint endPoint)
        {
            _idText.text = id.ToString();
            _addressText.text = endPoint.ToString();
        }

        private void Awake()
        {
            Assert.IsNotNull(_idText);
            Assert.IsNotNull(_addressText);
        }
    }
}
