using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.Extensions
{
    public static class Vector3Extensions
    {
        public static bool ApproximatelyZero(this Vector3 vector)
        {
            return vector.x.ApproximatelyZero() && vector.y.ApproximatelyZero() && vector.z.ApproximatelyZero();
        }

        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }
        public static Vector3 WithXY(this Vector3 vector, float x, float y)
        {
            return new Vector3(x, y, vector.z);
        }
        public static Vector3 WithXZ(this Vector3 vector, float x, float z)
        {
            return new Vector3(x, vector.y, z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
        public static Vector3 WithYZ(this Vector3 vector, float y, float z)
        {
            return new Vector3(vector.x, y, z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector2 ToXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}