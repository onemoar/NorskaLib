using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NorskaLib.UI.Widgets
{
    [RequireComponent(typeof(RectTransform))]
	public class Widget : MonoBehaviour
	{
        [SerializeField] string id;
        public string Id => id;

        public RectTransform RectTransform { get; private set; }

        private WidgetEvents events;

        #region MonoBehaviour

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();

            events = GetComponent<WidgetEvents>();
            if (events != null)
            {
                events.onClick          += OnClick;
            }

            WidgetProvider.Instance.RegisterInstance(this);
        }

        protected virtual void OnEnable()
        {
            WidgetProvider.Instance.OnWidgetEnabled(this);
        }

        protected virtual void OnDisable()
        {
            WidgetProvider.Instance.OnWidgetDisabled(this);
        }

        protected virtual void OnDestroy()
        {
            if (events != null)
            {
                events.onClick          -= OnClick;      
            }

            WidgetProvider.Instance.UnregisterInstance(this);
        }

        #endregion

        void OnClick(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(id))
                return;

            Events.onWidgetClick?.Invoke(this.id);
        }
    }
}