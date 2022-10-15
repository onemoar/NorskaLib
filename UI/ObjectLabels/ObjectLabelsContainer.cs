using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NorskaLib.Utilities;

namespace NorskaLib.UI
{
    public class ObjectLabelsContainer : MonoBehaviour
    {
        //[Dependency] IGameplayCameraProvider CameraProvider;
        //private Camera Camera => CameraProvider.HeroviewCamera;
        private Camera Camera => Camera.main;

        [SerializeField] RectTransform rect;

        [Space]
        [SerializeField] bool labelsCanShrinkOverDistance = true;
        [SerializeField] bool labelsCanFadeOverDistance = true;


        //[Header("Labels prefabs")]
        //[SerializeField] HealthbarLabel healthbarPrefab;

        public bool hideAll;

        public const string loadpath = "CanvasLabels";

        private Dictionary<Type, string> entryTypesToPrefabsNames;

        private Dictionary<ObjectLabelEntry, ObjectLabel> labels;

        void Awake()
        {
            //ContainerHolder.Container.BuildUp(this);

            labels = new Dictionary<ObjectLabelEntry, ObjectLabel>();

            entryTypesToPrefabsNames = new Dictionary<Type, string>()
            {
                //{ typeof(HealthbarEntry), "Healthbar" }
            };
        }

        void Start()
        {
            foreach (var entry in ObjectLabelEntry.instances)
                CreateLabel(entry);

            ObjectLabelEntry.onEntryAdded += CreateLabel;
            ObjectLabelEntry.onEntryRemoved += DestoyLabel;
        }

        void OnDestroy()
        {
            ObjectLabelEntry.onEntryAdded -= CreateLabel;
            ObjectLabelEntry.onEntryRemoved -= DestoyLabel;
        }

        void FixedUpdate()
        {
            UpdateLabels();
        }

        private void CreateLabel(ObjectLabelEntry entry)
        {
            //Debug.Log($"Creating label for {entry.transform.parent.name}...", entry);
            if (labels.ContainsKey(entry))
            {
                Debug.LogWarning($"Labels dictionary already contains key for {entry.name}", entry);
                return;
            }

            var filename = entryTypesToPrefabsNames[entry.GetType()];

            //Debug.Log($"Loading labels pref by path: Assets/Resources/{labelsLoadpath}/{filename}");

            var labelPref = Resources.Load<ObjectLabel>($"{loadpath}/{filename}");

            var label = Instantiate(labelPref, rect);
            label.Rect.anchorMin = Vector2.zero; //new Vector2(0.5f, 0.5f);
            label.Rect.anchorMax = Vector2.zero; //new Vector2(0.5f, 0.5f);
#if UNITY_EDITOR
            label.name = $"{label.GetType().Name} ({entry.transform.parent.name})";
#endif

            label.Setup(entry);
            labels.Add(entry, label);

            UpdateLabels();
        }

        public ObjectLabel GetLabel(ObjectLabelEntry entry)
            => labels.ContainsKey(entry) ? labels[entry] : null;

        private void DestoyLabel(ObjectLabelEntry entry)
        {
            var label = labels[entry];
            Destroy(label.gameObject);

            labels.Remove(entry);
        }

        private void UpdateLabels()
        {
            if (Camera == null)
                return;

            var cameraSize = new Vector2(
                Camera.pixelWidth,
                Camera.pixelHeight);
            var rectSize = rect.sizeDelta;

            var labelsToSort = new List<ObjectLabel.SortMeta>();

            foreach (var item in labels)
            {
                var label = item.Value;

                #region Для обычной камеры

                //var cameraSize = new Vector2(MainCamera.pixelWidth, MainCamera.pixelHeight);
                //var rectSize = labelsContainer.sizeDelta;

                #endregion

                var direction = label.WorldPosition - Camera.transform.position;
                var dotProduct = Vector3.Dot(direction.normalized, Camera.transform.forward);
                var inVision = dotProduct >= 0.1f;

                var visionIsBlocked = !label.ignoreObstacles 
                    && Physics.Linecast(
                        label.WorldPosition, 
                        Camera.transform.position, 
                        label.obstalceMask);

                var show = inVision
                    && !hideAll
                    && !visionIsBlocked
                    && !label.Entry.isHidden
                    && label.Entry.IsActive;

                if (show)
                {
                    if (!label.gameObject.activeInHierarchy)
                        label.gameObject.SetActive(true);

                    var distance = direction.magnitude;

                    #region Для камеры, рендерящей в текстуру (как в этом проекте)

                    var rectPosition = Utils.WorldToRectPosition(Camera, rect, label.WorldPosition);

                    #endregion

                    #region Для обычной камеры

                    //var screenPoint = RectTransformUtility.WorldToScreenPoint(MainCamera, label.WorldPosition);
                    //RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    //    labelsContainer,
                    //    screenPoint,
                    //    null,
                    //    out var rectPosition);

                    #endregion

                    label.Rect.anchoredPosition = rectPosition;
                    if (labelsCanShrinkOverDistance && label.shrinksOverDistance)
                    {
                        var shrinkFactor = Mathf.InverseLerp(
                            label.shrinkMinDistance,
                            label.shrinkMaxDistance, 
                            distance);
                        var scale = label.sizeMax - label.SizeDelta * shrinkFactor;
                        label.Rect.localScale = new Vector3(scale, scale, scale);
                    }
                    else
                    {
                        label.Rect.localScale = Vector3.one;
                    }

                    if (labelsCanFadeOverDistance && label.fadesOverDistance)
                    {
                        var fadeFactor = Mathf.InverseLerp(
                            label.fadeMinDistance, 
                            label.fadeMaxDistance, distance);
                        label.Group.alpha = 1 - fadeFactor;
                    }
                    else
                    {
                        label.Group.alpha = 1;
                    }

                    labelsToSort.Add(new ObjectLabel.SortMeta()
                    {
                        label = label,
                        order = label.order,
                        distance = distance
                    });
                }
                else
                {
                    if (label.gameObject.activeInHierarchy)
                        label.gameObject.SetActive(false);
                }

            }

            if (labelsToSort.Count > 1)
            {
                labelsToSort = labelsToSort.OrderBy(m => m.order).ThenBy(m => m.distance).ToList();
                for (int i = 0; i < labelsToSort.Count; i++)
                    labelsToSort[i].label.Rect.SetSiblingIndex(i);
            }
        }
    }
}
