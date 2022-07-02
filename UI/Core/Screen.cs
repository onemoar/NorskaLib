using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class Screen : MonoBehaviour
    {
        protected static ScreenManager GUIManager => ScreenManager.Instance;

        public RectTransform Rect
        { get; private set; }
        protected CanvasGroup Group
        { get; private set; }
        protected DoTweenCanvasFader screenFader;

        protected int order;
        public int Order
        {
            get => order;

            set
            {
                name = $"{this.GetType().Name} (Layer: {value})";
                order = value;
                GUIManager.UpdateScreensOrder();
            }
        }

        protected virtual void Awake()
        {
            Rect = GetComponent<RectTransform>();
            Group = GetComponent<CanvasGroup>();

            screenFader = new DoTweenCanvasFader(Group);

            name = $"{this.GetType().Name} (Layer: {Order})";
        }

        protected virtual void OnDestroy()
        {
            screenFader?.Stop();
            UI.Events.onScreenDestroyed.Invoke(this);
        }

        public void SetAlpha(float alpha, float duration = 0)
        {
            if (duration > 0)
                screenFader.Transit(alpha, duration);
            else
                screenFader.SetAlpha(alpha);
        }

        public void Show(bool animated = false)
        {
            IEnumerator Routine()
            {
                yield return StartCoroutine(ShowScenario());
                UI.Events.onScreenShown.Invoke(this);
            }

            gameObject.SetActive(true);
            if (animated)
                StartCoroutine(Routine());
            else
            {
                screenFader.SetAlpha(1);
                UI.Events.onScreenShown.Invoke(this);
            }
        }

        public void Hide(bool animated = false)
        {
            IEnumerator Routine()
            {
                yield return StartCoroutine(HideScenario());
                gameObject.SetActive(false);
                UI.Events.onScreenHidden.Invoke(this);
            }

            if (animated)
                StartCoroutine(Routine());
            else
            {
                gameObject.SetActive(false);
                screenFader.SetAlpha(0);
                UI.Events.onScreenHidden.Invoke(this);
            }
        }

        protected virtual IEnumerator ShowScenario()
        {
            screenFader.Transit(0, 1, 0.2f);
            yield return new WaitForSeconds(0.2f);
        }

        protected virtual IEnumerator HideScenario()
        {
            screenFader.Transit(1, 0, 0.2f);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
