using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.Extensions
{
    public static class IListExtensions
    {
        public static bool IndexIsValid(this IList list, int index)
        {
            return index >= 0 && index <= list.Count - 1;
        }

        public static void Shuffle<T>(this IList<T> instance)
        {
            int n = instance.Count;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, n + 1);

                T buffer = instance[k];
                instance[k] = instance[n];
                instance[n] = buffer;
            }
        }

        public static IList<T> ShuffledCopy<T>(this IList<T> instance)
        {
            var copy = new List<T>(instance);

            int n = copy.Count;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, n + 1);

                T buffer = copy[k];
                copy[k] = copy[n];
                copy[n] = buffer;
            }

            return copy;
        }

        public static T Random<T>(this IList<T> list)
        {
            var index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        /// <returns> FALSE - if next index is out of range. </returns>
        //public static bool TryGetNext<T>(this IList<T> list, int index, bool loop, out T element)
        //{
        //    if (!index.IsBetween(0, list.Count))
        //        throw new System.ArgumentOutOfRangeException($"Index '{index}' is out of range '{0}''{list.Count}'");

        //}

        /// <returns> FALSE - if next index is out of range. </returns>
        //public static bool TryGetPrevious<T>(this IList<T> list, int index, bool loop, out T element)
        //{
        //    if (!index.IsBetween(0, list.Count))
        //        throw new System.ArgumentOutOfRangeException($"Index '{index}' is out of range '{0}''{list.Count}'");


        //}

        public static bool TryGetNextIndex<T>(this IList<T> list, int index, bool loop, out int nextIndex)
        {
            if (!index.IsBetween(0, list.Count))
                throw new System.ArgumentOutOfRangeException($"Index '{index}' is out of range '{0}''{list.Count}'");


            nextIndex = loop
                ? index + 1 >= list.Count
                    ? 0
                    : index + 1
                : index + 1;

            return nextIndex >= list.Count;
        }

        public static bool TryGetPrevIndex<T>(this IList<T> list, int index, bool loop, out int prevIndex)
        {
            if (!index.IsBetween(0, list.Count))
                throw new System.ArgumentOutOfRangeException($"Index '{index}' is out of range '{0}''{list.Count}'");

            prevIndex = loop
                ? index - 1 < 0
                    ? list.Count - -1
                    : index - 1
                : index - 1;

            return prevIndex < 0;
        }
    }
}