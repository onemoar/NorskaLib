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
        public static ScreenManager Instance { get; private set; }

        [SerializeField] bool singleInstance;

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

        private RectTransform screensContainer;

        public Screen ActiveScreen { get; private set; }

        public enum ScreenModes
        {
            /// <summary>
            /// При открытии экрана в этом режиме все остальные экраны будут скрыты.
            /// </summary>
            Single,
            /// <summary>
            /// При открытии экрана в этом режиме прочие экраны не будут затронуты.
            /// </summary>
            Additive,
            /// <summary>
            /// При открытии экрана в этом режиме экраны с таким же
            /// порядковым номером (GUIScreen.Order) будут скрыты
            /// </summary>
            SoloInLayer
        }
        public const string screensLoadpath = "Screens";

        private Dictionary<System.Type, Screen> screensCached;

        #endregion

        void Awake()
        {
            #region Singleton initialization

            if (singleInstance)
            {
                if (Instance != null)
                {
                    Destroy(this.gameObject);
                    return;
                }
                DontDestroyOnLoad(this.gameObject);
            }

            Instance = this;

            #endregion

            #region Common initialization

            var children = new Dictionary<string, RectTransform>()
            {
                { "MaskScene", null },
                { "Labels", null },
                { "Screens", null },
                { "MaskFull", null }
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
                { MaskType.Full,  children["MaskFull"].gameObject.AddComponent<Image>() },
                { MaskType.Scene,  children["MaskScene"].gameObject.AddComponent<Image>() }
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

            screensContainer = children["Screens"];
            screensCached = new Dictionary<System.Type, Screen>();

            #endregion
        }

        void Start()
        {

        }

        void OnDestroy()
        {
            // Masks uninitialization
            if (maskHandlers != null)
                foreach (var h in maskHandlers)
                    h.Value?.Stop();
        }

        void FixedUpdate()
        {

        }

        #region Screens 

        public T GetScreen<T>() where T : Screen
        {
            if (screensCached.ContainsKey(typeof(T)))
                return (T)screensCached[typeof(T)];
            else
                return null;
        }

        public T ShowScreen<T>(ScreenModes mode = ScreenModes.Additive, int order = 0, bool animated = false, string prefabName = null) where T : Screen
        {
            T screen;

            if (screensCached.ContainsKey(typeof(T)))
            {
                screen = (T)screensCached[typeof(T)];
            }
            else
            {
                var filename = !string.IsNullOrEmpty(prefabName)
                    ? prefabName
                    : typeof(T).Name;

                //Debug.Log($"Loading screen by path: Assets/Resources/{loadpath}/{filename}");

                var screenPref = Resources.Load<T>($"{screensLoadpath}/{filename}");
                screen = Instantiate(screenPref, screensContainer);

                screensCached.Add(typeof(T), screen);
            }

            switch (mode)
            {
                default:
                case ScreenModes.Additive:
                    break;
                case ScreenModes.Single:
                    foreach (var s in screensCached)
                        if (s.Value != screen)
                            HideScreen(s.Value);
                    break;
                case ScreenModes.SoloInLayer:
                    foreach (var s in screensCached)
                        if (s.Value != screen && s.Value.Order == order)
                            HideScreen(s.Value);
                    break;
            }

            ActiveScreen = screen;
            screen.Order = order;
            screen.Show(animated);

            UpdateScreensOrder();

            return screen;
        }

        public void ShowScreen(Screen screen, bool animated = false)
        {
            screen.Show(animated);
        }

        public void HideScreen<T>(bool animated = false) where T : Screen
        {
            T screen;

            if (screensCached.ContainsKey(typeof(T)))
            {
                screen = (T)screensCached[typeof(T)];
                HideScreen(screen, animated);
            }
            else
                return;
        }
        public void HideScreen(Screen screen, bool animated = false)
        {
            screen.Hide(animated);
        }
        public void HideAll(bool animated = false)
        {
            foreach (var s in screensCached)
                HideScreen(s.Value, animated);
        }

        public void DestroyScreen<T>() where T : Screen
        {
            T screen;

            if (screensCached.ContainsKey(typeof(T)))
            {
                screen = (T)screensCached[typeof(T)];
                screensCached.Remove(typeof(T));

                if (screen != null)
                    Destroy(screen.gameObject);
            }
            else
                return;
        }
        public void DestroyScreen(Screen screen)
        {
            var type = screen.GetType();

            if (screensCached.ContainsKey(type))
                screensCached.Remove(type);

            Destroy(screen.gameObject);
        }
        public void DestroyAll()
        {
            foreach (var s in screensCached)
                Destroy(s.Value.gameObject);

            screensCached.Clear();
        }

        public void UpdateScreensOrder()
        {
            var screensSorted = screensCached.Values.OrderBy(s => s.Order).ToArray();
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