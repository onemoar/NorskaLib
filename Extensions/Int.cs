using System.Collections;
using System.Collections.Generic;

namespace NorskaLib.Extensions
{
    public static class IntExtensions
    {
        public static int Square(this int value)
        {
            return value * value;
        }

        public static int Cube(this int value)
        {
            return value * value * value;
        }

        public static bool IsBetween(this int value, int min, int max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            return exclusiveMin ? value > min : value >= min
                && exclusiveMax ? value < max : value <= max;
        }
    }
}