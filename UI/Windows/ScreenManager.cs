using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace NorskaLib.UI
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] Canvas canvas;

        #region Scale properties

        [SerializeField] CanvasScaler canvasScaler;

        [DisableInPlayMode][Tooltip("Смещение указателя, чтобы его не закрывал палец")]
        [SerializeField] Vector2 pointerDragOffset = new Vector2(22, 22);

        public Vector2 ScreenResolution
            => new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height);

        public float ScreenAspect
            => ScreenResolution.x / ScreenResolution.y;

        public Vector2 ReferenceResolution
            => canvasScaler.referenceResolution;

        public float ReferenceAspect
            => ReferenceResolution.x / ReferenceResolution.y;

        public Vector2 CurrentScale
            => ScreenResolution / ReferenceResolution;

        public Vector2 PointerOffset
            => pointerDragOffset * CurrentScale;

        #endregion

        #region Masks properties

        // Масками называются изображения (Image) на полотне интерфейса (Canvas)
        // которые полностью заполняют экран, закрывая только пространство сцены
        // (если указан MaskType.Scene) или все вместе с GUI (если указан MaskType.Full) 

        public enum MaskType
        {
            Full,
            Scene
        }

        Dictionary<MaskType, DoTweenGraphicColorizer> maskHandlers;
        Dictionary<MaskType, Image> maskImages;

        #endregion

        #region Screens properties

        private struct KeyWords
        {
            public const string Screens     = "Screens";

            public const string MaskFull    = "MaskFull";
            public const string MaskScene   = "MaskScene";
        }

        private RectTransform screensContainer;

        public const string ScreensFolder = "Screens";
        private Dictionary<System.Type, Screen> screens;
        private Dictionary<string, Screen> screensPrefabs;

        #endregion

        void Awake()
        {
            #region Common initialization

            var children = new Dictionary<string, RectTransform>()
            {
                { KeyWords.MaskScene,   null },
                { KeyWords.Screens,     null },
                { KeyWords.MaskFull,    null }
            };
            for (int i = 0; i < children.Count; i++)
            {
                var name = children.ElementAt(i).Key;

                var obj = new GameObject(name, typeof(RectTransform));
                var rect = obj.GetComponent<RectTransform>();
                rect.SetParent(this.transform);
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.localScale = Vector3.one;
                rect.localPosition = Vector3.zero;
                rect.localRotation = Quaternion.identity;

                children[name] = rect;
            }

            #endregion

            #region Masks initialization

            maskImages = new Dictionary<MaskType, Image>()
            {
                { MaskType.Full,  children[KeyWords.MaskFull].gameObject.AddComponent<Image>() },
                { MaskType.Scene,  children[KeyWords.MaskScene].gameObject.AddComponent<Image>() }
            };
            maskHandlers = new Dictionary<MaskType, DoTweenGraphicColorizer>()
            {
                { MaskType.Full,  new DoTweenGraphicColorizer(maskImages[MaskType.Full])},
                { MaskType.Scene,  new DoTweenGraphicColorizer(maskImages[MaskType.Scene])}
            };
            foreach (var pair in maskImages)
            {
                SetMaskAlpha(pair.Key, 0);
                SetMaskSprite(pair.Key, null);
                pair.Value.raycastTarget = false;
            }

            #endregion

            #region Screens initialization

            screensContainer    = children[KeyWords.Screens];
            screens             = new();
            screensPrefabs      = new();

            #endregion
        }

        void OnDestroy()
        {
            // Masks uninitialization
            if (maskHandlers != null)
                foreach (var h in maskHandlers)
                    h.Value?.Stop();
        }

        #region Screens 

        public Screen GetScreenPrefab(Type type, string prefabName = null)
        {
            var filename = !string.IsNullOrEmpty(prefabName)
                ? prefabName
                : type.Name;

            if (!screensPrefabs.TryGetValue(filename, out var prefab))
            {
                var path = $"{ScreensFolder}/{filename}";
                prefab = Resources.Load(path, type) as Screen;
                if (prefab != null)
                {
                    screensPrefabs.Add(filename, prefab);
                    return prefab;
                }
                else
                {
                    Debug.LogError($"Prefab for screen type '{type.Name}' named '{filename}' not found.");
                    return null;
                }
            }

            return prefab;
        }

        public T GetScreen<T>() where T : Screen
        {
            var type = typeof(T);

            if (screens.ContainsKey(type))
                return screens[type] as T;
            else
                return null;
        }

        public T ShowScreen<T>(ShowScreenMode mode = ShowScreenMode.Additive, int order = 0, bool animated = false, string prefabName = null) where T : Screen
        {
            var type = typeof(T);

            if (!screens.TryGetValue(type, out var screen) || screen == null)
            {
                var prefab = GetScreenPrefab(type, prefabName);
                screen = Instantiate(prefab, screensContainer);
                screen.Initialize(this);

                screens.Add(type, screen);
            }

            switch (mode)
            {
                default:
                case ShowScreenMode.Additive:
                    break;
                case ShowScreenMode.Single:
                    foreach (var s in screens)
                        if (s.Value != screen)
                            HideScreen(s.Value);
                    break;
                case ShowScreenMode.SoloInLayer:
                    foreach (var s in screens)
                        if (s.Value != screen && s.Value.Order == order)
                            HideScreen(s.Value);
                    break;
            }

            screen.Order = order;
            screen.Show(animated);

            UpdateScreensOrder();

            return screen as T;
        }
        public Screen ShowScreen(Screen screen, bool animated = false)
        {
            screen.Show(animated);

            return screen;
        }

        public void HideScreen<T>(bool animated = false, bool destroy = false) where T : Screen
        {
            var type = typeof(T);

            if (!screens.TryGetValue(type, out var screen))
                return;

            if (destroy)
                screens.Remove(type);
            screen.Hide(animated, destroy);
        }
        public void HideScreen(Screen screen, bool animated = false, bool destroy = false)
        {
            var type = screen.GetType();

            if (destroy)
                screens.Remove(type);
            screen.Hide(animated, destroy);
        }
        public void HideAll(bool animated = false, bool destroy = false)
        {
            foreach (var pair in screens)
                HideScreen(pair.Value, animated, destroy);
        }

        public void UpdateScreensOrder()
        {
            var screensSorted = screens.Values.OrderBy(s => s.Order).ToArray();
            for (int i = 0; i < screensSorted.Length; i++)
                screensSorted[i].Rect.SetSiblingIndex(i);
        }

        #endregion

        #region Masks

        public void SetMaskSprite(MaskType maskType, Sprite sprite)
        {
            maskImages[maskType].sprite = sprite;
        }

        public void SetMaskAlpha(MaskType maskType, float alpha, float duration = 0)
        {
            if (duration > 0)
                maskHandlers[maskType].Transit(alpha, duration);
            else
                maskHandlers[maskType].SetAlpha(alpha);
        }

        public void SetMaskColor(MaskType maskType, Color color, float duration = 0)
        {
            if (duration > 0)
                maskHandlers[maskType].Transit(color, duration);
            else
                maskHandlers[maskType].SetColor(color);
        }

        #endregion
    }
}