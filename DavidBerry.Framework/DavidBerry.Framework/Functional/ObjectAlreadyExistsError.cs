using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{

    /// <summary>
    /// Represents an error of where an object/record already exists in the system can therefor cannot be added.  For example
    /// adding a customer with the same email address as an existing customer indicates that customer already exists in the
    /// system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectAlreadyExistsError<T> : Error
    {

        public T ExistingObject { get; private set; }

        public ObjectAlreadyExistsError(string message, T existingObject) : base(message)
        {
            ExistingObject = existingObject;
        }

    }
}
