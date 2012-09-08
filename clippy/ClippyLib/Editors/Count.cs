using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    class Count : AClipEditor
    {
        public override string EditorName
        {
            get { return "Count"; }
        }
        

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Char or lines",
                Sequence = 1,
                Validator = a => (a.StartsWith("char", StringComparison.CurrentCultureIgnoreCase) || a.StartsWith("line", StringComparison.CurrentCultureIgnoreCase)),
                Expecting = "either \"char\" or \"line\"",
                Required=false,
                DefaultValue="char"
            });

        }

        public override string ShortDescription
        {
            get { return "Counts the number of characters or lines."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Syntax: count [countType]
Counts either the characters in the data, or the number of lines

countType - one of either ""char"" or ""line""
defaults to ""char""

Example:
    clippy count
    will display a message stating ""x characters""
    and
    clippy count line
    will display a message stating ""x lines""
";
            }
        }
        
        public override void Edit()
        {
            if (ParameterList.Count > 0 && ParameterList[0].Value != null && ParameterList[0].Value.StartsWith("line", StringComparison.CurrentCultureIgnoreCase))
            {
                int lines = String.IsNullOrEmpty(SourceData) ? 0 : 1;
                int currchar = 0;
                while ((currchar = SourceData.IndexOf('\n', currchar)) != -1)
                {
                    currchar++;//increment so we don't keep hitting the same new line char
                    lines++;
                }
                RespondToExe(String.Format("{0} lines", lines.ToString()));
            }
            else
            {
                RespondToExe(String.Format("{0} characters", SourceData.Length.ToString()));
            }
            //note: this particular edit doesn't change the sourcedata
            //that is on purpose
        }

           
    }
}
