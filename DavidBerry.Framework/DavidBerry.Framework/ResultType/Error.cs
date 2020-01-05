using System;


namespace DavidBerry.Framework.ResultType
{
    public class Error
    {

        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

    }
}
