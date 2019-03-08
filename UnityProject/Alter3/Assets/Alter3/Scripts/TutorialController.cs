using UnityEngine;
using UnityEngine.EventSystems;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class TutorialController : MonoBehaviour, IPointerClickHandler
    {
        private const string FlagKey = "tutorialShown";

        [SerializeField]
        private Animator _animator = null;

        private void Awake()
        {
            var tutorialShownFlag = PlayerPrefs.GetInt(FlagKey, 0);
            if (tutorialShownFlag == 0)
            {
                PlayerPrefs.SetInt(FlagKey, 1);
            }
            else
            {
                Destroy(gameObject);
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
