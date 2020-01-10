using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{

    /// <summary>
    /// Represents a concurrency error, typically when an operation attempts to update an object/record that has been modified
    /// since that operation read the record
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrencyError<T> : Error
    {

        public T ConflictingObject { get; private set; }


        public ConcurrencyError(string message, T conflictingObject) : base(message)
        {
            ConflictingObject = conflictingObject;
        }

    }
}
