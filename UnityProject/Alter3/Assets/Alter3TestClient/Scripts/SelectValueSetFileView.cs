using System;
using System.IO;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class SelectValueSetFileView : MonoBehaviour
    {
        [SerializeField]
        private ButtonView _buttonPrefab = null;

        [SerializeField]
        private Transform _buttonRoot = null;

        public event Action<string> OnFileSelected;

        private void OnEnable()
        {
            foreach (var filePath in Directory.EnumerateFiles(@"./Presets", @"*.csv"))
            {
                var button = Instantiate(_buttonPrefab, _buttonRoot, false);
                button.ButtonText = filePath;
                button.OnClick += () =>
                {
                    OnFileSelected?.Invoke(filePath);
                    gameObject.SetActive(false);
                };
            }
        }

        private void OnDisable()
        {
            foreach (Transform child in _buttonRoot)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
