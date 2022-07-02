using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TextField = TMPro.TextMeshProUGUI;

namespace NorskaLib.UI
{
    public class IconedLabel : MonoBehaviour
    {
        [SerializeField] Image icon;
        public Image Icon => icon;
        public RectTransform IconRect => icon.rectTransform;

        [SerializeField] TextField label;
        public TextField Label => label;
        public RectTransform LabelRect => label.rectTransform;

        Coroutine countAnimatioRoutine;

        public void AnimateCount(int a, int b, float duration)
        {
            if (countAnimatioRoutine != null)
                StopCoroutine(countAnimatioRoutine);

            countAnimatioRoutine = StartCoroutine(CountAnimatioRoutine(a, b, duration));
        }
        IEnumerator CountAnimatioRoutine(int a, int b, float duration)
        {
            var t = 0f;
        
            while (t < duration)
            {
                label.text = Mathf.RoundToInt(Mathf.Lerp(a, b, t / duration)).ToString();
                yield return null;
                t += Time.deltaTime;
            }
            label.text = b.ToString();
            countAnimatioRoutine = null;
        }
    }
}
