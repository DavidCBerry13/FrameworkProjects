using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{

    /// <summary>
    /// Class to represent a severe error condition in an application, like data not being loading or some critical item missing
    /// </summary>
    public class ApplicationError : Error
    {

        public ApplicationError(string message) : base(message)
        {

        }

    }
}
