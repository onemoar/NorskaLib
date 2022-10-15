using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NorskaLib.Extensions
{
    // TO DO:
    // Other types of components
    public static class ComponentExtensions
    {
        public static void Mimic<T>(this T @this, T other) where T : Component
        {
            if (@this is Behaviour thisB)
            {
                var otherB = other as Behaviour;
                thisB.enabled = otherB.enabled;
            }

            switch (@this)
            {
                case RectTransform thisRT:
                    thisRT.Mimic(other as RectTransform);
                    break;

                case ContentSizeFitter thisCSF:
                    thisCSF.Mimic(other as ContentSizeFitter);
                    break;

                case HorizontalOrVerticalLayoutGroup thisLG:
                    thisLG.Mimic(other as HorizontalOrVerticalLayoutGroup);
                    break;

                default:
                    throw new System.NotImplementedException();
                    //break;
            }
        }

        public static void Mimic(this ContentSizeFitter @this, ContentSizeFitter other)
        {
            @this.horizontalFit   = other.horizontalFit;
            @this.verticalFit     = other.verticalFit;
        }

        public static void Mimic(this RectTransform @this, RectTransform other)
        {
            @this.pivot             = other.pivot;
            @this.anchorMin         = other.anchorMin;
            @this.anchorMax         = other.anchorMax;
            @this.sizeDelta         = other.sizeDelta;
            @this.anchoredPosition  = other.anchoredPosition;
        }

        public static void Mimic(this HorizontalOrVerticalLayoutGroup @this, HorizontalOrVerticalLayoutGroup other)
        {
            @this.childAlignment           = other.childAlignment;
            @this.padding                  = other.padding;
            @this.spacing                  = other.spacing;
            @this.reverseArrangement       = other.reverseArrangement;

            @this.childControlHeight       = other.childControlHeight;
            @this.childControlWidth        = other.childControlWidth;
            @this.childForceExpandHeight   = other.childForceExpandHeight;
            @this.childForceExpandWidth    = other.childForceExpandWidth;
            @this.childScaleHeight         = other.childScaleHeight;
            @this.childScaleWidth          = other.childScaleWidth;
        }
    }
}