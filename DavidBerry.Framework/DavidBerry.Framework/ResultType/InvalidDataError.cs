using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.ResultType
{
    public class InvalidDataError : Error
    {

        public InvalidDataError(string message) : base(message)
        {

        }

    }
}
