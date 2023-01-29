using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NorskaLib.Pools
{
    public abstract class ComponentPool<C> : MonoBehaviour where C : Component
    {
        [Tooltip("Preffered amount of instances in the pool. " +
            "If an instances gets deacllocated and pool already has given amount of instances, " +
            "it will be destroyed, rather than disabled.")]
        public int capacity = 8;

        [Tooltip("Original instance (prefab)")]
        public C original;

        [Space]

        [Tooltip("Transform that serves as a parent for deallocated instances")]
        public Transform container;

        [Tooltip("If set, game objects will be activated/deactivated upon allocation/deallocation.")]
        public bool disablePooled = true;

        [Tooltip("Instances that must be loaded with scene and moved into the pool.")]
        [SerializeField] C[] prewarmedInstances;

        private Stack<C> stack;

        public C Allocate()
        {
            var instance = stack.Count > 0
                ? stack.Pop()
                : Instantiate(original);

            if (disablePooled)
                instance.gameObject.SetActive(true);

            if (instance is IPoolable poolable)
                poolable.OnAllocated();

            return instance;
        }

        public void Deallocate(C instance)
        {
            if (instance is IPoolable poolable)
                poolable.OnDeallocated();

            if (stack.Count < capacity)
            {
                stack.Push(instance);
                instance.transform.SetParent(container);

                if (disablePooled)
                    instance.gameObject.SetActive(false);
            }
            else
            {
                Destroy(instance.gameObject);
            }
        }

        #region MonoBehaviour

        protected virtual void Awake()
        {
            stack = new Stack<C>(capacity);

            var prewarmedTake = prewarmedInstances.Take(capacity);

            if (disablePooled)
                foreach (var instance in prewarmedInstances)
                    instance.gameObject.SetActive(false);

            if (prewarmedInstances.Length > capacity)
                Debug.Log($"Prewarmed instances count in {this.name} ({this.GetType().Name}) is bigger than the pool's capacity. Some of these instances will never be used.");

            foreach (var instance in prewarmedTake)
                stack.Push(instance);
        }

        #endregion

    }
}