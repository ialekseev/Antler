using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SmartElk.Antler.Common.Extensions
{
    public static class ObjectEx
    {
        public static IEnumerable<T> AsEnumerable<T>(this T obj)
        {
            if (obj != null)
                yield return obj;
        }

        public static IEnumerable<T> AsEnumerable<T>(this object obj)
        {
            if (obj != null && obj is T)
                yield return (T)obj;
        }

        public static T DeepClone<T>(this T obj)
        {
            return (T)obj.DeepClone(typeof(T));
        }

        public static T DeepCloneAs<T>(this T obj, Type type)
        {
            return (T)obj.DeepClone(type);
        }

        public static object DeepClone(this object obj, Type type)
        {
            var members = FormatterServices.GetSerializableMembers(type);
            var data = FormatterServices.GetObjectData(obj, members);
            var cloned = FormatterServices.GetSafeUninitializedObject(type);
            FormatterServices.PopulateObjectMembers
                (cloned, members, data);
            return cloned;
        }
    }
}