using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{

    /// <summary>
    /// Error to represent when the requested object could not be found
    /// </summary>
    public class ObjectNotFoundError : Error
    {



        public ObjectNotFoundError(string message) : base(message)
        {

        }
    }
}
