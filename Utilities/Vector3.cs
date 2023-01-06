using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.Utilities
{
    public struct Vector3Utils
    {
        public static Vector3 ComponentMult (Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }

        public static Vector3 Uniform(float value)
        {
            return new Vector3(value, value, value);
        }
    }

    public struct Vector2Utils
    {
        public static Vector2 ComponentMult(Vector2 a, Vector2 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y);
        }

        public static bool Approximately(Vector2 a, Vector2 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
        }

        public static Vector2 Uniform(float value)
        {
            return new Vector2(value, value);
        }
    }
}
