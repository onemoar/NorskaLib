using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NorskaLib.UI
{
    /// <summary>
    /// Base class for UI elements such as HP Bars, names signs etc.
    /// which should follow the specific object on scene.
    /// </summary>
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class ObjectLabel : MonoBehaviour
    {
        public RectTransform Rect
        { get; private set; }
        public CanvasGroup Group
        { get; private set; }

        public int order = 0;
        public bool ignoreObstacles = true;
        [EnableIf("@!ignoreObstacles")]
        public LayerMask obstalceMask;

        [Space]
        [SerializeField] public bool shrinksOverDistance;
        [SerializeField] public float shrinkMinDistance = 5;
        [SerializeField] public float shrinkMaxDistance = 15;
        [SerializeField] public float sizeMax = 1.0f;
        [SerializeField] public float sizeMin = 0.5f;
        public float SizeDelta => sizeMax - sizeMin;

        [Space]
        [SerializeField] public bool fadesOverDistance;
        [SerializeField] public float fadeMinDistance = 5;
        [SerializeField] public float fadeMaxDistance = 15;

        [HideInInspector]
        public ObjectLabelEntry Entry
        { get; private set; }
        public Vector3 WorldPosition => Entry.Position;

        protected virtual void Awake()
        {
            Rect = GetComponent<RectTransform>();
            Group = GetComponent<CanvasGroup>();
        }

        protected virtual void Start() { }

        protected virtual void OnDestroy() { }

        public virtual void Setup(ObjectLabelEntry entry)
        {
            this.Entry = entry;
        }

        public struct SortMeta
        {
            public ObjectLabel label;
            public int order;
            public float distance;
        }
    }
}