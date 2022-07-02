using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class SimpleButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Vector3 scalePressed = new Vector3(0.9f, 0.9f, 0.9f);
    [SerializeField] float duration = 0.3f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.DOScale(scalePressed, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.DOScale(Vector3.one, duration);
    }
}
