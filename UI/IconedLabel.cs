using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Label = TMPro.TextMeshProUGUI;

namespace NorskaLib.UI
{
    public class IconedLabel : MonoBehaviour
    {
        [SerializeField] Image icon;
        public Image Icon => icon;
        public RectTransform IconRect => icon.rectTransform;

        [SerializeField] Label label;
        public Label Label => label;
        public RectTransform LabelRect => label.rectTransform;
    }
}
