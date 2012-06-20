using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClippyLib
{
    public class Parameter
    {
        public int Sequence { get; set; }
        public string ParameterName { get; set; }
        public Func<string, bool> Validator {get;set;}
        public string Expecting { get; set; }
        public string DefaultValue { get; set; }
        public bool Required { get; set; }
        public bool Validate(string input)
        {
            return Validator(input);
        }
        private string _value = null;
        public string Value
        {
            get { return _value; }
            set 
            {
                if (!Validate(value))
                {
                    throw new InvalidParameterException("Parameter {0} is not valid, Expecting: {1}", ParameterName, Expecting);
                }
                _value = value;
            }
        }
        public bool IsValued 
        {
            get
            {
                if (Value == null)
                {
                    return false;
                }
                return true;
            }
        }

    }
}
