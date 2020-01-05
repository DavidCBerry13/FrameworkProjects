using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.ResultType
{
    public class ObjectAlreadyExistsError<T> : Error
    {

        public T ExistingObject { get; private set; }

        public ObjectAlreadyExistsError(string message, T existingObject) : base(message)
        {
            ExistingObject = existingObject;
        }

    }
}
