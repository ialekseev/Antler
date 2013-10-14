using System;

namespace SmartElk.Antler.Common.Reflection
{
    public static class TypeEx
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type generic)
        {
            while (type != null & type != typeof(object))
            {
                var intermediate = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == intermediate)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static bool IsRawGeneric(this Type type, Type generic)
        {
            if (type == null || generic == null) return false;

            return type.IsGenericType && type.GetGenericTypeDefinition() == generic;
        }
    }
}