using System;
using System.Collections.Generic;

namespace SmartElk.Antler.Core.Common.Extensions
{
    public static class EnumerableEx
    {        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }
    }
}
