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

namespace ClippyLib.Editors
{
    public class Rep : AClipEditor
    {
		public Rep()
		{
			Name = "Rep";
			Description = @"Replaces clipboard text matched by pattern A with replacement string B.
Pattern A is a regular expression with IgnoreCase option set.
Replacement B is the replacement string
The final optional parameter defines the pattern type
    regex is a regular expression
    sql is a TSQL LIKE pattern
    text is plain text, no pattern

Special characters:
\n    new line character
\q    quotation mark
\t    tab character

Extensions to regex pattern and replacement string
Regex Pattern:
    use the pattern ""/^line starts and.+ends$/m"" 
        to search for the pattern with the MultiLine option, case sensitive
    use the pattern ""/^line starts and.+ends$/mi"" 
        to search for the pattern with the MultiLine option, case insensitive
    use the pattern ""some pattern""
        by default to search for the case-insensitive pattern
Replacement String:
    replace instances of ""my dog lasso"" with ""my dog Lasso"" with
        rep ""my dog (\w)(\w+)"" ""my dog \u$1$2""

    \u replaces the group with upper case version
    \l replaces the group with lower case
";
			exampleInput = "sw33t";
			exampleCommand = "rep \\d e";
			exampleOutput = "sweet";
			DefineParameters();
		}

		public override string ShortDescription { get {return "Replaces clipboard text matched by pattern A with replacement string B.";}}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Regex Pattern",
                Sequence = 1,
                Validator = a => true,//todo: figure out how to validate this in context of text/sql/regex
                Expecting = "a regular expression pattern",
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
                ParameterName = "pattern type",
                Sequence = 3,
                Validator = (a => (true)),
                Expecting = "either regex, sql, or text",
                Required = false,
                DefaultValue = "regex"
            }); 
        }
		        
        public override void Edit()
        {
            SuperRegex repper = null;
            switch((ParameterList[2].Value ?? ParameterList[2].DefaultValue).ToLower())
            {
                case "sql":
                    string pattern = Regex.Replace(ParameterList[0].Value, @"(?<esc>[\.\}\{\+\*\\\?\|\)\(\$\^\#])", "\\${esc}");
                    pattern = Regex.Replace(pattern, "(?!\\\\)_", ".");
                    pattern = Regex.Replace(pattern, "(?!\\\\)%", @"(.|\n)*");
                    repper = ClipEscape(pattern).ToSuperRegex();
                break;
                case "text":
                    repper = Regex.Escape(ClipEscape(ParameterList[0].Value)).ToSuperRegex();
                break;
                default:
                    repper = ClipEscape(ParameterList[0].Value).ToSuperRegex();
                break;
            }
            
            SourceData = repper.SuperReplace(SourceData, ClipEscape(ParameterList[1].Value));
        }

        private bool ValidateRegex(string pattern)
        {
            try
            {
                //todo: figure out this validation with the new patterns
                ClipEscape(pattern).ToSuperRegex();
                return true;
            }
            catch
            {
                return false;
            }
        }
           
    }
}
