using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.ResultType
{
    public class ConcurrencyError<T> : Error
    {

        public T ConflictingObject { get; private set; }


        public ConcurrencyError(string message, T conflictingObject) : base(message)
        {
            ConflictingObject = conflictingObject;
        }

    }
}
