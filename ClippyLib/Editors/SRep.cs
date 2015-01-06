/*
 * 
 * Copyright 2012 Matthew Rikard
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

namespace ClippyLib.Editors
{
    class StringRep : AClipEditor
    {
        public override string EditorName
        {
            get { return "SRep"; }
        }
        

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Replace What",
                Sequence = 1,
                Validator = a => true,
                Expecting = "A string to replace",
                Required=true
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "string replacement",
                Sequence = 2,
                Validator = (a => (true)),
                Expecting = "a replacement string",
                Required = true
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Ignore Case",
                Sequence = 3,
                Validator = (a => true),
                Expecting = "i or true indicate ignoring case.  Anything else is case sensitive",
                Required = false,
                DefaultValue = String.Empty
            }); 
        }

        public override string ShortDescription
        {
            get { return "Replaces string A with replacement string B."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"SRep
Syntax: srep ""A"" ""B""

Performs an exact replacement on the source data

Pattern A is a string to replace.
Replacement B is the replacement string

Special characters:
\n    new line character
\q    quotation mark
\t    tab character

Example:
    clippy srep ""Q"" ""A""
    will replace every literal ""Q"" in your source data with the letter ""A""
";
            }
        }
        
        public override void Edit()
        {
        	if(ParameterList[2].Value.Equals("i",StringComparison.CurrentCultureIgnoreCase) || 
        	   ParameterList[2].Value.Equals("true",StringComparison.CurrentCultureIgnoreCase))
        	{
        		SourceData = Regex.Replace(SourceData, Regex.Escape(ParameterList[0].Value), ParameterList[1].Value, RegexOptions.IgnoreCase);
        	}
        	SourceData = SourceData.Replace(ParameterList[0].Value, ParameterList[1].Value);
        }          
    }
}
