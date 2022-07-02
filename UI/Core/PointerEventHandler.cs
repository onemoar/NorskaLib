using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Events = NorskaLib.UI.Events;

namespace NorskaLib.UI
{
    public class PointerEventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] string id;

        [SerializeField] bool catchPointerClick,
                              catchPointerDown,
                              catchPointerUp,
                              catchPointerEnter,
                              catchPointerExit;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (catchPointerClick)
                Events.onWidgetClick.Invoke(id);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }
    }
}
