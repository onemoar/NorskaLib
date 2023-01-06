using UnityEngine;

namespace NorskaLib.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 ComponentMult(this Vector2 a, Vector2 b)
        {
            return a * b;
        }

        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        public static Vector3 FromXZ(this Vector2 vector, float y = 0)
        {
            return new Vector3(vector.x, y, vector.y);
        }
    }
}