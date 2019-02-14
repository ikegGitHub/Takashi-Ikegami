using UnityEngine;
using UnityEngine.EventSystems;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private DragAreaView _dragAreView = null;

        private void Awake()
        {
            _dragAreView.OnDrag += OnDrag;
            _dragAreView.OnScroll += OnScroll;
        }

        private void OnDestroy()
        {
            _dragAreView.OnDrag -= OnDrag;
            _dragAreView.OnScroll -= OnScroll;
        }

        private void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                float transSpeed = 0.2f;
                var delta2 = new Vector2(delta.x, -delta.y);
                transform.Translate(delta2 * Time.deltaTime * transSpeed);
            }
            else
            {
                var angles = transform.localEulerAngles;
                angles.y = Mathf.Repeat(angles.y + delta.x * 0.15f, 360);
                angles.x = Mathf.Repeat(angles.x + delta.y * 0.15f, 360);
                transform.localEulerAngles = angles;
            }
        }

        private void OnScroll(PointerEventData eventData)
        {
            transform.Translate(0, 0, eventData.scrollDelta.y);
        }
    }
}
