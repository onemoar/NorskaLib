using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickableAnimator : MonoBehaviour
{
    [SerializeField] Transform visualsPivot;

    [FoldoutGroup("Float Animation"), LabelText("Enabled")]
    public bool animateFloat = true;
    [FoldoutGroup("Float Animation"), EnableIf("animateFloat")]
    public float floatDuration = 1;
    [FoldoutGroup("Float Animation"), EnableIf("animateFloat")]
    public float floatSpan = 0.1f;
    [FoldoutGroup("Float Animation"), EnableIf("animateFloat")]
    public AnimationCurve floatCurve;

    private short floatDirection = 1;
    private float floatProgress = 0.5f;
    private Vector3 floatOrigin;

    [Space]

    [FoldoutGroup("Rotation Animation"), LabelText("Enabled")]
    public bool animateRotation = true;
    [FoldoutGroup("Rotation Animation"), EnableIf("animateRotation")]
    public float angularSpeed = 15f;

    void Awake()
    {
        floatOrigin = visualsPivot.localPosition;
    }

    void Update()
    {
        var deltaTime = Time.deltaTime;

        if (animateRotation)
        {
            visualsPivot.Rotate(0, angularSpeed * deltaTime, 0);

            //Debug.Log($"SimplePickableAnimator.Update(): deltaTime = '{deltaTime}'");
        }

        if (animateFloat)
        {
            floatProgress += floatDirection * 1 / floatDuration * deltaTime;

            var position = visualsPivot.localPosition;
            position.y = floatOrigin.y + Mathf.Lerp(-floatSpan, +floatSpan, floatCurve.Evaluate(floatProgress));

            visualsPivot.localPosition = position;

            if ((floatDirection == +1 && floatProgress >= 1) || (floatDirection == -1 && floatProgress <= 0))
                floatDirection *= -1;
        }
    }
}
