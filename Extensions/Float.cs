using System.Collections;
using System.Collections.Generic;

namespace NorskaLib.Extensions
{
    public static class FloatExtensions
    {
        public static float Square(this float value)
        {
            return value * value;
        }

        public static float Cube(this float value)
        {
            return value * value * value;
        }

        public static bool IsBetween(this float value, float min, float max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            static bool CompareToMin(float value, float min, bool exclusive)
            {
                return exclusive ? value > min : value >= min;
            }

            static bool CompareToMax(float value, float max, bool exclusive)
            {
                return exclusive ? value < max : value <= max;
            }

            return CompareToMin(value, min, exclusiveMin) && CompareToMax(value, max, exclusiveMax);
        }
    }
}