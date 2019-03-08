using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class TutorialController : MonoBehaviour, IPointerClickHandler
    {
        private const string FlagFileName = "tutorial_shown.txt";

        [SerializeField]
        private Animator _animator = null;

        private IEnumerator Start()
        {
            var path = Path.Combine(Application.persistentDataPath, FlagFileName);
            if (File.Exists(path))
            {
                Destroy(gameObject);
            }
            else
            {
                yield return new WaitUntil(() => SceneManager.GetActiveScene() == gameObject.scene);
                yield return new WaitForSeconds(1.0f);
                _animator.SetTrigger("Loaded");
                File.WriteAllText(path, "If you want to view tutorial again, delete me!");
            }
        }

        public void OnFinished()
        {
            Destroy(gameObject);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _animator.SetTrigger("Next");
        }
    }
}
