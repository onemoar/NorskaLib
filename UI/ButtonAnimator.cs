using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using NorskaLib.Utilities;

namespace NorskaLib.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] bool uniformScale;

        [ShowIf("@uniformScale"), LabelText("Scale Pressed")]
        [SerializeField] float scalePressedUniform = 0.9f;
        [ShowIf("@!uniformScale"), LabelText("Scale Pressed")]
        [SerializeField] Vector3 scalePressed = new Vector3(0.9f, 0.9f, 0.9f);

        [SerializeField] float duration = 0.2f;

        private RectTransform rectTransform;
        private Tween scaleTween;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void OnDisable()
        {
            scaleTween?.Kill(complete: false);
            rectTransform.localScale = Vector3.one;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            scaleTween?.Kill(complete: true);
            var scale = uniformScale ? Vector3Utils.Uniform(scalePressedUniform) : scalePressed;
            scaleTween = rectTransform.DOScale(scale, duration);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            scaleTween?.Kill(complete: true);
            scaleTween = rectTransform.DOScale(Vector3.one, duration);
        }
    }
}