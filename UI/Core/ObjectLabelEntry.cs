using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.UI
{
    /// <summary>
    /// Base class for a component of GameObjects, that should be followed
    /// by a UI element such as name sign or HP Bar (see ObjectLabel).
    /// </summary>
    public abstract class ObjectLabelEntry : MonoBehaviour
    {
        public static List<ObjectLabelEntry> instances = new List<ObjectLabelEntry>();
        public static Action<ObjectLabelEntry> onEntryAdded = (ObjectLabelEntry entry) => { };
        public static Action<ObjectLabelEntry> onEntryRemoved = (ObjectLabelEntry entry) => { };

        public abstract Vector3 Position { get; }

        public bool isHidden;
        public bool IsActive => gameObject.activeInHierarchy;

        protected virtual void Start()
        {
            RegisterInstance();
        }

        protected virtual void OnDestroy()
        {
            UnregisterInstance();
        }

        protected void RegisterInstance()
        {
            //Debug.LogWarning($"Regstring label entry for {transform.parent.name}", this);

            instances.Add(this);
            onEntryAdded.Invoke(this);
        }

        protected void UnregisterInstance()
        {
            instances.Remove(this);
            onEntryRemoved.Invoke(this);
        }
    }
}