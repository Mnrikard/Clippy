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
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Grep : AClipEditor
    {
        #region boilerplate 

        public override string EditorName
        {
            get { return "Grep"; }
        }

        public override string ShortDescription
        {
            get { return "Gets a list of each pattern match."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Grep
Syntax: grep ""pattern"" [separator] [patternType]
Gets a Regular Expression match list based on the pattern passed in

Pattern - a regular expression pattern, ignores case
separator - The delimiter between matchs for the output
separator defaults to new line character.
patternType - One of regex, sql or text. Defaults to regex

Example:
    clippy grep ""\d+""
    will return each sequence of digits to a line.
";
            }
        }
        
        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Pattern",
                Sequence = 1,
                Validator = ValidateRegex,
                DefaultValue = null,
                Required = true,
                Expecting = "A regular expression pattern"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Separator",
                Sequence = 2,
                Validator = (a => true),
                DefaultValue = "\n",
                Required = false,
                Expecting = "A string separator"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Pattern Type",
                Sequence = 3,
                Validator = (a => true),
                DefaultValue = "regex",
                Required = false,
                Expecting = "One of [regex|sql|text]"
            });
        }

        private bool ValidateRegex(string pattern)
        {
            try
            {
                Regex r = new Regex(ClipEscape(pattern));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public override void SetParameters(string[] args)
        {
            SetParameter(2, ParameterList[1].DefaultValue);
            if (args.Length > 1)
                SetParameter(1, args[1]);
            if (args.Length > 2)
            {
                SetParameter(2, args[2]);
            }
        }

        public override void Edit()
        {
         	SuperRegex grepper = null;
            switch((ParameterList[2].Value ?? ParameterList[2].DefaultValue).ToLower())
            {
                case "sql":
                    string pattern = Regex.Replace(ParameterList[0].Value, @"(?<esc>[\.\}\{\+\*\\\?\|\)\(\$\^\#])", "\\${esc}");
                    pattern = Regex.Replace(pattern, "(?!\\\\)_", ".");
                    pattern = Regex.Replace(pattern, "(?!\\\\)%", @"(.|\n)*");
                    grepper = ClipEscape(pattern).ToSuperRegex();
                break;
                case "text":
                    grepper = Regex.Escape(ClipEscape(ParameterList[0].Value)).ToSuperRegex();
                break;
                default:
                    grepper = ClipEscape(ParameterList[0].Value).ToSuperRegex();
                break;
            }
            
        	MatchCollection matches = grepper.Matches(SourceData);
            List<string> matchlist = new List<string>();
            foreach (Match match in matches)
            {
                matchlist.Add(match.Value);
            }

            if (matchlist.Count == 0)
                RespondToExe("Pattern did not find a match in the string");
            else
                SourceData = String.Join(ClipEscape(ParameterList[1].Value), matchlist.ToArray());
        }       
        
    }
}
