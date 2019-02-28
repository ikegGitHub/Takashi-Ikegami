using UnityEngine;
using UnityEngine.Assertions;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class FaceCameraController : MonoBehaviour
    {
        [SerializeField]
        private float _distance = 1;

        [SerializeField]
        private float _offsetHeight = 0;

        public Transform LookTarget { get; set; }

        private void OnEnable()
        {
            Assert.IsNotNull(LookTarget);
        }

        private void LateUpdate()
        {
            if (LookTarget != null)
            {
                transform.position = LookTarget.position + LookTarget.forward * _distance;
                transform.LookAt(LookTarget, LookTarget.up);
                transform.position += LookTarget.up * _offsetHeight;
            }
        }
    }
}
