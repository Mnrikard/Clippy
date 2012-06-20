using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Reverse : AClipEditor
    {
        public override string EditorName
        {
            get { return "Reverse"; }
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

        public override void SetParameters(string[] args)
        {
            for (int i = 0; i < ParameterList.Count; i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                ParameterList[0].Value = args[1];                
            }
        }

        public override void Edit()
        {
            string[] sortable = Regex.Split(SourceData, Regex.Escape(ClipEscape(ParameterList[0].Value)));
            Array.Reverse(sortable);
            SourceData = String.Join(ParameterList[0].Value, sortable);
        }


        public override string ShortDescription
        {
            get { return "Reverses the order of a string, based on a string delimiter"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Reverse
Syntax: reverse [delimiter] 
Reverses a string based on a string delimiter.

delimiter - any string separator
Defaults to new line character

Example:
    clippy reverse "",""
    will change ""a,b,c"" to ""c,b,a""
";
            }
        }
    }
}
