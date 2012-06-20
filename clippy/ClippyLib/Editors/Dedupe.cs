using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ClippyLib.Editors
{
    public class Dedupe : AClipEditor
    {
        public override string EditorName { get { return "Dedup"; } }

        #region boilerplate

        public override string ShortDescription
        {
            get { return "Removes duplicate values from a list"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Dedup
Syntax: clippy dedup [delimiter]
Separates the string into a list based on the delimiter and removes duplicate entries.

delimiter can be any string
defaults to new line character

Example:
    clippy dedup
    will remove duplicates from the source data after splitting by a new line character
    clippy dedup "",""
    will change the source data from ""a,b,a,c"" to ""a,b,c""
";
            }
        }

        public Dedupe()
        {
            
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Delimiter",
                Sequence = 1,
                Validator = (a => true),
                DefaultValue = "\n",
                Required = false,
                Expecting = "a string delimiter"
            });
        }

        #endregion

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            for(int i=0;i<ParameterList.Count;i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                ParameterList[0].Value = args[1];
            }
        }

        public override void Edit()
        {
            string[] distinctItems = (from itm in Regex.Split(SourceData, Regex.Escape(ClipEscape(ParameterList[0].Value)), RegexOptions.IgnoreCase)
                                      select itm).Distinct().ToArray();
            SourceData = String.Join(ParameterList[0].Value, distinctItems);
        }       
        
    }
}
