using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NorskaLib.DependencyInjection;

namespace NorskaLib.Storage
{
    public abstract class LocalStorage
    {
        protected IStorageModule[] modulesShared;
        protected IStorageModule[] modulesSlot;

        public abstract string ActiveSlot { get; }

        public Action<string> onSlotChanged;

        public bool IsInitialized { get; private set; }

        public void Initialize(IEnumerable<Type> allModulesTypes)
        {
            void RegisterModules(ref IStorageModule[] collection, bool needSharedAttribute)
            {
                var types = allModulesTypes
                    .Where(mt => needSharedAttribute 
                        ? Attribute.IsDefined(mt, typeof(SharedAttribute)) 
                        : !Attribute.IsDefined(mt, typeof(SharedAttribute)))
                    .ToArray();
                collection = new IStorageModule[types.Length];
                for (int i = 0; i < collection.Length; i++)
                {
                    var moduleType = types[i];
                    var moduleInstance = Activator.CreateInstance(moduleType) as IStorageModule;
                    DependencyContainer.Instance.RegisterInstance(moduleInstance, moduleType);

                    collection[i] = moduleInstance;
                }
            }

            RegisterModules(ref modulesShared, true);
            RegisterModules(ref modulesSlot, false);

            IsInitialized = true;
        }

        public abstract void LoadShared(string name);

        public abstract void LoadSlot(string name);

        public abstract void SaveShared(string name);

        public abstract void SaveSlot(string name);

        public abstract void DeleteSlot(string name);

        /// <summary>
        /// Saves current loaded slot.
        /// </summary>
        public void SaveSlot()
        {
            if (string.IsNullOrEmpty(ActiveSlot))
            {
                Debug.LogError($"No slot loaded!");
                return;
            }

            SaveSlot(ActiveSlot);
        }
    }
}