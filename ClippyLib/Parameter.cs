/*
 * 
 * Copyright 2012-2015 Matthew Rikard
 * This file is part of Clippy.
 * 
 *  Clippy is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Clippy is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Clippy.  If not, see <http://www.gnu.org/licenses/>.
 *
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
			if(null == Validator)
				return true;

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

		public string GetEscapedValue()
		{
			return GetEscapedValue(Value);
		}

		private string GetEscapedValue(string value)
		{
			return ClipEscape(Regex.Escape(value));
		}

		public string GetEscapedValueOrDefault()
		{
			return GetEscapedValue(GetValueOrDefault());
		}

		public string GetValueOrDefault()
		{
			if(IsValued)
				return Value;
			if(Required)
				return null;

			return DefaultValue;
		}

		public static string ClipEscape(string input)
		{
			return input.Replace("\\q", "\"")
				.Replace("\\t", "\t")
					.Replace("\\n", "\n");
		}

    }
}
