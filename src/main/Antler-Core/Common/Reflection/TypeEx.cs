using System;
using SmartElk.Antler.Core.Common.Dynamic;

namespace SmartElk.Antler.Core.Common.Reflection
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

        public static dynamic AsStaticMembersDynamicWrapper(this Type type)
        {                        
            return new StaticMembersDynamicWrapper(type);
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}