using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NorskaLib.UI.Widgets
{
    [RequireComponent(typeof(RectTransform))]
	public abstract class Widget : MonoBehaviour
	{
        public abstract string Id { get;  }

        public RectTransform RectTransform { get; private set; }
        public WidgetEvents Events { get; private set; }

        #region MonoBehaviour

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();

            Events = GetComponent<WidgetEvents>();
            if (Events != null)
            {
                Events.onClick          += OnClick;
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
            if (Events != null)
            {
                Events.onClick          -= OnClick;      
            }

            WidgetProvider.Instance.UnregisterInstance(this);
        }

        #endregion

        void OnClick(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(Id))
                return;

            UI.Events.onWidgetClick?.Invoke(this.Id);
        }
    }
}