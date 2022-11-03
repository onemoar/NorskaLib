using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.Utilities
{
    public struct RandomUtils
    {
        public static bool Bool(float chance, out float dice)
        {
            dice = Random.Range(0, 0.99f);
            return dice < Mathf.Clamp01(chance);
        }
        public static bool Bool(float chance)
        {
            return Bool(chance, out var dice);
        }

        public static int Range(int min, int max, int exeption)
        {
            var pool = new List<int>();
            for (int i = min; i < max; i++)
                if (i != exeption)
                    pool.Add(i);

            var index = UnityEngine.Random.Range(0, pool.Count);

            return pool[index];
        }
        public static int Range(int min, int max, int[] exeptions)
        {
            var pool = new List<int>();
            for (int i = min; i < max; i++)
            {
                var exclude = false;
                for (int j = 0; j < exeptions.Length; j++)
                    if (i == exeptions[j])
                    {
                        exclude = true;
                        break;
                    }

                if (!exclude)
                    pool.Add(i);
            }

            var index = UnityEngine.Random.Range(0, pool.Count);

            return pool[index];
        }
        public static int RangeInclusive(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }

        public static Vector3 Vector3Normalized()
        {
            return Vector3(-1, 1, -1, 1, -1, 1).normalized;
        }
        public static Vector3 Vector3(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            var x = UnityEngine.Random.Range(minX, maxX);
            var y = UnityEngine.Random.Range(minY, maxY);
            var z = UnityEngine.Random.Range(minZ, maxZ);

            return new Vector3(x, y, z);
        }

        public static T Value<T>(IList<Meta<T>> metas)
        {
            var weights = metas.Select(m => m.weight).ToArray();
            var index = Index(weights);
            return metas[index].value;
        }
        public static T Value<T>(Meta<T>[] metas)
        {
            var weights = metas.Select(m => m.weight).ToArray();
            var index = Index(weights);
            return metas[index].value;
        }
        public static int Index(float[] weigths)
        {
            float weightsSum = 0;
            for (int i = 0; i < weigths.Length; i++)
                weightsSum += weigths[i];

            float roll = UnityEngine.Random.Range(0, weightsSum);

            float lastMin = 0;
            float lastMax = 0;
            int index = -1;
            for (int i = 0; i < weigths.Length; i++)
            {
                if (i > 0)
                    lastMin += weigths[i - 1];

                lastMax += weigths[i];

                if (roll >= lastMin && roll < lastMax && !Mathf.Approximately(weigths[i], 0))
                    index = i;
            }
            return index;
        }

        public struct Meta<T>
        {
            public T value;
            public float weight;
        }
    }
}
