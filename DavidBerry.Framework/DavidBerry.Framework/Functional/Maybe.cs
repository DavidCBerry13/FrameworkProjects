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


    private T _value;

    public T Value => _value;


    public Maybe(T value)
    {
        _value = value;
    }



    public static Maybe<T> Create(T value)
    {
        return new Maybe<T>(value);
    }


    public static Maybe<T> None() => new Maybe<T>();

    /// <summary>
    /// Checks if this Maybe object contains a value
    /// </summary>
    public bool HasValue => _value != null;

    /// <summary>
    /// Chacks if this Maybe object does not contain a value
    /// </summary>
    public bool HasNoValue => !HasValue;


    /// <summary>
    /// Casts a value of type T to a Maybe of type T
    /// </summary>
    /// <remarks>
    /// This is useful so a method can have a return type of Maybe<T> and you can just return a value of type T
    /// from the code within the methd
    /// </remarks>
    /// <param name="value"></param>
    public static implicit operator Maybe<T>(T value)
    {
        return new Maybe<T>(value);
    }

    public static bool operator ==(Maybe<T> maybe, T value)
    {
        if (maybe.HasNoValue)
            return false;

        return maybe.Value.Equals(value);
    }

    public static bool operator !=(Maybe<T> maybe, T value)
    {
        return !(maybe == value);
    }

    /// <summary>
    /// Checks if this Maybe object contains a value and calls the appropriate function
    /// </summary>
    /// <remarks>
    /// This is useful to extract a value from the Maybe object without having to check if it has a value first
    /// </remarks>
    /// <typeparam name="R"></typeparam>
    /// <param name="hasValue"></param>
    /// <param name="hasNoValue"></param>
    /// <returns></returns>
    public R Match<R>(Func<T, R> hasValue, Func<R> hasNoValue)
    {
        return HasValue
            ? hasValue(_value)
            : hasNoValue();
    }


    public override bool Equals(object? obj)
    {
        if (obj is Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;
            if (HasNoValue || other.HasNoValue)
                return false;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }
        return false;
    }

    public override int GetHashCode()
    {
        if (HasNoValue)
            return 0;
        return Value.GetHashCode();
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