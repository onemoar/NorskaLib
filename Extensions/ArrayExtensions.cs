using System;

namespace NorskaLib.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IndexIsValid(this Array array, int index)
        {
            return index >= 0 && index <= array.Length - 1;
        }
    }
}