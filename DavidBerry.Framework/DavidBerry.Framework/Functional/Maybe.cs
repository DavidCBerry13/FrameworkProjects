#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional;


/// <summary>
/// Represents a Maybe Monad concept from functional programming, basically wrapping a return value in an object (Maybe) similar to nullable value types
/// </summary>
/// <remarks>
/// This is sometimes also known as the Optional pattern.  Rather than returning a value or null, the return value is wrapped in this object, the Maybe object.
/// This helps you handle null values more effectively and better protects against null reference exceptions
///
/// https://edgamat.com/2021/01/24/Avoiding-Null-Obsession-with-Maybe.html
/// nameof(value)
/// </remarks>
/// <typeparam name="T"></typeparam>
public struct Maybe<T> where T : class
{
    public T Value { get; private set; }

    internal Maybe(T value)
    {
        //ArgumentNullException.ThrowIfNull(value, nameof(value));

        Value = value;
    }

    //private Maybe()
    //{
    //}


    /// <summary>
    /// Checks if this object contains an actual value (is not null)
    /// </summary>
    public bool HasValue => Value != null;

    /// <summary>
    /// Check if this object does not contain a value (and is null)
    /// </summary>
    public bool HasNoValue => !HasValue;


    // ------------------------------------------------------------------------
    // Operators
    // ------------------------------------------------------------------------

    public static implicit operator Maybe<T>(T? value)
    {
        if (value is Maybe<T> m)
        {
            return m;
        }

        return Maybe.Create<T>(value);
    }

    public static bool operator ==(Maybe<T> maybe, T value)
    {
        if (value is Maybe<T> maybeValue)
            return maybe.Equals(maybeValue);

        if (maybe.HasNoValue)
            return false;

        return maybe.Value.Equals(value);
    }

    public static bool operator !=(Maybe<T> maybe, T value)
    {
        return !(maybe == value);
    }


    public static bool operator ==(Maybe<T> maybe, object other)
    {
        return maybe.Equals(other);
    }

    public static bool operator !=(Maybe<T> maybe, object other)
    {
        return !(maybe == other);
    }


    public static bool operator ==(Maybe<T> first, Maybe<T> second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(Maybe<T> first, Maybe<T> second)
    {
        return !(first == second);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is Maybe<T> otherMaybe)
            return Equals(otherMaybe);

        if (obj is T otherValue)
            return Equals(otherValue);

        return false;
    }



    public bool Equals(Maybe<T> other)
    {
        if (HasNoValue && other.HasNoValue)
            return true;

        if (HasNoValue || other.HasNoValue)
            return false;

        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }


    public override int GetHashCode()
    {
        if (HasNoValue)
            return 0;

        return Value.GetHashCode();
    }


    public override string ToString()
    {
        if (HasNoValue)
            return "No value";

        return Value.ToString() ?? Value.GetType().Name;
    }



    public static Maybe<T> None() => new Maybe<T>(null);

    /// <summary>
    /// Checks if this object contains a value and then runs the corresponding funtion based on whether of not it does contain a value.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="hasValue"></param>
    /// <param name="noValue"></param>
    /// <returns></returns>
    public U Match<U>(Func<T, U> hasValue, Func<U> hasNoValue)
    {
        return HasValue
          ? hasValue(Value)
          : hasNoValue();
    }

}


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