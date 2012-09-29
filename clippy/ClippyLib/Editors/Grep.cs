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
Syntax: grep ""pattern"" [separator]
Gets a Regular Expression match list based on the pattern passed in

Pattern - a regular expression pattern, ignores case
separator - The delimiter between matchs for the output
separator defaults to new line character.

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
            Regex matcher = ClipEscape(ParameterList[0].Value).ToRegex();
            MatchCollection matches = matcher.Matches(SourceData);
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
