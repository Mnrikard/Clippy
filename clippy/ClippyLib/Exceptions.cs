using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClippyLib
{
    public class InvalidParameterException : System.Exception
    {
        public string ParameterMessage { get; private set; }
        public InvalidParameterException(string formatMessage, params object[] formatItems)
        {
            ParameterMessage = String.Format(formatMessage, formatItems);
        }
    }

    public class UndefinedFunctionException : System.Exception
    {
        public string FunctionMessage { get; private set; }
        public UndefinedFunctionException(string formatMessage, params object[] formatItems)
        {
            FunctionMessage = String.Format(formatMessage, formatItems);
        }
    }
}
