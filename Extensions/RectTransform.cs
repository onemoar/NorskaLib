using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.Extensions
{
    public static class RectTransformExtensions
    {
        public static void DestroyChildren(this RectTransform instance)
        {
            var childCount = instance.childCount;
            for (var i = childCount - 1; i > -1; i--)
            {
                Object.Destroy(instance.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Shortcut for 'transform.localScale = new Vector3(scale,scale,scale)'.
        /// </summary>
        public static void SetScale(this RectTransform instance, float scale)
        {
            instance.localScale = new Vector3(scale, scale, scale);
        }
    }
}