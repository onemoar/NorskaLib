﻿using NorskaLib.Utilities;
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

        // TO DO: add optional origin offset parameter
        public static Vector2 Snap(this Vector2 position, Vector2 cellSize)
        {
            var cellCount = Vector2Utils.RoundToInt(Vector2Utils.ComponentDiv(position, cellSize));
            return ComponentMult(cellCount, cellSize);
        }
        public static Vector2 Snap(this Vector2 position, float cellSizeUnitform = 1)
        {
            return Snap(position, Vector2Utils.Uniform(cellSizeUnitform));
        }
    }
}