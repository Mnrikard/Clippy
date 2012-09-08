using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    class Rep : AClipEditor
    {
        public override string EditorName
        {
            get { return "Rep"; }
        }
        

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Regex Pattern",
                Sequence = 1,
                Validator = ValidateRegex,
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
        }

        public override string ShortDescription
        {
            get { return "Replaces clipboard text matched by pattern A with replacement string B."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Rep
Syntax: rep ""A"" ""B""

Performs a regular expression replacement on the source data

Pattern A is a regular expression with IgnoreCase option set.
Replacement B is the replacement string

Special characters:
\n    new line character
\q    quotation mark
\t    tab character

Example:
    clippy rep ""\d"" ""A""
    will replace every digit in your source data with the letter ""A""
";
            }
        }
        
        public override void Edit()
        {
            SuperRegex repper = ClipEscape(ParameterList[0].Value).ToSuperRegex();
            SourceData = repper.SuperReplace(SourceData, ClipEscape(ParameterList[1].Value));
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
           
    }
}
