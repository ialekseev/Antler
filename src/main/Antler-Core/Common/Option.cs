using System;
using SmartElk.Antler.Core.Common.Reflection;

namespace SmartElk.Antler.Core.Common
{
    public abstract class Option<T>
    {
        public static Option<T> None
        {
            get { return new None<T>(); }
        }

        public static Option<T> Some(T value)
        {
            return new Some<T>(value);
        }

        public abstract T Value { get; }
        public abstract T ValueOrDefault { get; }
        public abstract bool IsSome { get; }
        public abstract bool IsNone { get; }

        public static implicit operator Option<T>(T value)
        {
            return value.AsOption();
        }

        public static implicit operator bool(Option<T> option)
        {
            return option.IsSome;
        }

        public static explicit operator T(Option<T> option)
        {
            return option.Value;
        }
    }

    public sealed class Some<T> : Option<T>
    {
        private readonly T _value;

        public Some(T value)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (/*!typeof(T).IsValueType &&*/ value == null)
            // ReSharper restore CompareNonConstrainedGenericWithNull
            {
                throw new ArgumentNullException("value", "Some value was null, use None instead");
            }

            if (typeof(T).IsSubclassOfRawGeneric(typeof(Option<>)))
            {
                throw new InvalidOperationException("Nested Option is not supported!");
            }

            _value = value;
        }

        public override T Value
        {
            get { return _value; }
        }

        public override T ValueOrDefault
        {
            get { return _value; }
        }

        public override bool IsSome
        {
            get { return true; }
        }

        public override bool IsNone
        {
            get { return false; }
        }
    }

    public sealed class None<T> : Option<T>
    {
        public override T Value
        {
            get { throw new NotSupportedException("There is no value"); }
        }

        public override T ValueOrDefault
        {
            get { return default(T); }
        }

        public override bool IsSome
        {
            get { return false; }
        }

        public override bool IsNone
        {
            get { return true; }
        }
    }

    public static class OptionEx
    {
        public static Option<T> AsOption<T>(this T value)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (/*!typeof(T).IsValueType &&*/ value == null) return new None<T>();
            // ReSharper restore CompareNonConstrainedGenericWithNull

            return new Some<T>(value);
        }
    }        
}
