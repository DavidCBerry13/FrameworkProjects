using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{

    /// <summary>
    /// Represents an error where invalid data was passed to an operation.  The error message should include a description
    /// of what was wrong with the data so the calling client can attempt to fix it
    /// </summary>
    public class InvalidDataError : Error
    {

        public InvalidDataError(string message) : base(message)
        {

        }

    }
}
