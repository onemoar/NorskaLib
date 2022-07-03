using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Game
{
    public class TimeController : MonoBehaviour
    {
        public static Action<float> onTimeScaleChanged = (value) => { };

        public static TimeController Instance { get; private set; }


        private float timeScale = 1;
        public float TimeScale
        {
            get => timeScale;

            private set
            {
                if (!Mathf.Approximately(value, timeScale))
                    onTimeScaleChanged.Invoke(value);
                timeScale = value;
            }
        }

        void Awake()
        {
            Instance = this;

            requests = new List<TimescaleRequest>(3);
        }

        void Update()
        {
            bool useLerp;
            float targetScale;

            if (requests.Count > 0)
            {
                var removedCount = requests.RemoveAll(r => 
                    r.source == null 
                    || (r.releaseOnDisabled && !r.source.activeInHierarchy));
                if (removedCount > 0)
                    SetupLerp();

                var currentRequest = requests.FirstOrDefault();
                if (currentRequest is null)
                {
                    // Reverting default timescale
                    useLerp = true;
                    targetScale = defaultScale;
                }
                else
                {
                    useLerp = currentRequest.soft;
                    targetScale = currentRequest.value;
                }
            }
            else
            {
                useLerp = true;
                targetScale = defaultScale;
            }

            if (useLerp)
            {
                if (lerpTimer < lerpDuration)
                    lerpTimer += Time.deltaTime;

                var t = lerpTimer / lerpDuration;
                TimeScale = Mathf.Lerp(lerpInitValue, targetScale, t);
            }
            else
            {
                TimeScale = targetScale;
            }
        }

        [SerializeField] float lerpDuration   = 0.5f;
        private float lerpInitValue           = 1f;
        private float lerpTimer               = 0f;

        private void SetupLerp()
        {
            lerpInitValue   = timeScale;
            lerpTimer       = 0f;
        }

        #region Default scale management

        private float defaultScale = 1;
        public void SetDefaultScale(float value)
        {
            value = Mathf.Clamp(value, 0, 2);
            defaultScale = value;

            SetupLerp();
        }

        #endregion

        #region Requests management

        private List<TimescaleRequest> requests;

        public class TimescaleRequest
        {
            public readonly GameObject source;
            public readonly float value;
            public readonly bool releaseOnDisabled;
            public readonly bool soft;

            public TimescaleRequest(GameObject source, float value, bool soft = true, bool releaseOnDisabled = true)
            {
                this.source = source;
                this.value = value;
                this.releaseOnDisabled = releaseOnDisabled;
                this.soft = soft;
            }  
        }

        /// <summary>
        /// If this source is already present in request queue, it will be replaced.
        /// </summary>
        /// <param name="request"></param>
        public void RequireScale(TimescaleRequest request)
        {
            var clone = requests.FirstOrDefault(r => r.source == request.source);
            if (clone != null)
                requests.Remove(clone);

            requests.Add(request);
            UpdateRequestsSorting();
        }

        /// <summary>
        /// Discarding request from this source.
        /// </summary>
        /// <param name="source"></param>
        public void UnrequireScale(GameObject source)
        {
            var request = requests.FirstOrDefault(r => r.source == source);
            if (request is null)
                return;

            requests.Remove(request);
            UpdateRequestsSorting();
        }

        private void UpdateRequestsSorting()
        {
            requests = requests.OrderBy(r => r.value).ToList();
        }

        #endregion

        #region Debugging
#if UNITY_EDITOR

        [Header("Debugging")]

        [Range(0,2)][DisableIf("@true")]
        public float currentScaleView;
        [Range(0, 2)][DisableIf("@true")]
        public float defaultScaleView;

        [DisableIf("@true")]
        public float lerpTimerView;

        [DisableIf("@true")]
        public TimescaleRequestView[] requestsView;

        [System.Serializable]
        public struct TimescaleRequestView
        {
            public GameObject source;
            public float value;
            public bool releaseOnDisabled;
            public bool soft;

            public TimescaleRequestView(TimescaleRequest request)
            {
                this.source = request.source;
                this.value = request.value;
                this.releaseOnDisabled = request.releaseOnDisabled;
                this.soft = request.soft;
            }
        }

        void LateUpdate()
        {
            currentScaleView = timeScale;
            defaultScaleView = defaultScale;
            lerpTimerView = lerpTimer;

            requestsView = requests.Select(r => new TimescaleRequestView(r)).ToArray();
        }

#endif
        #endregion
    }
}
