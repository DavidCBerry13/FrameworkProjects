using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{


    public class Maybe
    {
        private Maybe()
        { }

        public static Maybe<T> Create<T>(T value) where T : class
        {
            return value != null ?
                new Maybe<T>(value)
                : Maybe<T>.None();
        }
    }

    public class Maybe<T> where T : class
    {
        public T Value { get; private set; }

        internal Maybe(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        private Maybe()
        {
        }

        public bool HasValue => Value != null;




        public static Maybe<T> None() => new Maybe<T>();

        public U Eval<U>(Func<T, U> hasValue, Func<U> noValue)
        {
            return HasValue
              ? hasValue(Value)
              : noValue();
        }
    }
}
