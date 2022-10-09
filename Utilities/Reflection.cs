using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NorskaLib.Utilities
{
    public struct ReflectionUtils
    {
        public static IEnumerable<Type> GetSubclasses<T>(bool includeAbstract) where T : class
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(t => t.IsClass && (includeAbstract && t.IsAbstract) && t.IsSubclassOf(typeof(T)));
        }

        public static IEnumerable<Type> GetAssignables<T>(Assembly assembly = null)
        {
            return (assembly ?? Assembly.GetAssembly(typeof(T))).GetTypes()
                .Where(t => t != typeof(T) && typeof(T).IsAssignableFrom(t));
        }
    }
}