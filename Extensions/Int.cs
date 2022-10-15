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
            static bool CompareToMin(int value, int min, bool exclusive)
            {
                return exclusive ? value > min : value >= min;
            }

            static bool CompareToMax(int value, int max, bool exclusive)
            {
                return exclusive ? value < max : value <= max;
            }

            return CompareToMin(value, min, exclusiveMin) && CompareToMax(value, max, exclusiveMax);
        }
    }
}