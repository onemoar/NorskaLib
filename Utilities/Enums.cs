using System;

namespace NorskaLib.Utilities
{
    public struct EnumUtils
    {
        public static T[] GetArray<T>() where T : System.Enum
        {
            var valuesArray = Enum.GetValues(typeof(T));
            var enumArray = new T[valuesArray.Length];
            for (int i = 0; i < valuesArray.Length; i++)
                enumArray[i] = (T)valuesArray.GetValue(i);

            return enumArray;
        }
    }
}
