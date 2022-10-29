using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class Window : MonoBehaviour
    {
        public RectTransform Rect
        { get; private set; }
        protected CanvasGroup Group
        { get; private set; }
        protected Tween fadeTween;

        protected int order;
        public int Order
        {
            get => order;

            set
            {
                if (order == value)
                    return;

#if UNITY_EDITOR
                name = $"{this.GetType().Name} (Layer: {value})";
#endif

                order = value;
                UI.Events.onWindowOrderChanged?.Invoke(this, value);
            }
        }

        protected virtual void Awake()
        {
            Rect = GetComponent<RectTransform>();
            Group = GetComponent<CanvasGroup>();

#if UNITY_EDITOR
            name = $"{this.GetType().Name} (Layer: {Order})";
#endif
        }

        protected virtual void OnDestroy()
        {
            fadeTween?.Kill(true);
            UI.Events.onWindowDestroyed?.Invoke(this);
        }

        public void SetAlpha(float alpha, float duration = 0)
        {
            if (duration > 0)
                fadeTween = Group.DOFade(alpha, duration);
            else
                Group.alpha = alpha;
        }

        internal void Show(bool animated)
        {
            void Finish()
            {
                Group.alpha = 1;
                UI.Events.onWindowShown?.Invoke(this);
            }
            
            IEnumerator Routine()
            {
                yield return StartCoroutine(ShowScenario());
                Finish();
            }

            gameObject.SetActive(true);

            if (animated)
                StartCoroutine(Routine());
            else
                Finish();

        }

        internal void Hide(bool animated, bool destroy)
        {
            void Finish()
            {
                if (destroy)
                    Destroy(this.gameObject);
                else
                {
                    gameObject.SetActive(false);
                    Group.alpha = 0;
                }

                UI.Events.onWindowHidden?.Invoke(this);
            }

            IEnumerator Routine()
            {
                yield return StartCoroutine(HideScenario());
                Finish();
            }

            if (destroy && !gameObject.activeSelf)
            {
                Destroy(this.gameObject);
                return;
            }

            if (animated)
                StartCoroutine(Routine());
            else
                Finish();
        }

        protected virtual IEnumerator ShowScenario()
        {
            var duration = 0.2f;

            Group.alpha = 0;
            fadeTween = Group.DOFade(1, duration);

            yield return new WaitForSeconds(duration);
        }

        protected virtual IEnumerator HideScenario()
        {
            var duration = 0.2f;

            Group.alpha = 1;
            fadeTween = Group.DOFade(0, duration);

            yield return new WaitForSeconds(duration);
        }
    }
}
