using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(Graphic))]
    public class DragAreaView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> OnBeginDrag = delegate { };
        public event Action<PointerEventData> OnDrag = delegate { };
        public event Action<PointerEventData> OnEndDrag = delegate { };

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDrag(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }
    }
}
