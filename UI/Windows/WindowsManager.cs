using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace NorskaLib.UI
{
    public class WindowsManager : MonoBehaviour
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

        //Dictionary<MaskType, DoTweenGraphicColorizer> maskHandlers;
        Dictionary<MaskType, Image> maskImages;

        #endregion

        #region Windows properties

        private struct KeyWords
        {
            public const string Windows     = "Windows";

            public const string MaskFull    = "MaskFull";
            public const string MaskScene   = "MaskScene";
        }

        private RectTransform windowsContainer;

        public const string WindowsFolder = "Windows";
        private Dictionary<System.Type, Window> windows;
        private Dictionary<string, Window> windowsPrefabs;

        #endregion

        void Awake()
        {
            #region Common initialization

            var children = new Dictionary<string, RectTransform>()
            {
                { KeyWords.MaskScene,   null },
                { KeyWords.Windows,     null },
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
            //maskHandlers = new Dictionary<MaskType, DoTweenGraphicColorizer>()
            //{
            //    { MaskType.Full,  new DoTweenGraphicColorizer(maskImages[MaskType.Full])},
            //    { MaskType.Scene,  new DoTweenGraphicColorizer(maskImages[MaskType.Scene])}
            //};
            foreach (var pair in maskImages)
            {
                SetMaskAlpha(pair.Key, 0);
                SetMaskSprite(pair.Key, null);
                pair.Value.raycastTarget = false;
            }

            #endregion

            #region Windows initialization

            windowsContainer    = children[KeyWords.Windows];
            windows             = new();
            windowsPrefabs      = new();

            UI.Events.onWindowOrderChanged += OnWindowOrderChanged;

            #endregion
        }

        void OnDestroy()
        {
            // Masks deinitialization
            //if (maskHandlers != null)
            //    foreach (var h in maskHandlers)
            //        h.Value?.Stop();

            // Screen deinitialization
            UI.Events.onWindowOrderChanged -= OnWindowOrderChanged;
        }

        #region Windows 

        public Window GetWindowPrefab(Type type, string prefabName = null)
        {
            var filename = !string.IsNullOrEmpty(prefabName)
                ? prefabName
                : type.Name;

            if (windowsPrefabs.TryGetValue(filename, out var prefab))
                return prefab;

            var path = $"{WindowsFolder}/{filename}";
            prefab = Resources.Load(path, type) as Window;
            if (prefab != null)
                windowsPrefabs.Add(filename, prefab);
            else
                Debug.LogError($"Prefab for screen type '{type.Name}' named '{filename}' not found.");

            return prefab;
        }

        public W GetWindow<W>() where W : Window
        {
            var type = typeof(W);

            if (windows.ContainsKey(type))
                return windows[type] as W;
            else
                return null;
        }

        public W ShowWindow<W>(ShowWindowMode mode = ShowWindowMode.Additive, int order = 0, bool animated = false, string prefabName = null) where W : Window
        {
            var type = typeof(W);

            if (!windows.TryGetValue(type, out var window) || window == null)
            {
                var prefab = GetWindowPrefab(type, prefabName);
                window = Instantiate(prefab, windowsContainer);

                windows.Add(type, window);
            }

            switch (mode)
            {
                default:
                case ShowWindowMode.Additive:
                    break;

                case ShowWindowMode.Single:
                    foreach (var s in windows)
                        if (s.Value != window)
                            HideWindow(s.Value);
                    break;

                case ShowWindowMode.SoloInLayer:
                    foreach (var s in windows)
                        if (s.Value != window && s.Value.Order == order)
                            HideWindow(s.Value);
                    break;
            }

            window.Order = order;
            window.Show(animated);

            UpdateWindowsOrder();

            return window as W;
        }
        public Window ShowWindow(Window screen, bool animated = false)
        {
            screen.Show(animated);

            return screen;
        }

        public void HideWindow<W>(bool animated = false, bool destroy = false) where W : Window
        {
            var type = typeof(W);

            if (!windows.TryGetValue(type, out var window))
                return;

            if (destroy)
                windows.Remove(type);
            window.Hide(animated, destroy);
        }
        public void HideWindow(Window window, bool animated = false, bool destroy = false)
        {
            var type = window.GetType();

            if (destroy)
                windows.Remove(type);
            window.Hide(animated, destroy);
        }
        public void HideAll(bool animated = false, bool destroy = false)
        {
            foreach (var pair in windows)
                HideWindow(pair.Value, animated, destroy);
        }

        private void OnWindowOrderChanged(Window source, int order)
        {
            if (!windows.ContainsValue(source))
                return;

            UpdateWindowsOrder();
        }

        public void UpdateWindowsOrder()
        {
            var windowsSorted = windows.Values.OrderBy(s => s.Order).ToArray();
            for (int i = 0; i < windowsSorted.Length; i++)
                windowsSorted[i].Rect.SetSiblingIndex(i);
        }

        #endregion

        #region Masks

        public void SetMaskSprite(MaskType maskType, Sprite sprite)
        {
            maskImages[maskType].sprite = sprite;
        }

        public void SetMaskAlpha(MaskType maskType, float alpha, float duration = 0)
        {
            //if (duration > 0)
            //    maskHandlers[maskType].Transit(alpha, duration);
            //else
            //    maskHandlers[maskType].SetAlpha(alpha);
        }

        public void SetMaskColor(MaskType maskType, Color color, float duration = 0)
        {
            //if (duration > 0)
            //    maskHandlers[maskType].Transit(color, duration);
            //else
            //    maskHandlers[maskType].SetColor(color);
        }

        #endregion
    }
}