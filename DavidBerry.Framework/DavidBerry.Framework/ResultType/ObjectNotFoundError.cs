using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.ResultType
{
    public class ObjectNotFoundError : Error
    {


        public ObjectNotFoundError(string message) : base(message)
        {

        }
    }
}
