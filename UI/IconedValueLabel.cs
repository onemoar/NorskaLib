using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Label = TMPro.TextMeshProUGUI;

namespace NorskaLib.UI
{
    public class IconedValueLabel : MonoBehaviour
    {
        [SerializeField] Image icon;
        public Image Icon => icon;
        public RectTransform IconRect => icon.rectTransform;

        [SerializeField] ValueLabel label;
        public ValueLabel Label => label;
        public RectTransform LabelRect => label.Label.rectTransform;
    }
}