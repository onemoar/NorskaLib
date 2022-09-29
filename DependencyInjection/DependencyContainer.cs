using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NorskaLib.DependencyInjection
{
    public class DependencyContainer
    {
        #region Static

        public static DependencyContainer Instance { get; private set; }

        public DependencyContainer()
        {
            Instance = this;
        }

        #endregion

        private readonly Dictionary<Type, object> instances = new();

        public void RegisterInstance<T>(T instance) where T : class
        {
            var type = typeof(T);
            RegisterInstance(instance, type);
        }

        public void RegisterInstance(object instance, Type type)
        {
            if (!instances.ContainsKey(type))
                instances.Add(type, instance);
            else
                instances[type] = instance;
        }

        public void UnregisterInstance<T>(T instance) where T : class
        {
            var type = typeof(T);
            if (!instances.TryGetValue(type, out var registredInstance) || registredInstance != instance)
                return;
            else
                instances[type] = null;
        }

        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            return Resolve(type) as T;
        }

        private object Resolve(Type type)
        {
            if (instances.TryGetValue(type, out var instance))
                return instance;

            foreach (var registredType in instances.Keys)
                if (type.IsAssignableFrom(registredType))
                    return instances[registredType];

            Debug.LogWarning($"Couldn't resolve dependency for type {type}");
            return null;
        }

        public void BuildUp(object target)
        {
            var targetType = target.GetType();
            var dependenciesFieldsInfos = targetType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fi => Attribute.IsDefined(fi, typeof(DependencyAttribute)));

            foreach (var dependency in dependenciesFieldsInfos)
            {
                var value = Resolve(dependency.FieldType);
                dependency.SetValue(target, value);
            }
        }
    }
}