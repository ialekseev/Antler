using System;

namespace SmartElk.Antler.Core.Common.Extensions
{
    public static class ArrayEx
    {
        public static T[] ForEach<T>(this T[] list, Action<T> action)
        {
            foreach (T item in list) action(item);
            return list;
        }
    }
}