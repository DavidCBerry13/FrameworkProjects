using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.Functional
{
    public class ObjectNotFoundError : Error
    {


        public ObjectNotFoundError(string message) : base(message)
        {

        }
    }
}
