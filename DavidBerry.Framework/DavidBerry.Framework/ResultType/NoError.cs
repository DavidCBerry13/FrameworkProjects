using System;
using System.Collections.Generic;
using System.Text;

namespace DavidBerry.Framework.ResultType
{
    public sealed class NoError : Error
    {

        private NoError() : base(string.Empty)
        {

        }



        public static readonly NoError NO_ERROR = new NoError();


    }
}
