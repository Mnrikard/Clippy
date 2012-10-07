using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class ClipSort : AClipEditor
    {
        public override string EditorName
        {
            get { return "Sort"; }
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Sort Order",
                Sequence = 1,
                Validator = (a => (a.ToLower() == "asc" || a.ToLower() == "desc")),
                DefaultValue = "asc",
                Required = false,
                Expecting = "asc or desc"
            }); 
            
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Delimiter",
                Sequence = 2,
                Validator = (a => true),
                DefaultValue = "\n",
                Required = false,
                Expecting = "a string delimiter"
            });

            _parameterList.Add(new Parameter()
            {
                ParameterName = "Ignore Case",
                Sequence = 3,
                Validator = (a => (a.ToLower() == "true" || a.ToLower() == "false")),
                DefaultValue = "true",
                Required = false,
                Expecting = "true or false"
            });
        }

        private bool _ignoreCase = true;

        public override void SetParameters(string[] args)
        {
            for (int i = 0; i < ParameterList.Count; i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                if (args[1].Trim().StartsWith("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    ParameterList[0].Value = "desc";
                } 
                if (args.Length > 2)
                {
                    ParameterList[1].Value = args[2];
                
                    if (args.Length > 3)
                    {
                        if (args[3].Equals("false", StringComparison.CurrentCultureIgnoreCase))
                            _ignoreCase = false;
                    }
                }
            }
        }

        private int SortUnknown(string a, string b)
        {
            Decimal da, db;
            if (Decimal.TryParse(a, out da) && Decimal.TryParse(b, out db))
                return Decimal.Compare(da, db);
            DateTime dta, dtb;
            if (DateTime.TryParse(a, out dta) && DateTime.TryParse(b, out dtb))
                return DateTime.Compare(dta, dtb);
            return String.Compare(a, b, _ignoreCase);
        }

        public override void Edit()
        {
            string[] sortable = Regex.Split(SourceData, Regex.Escape(ClipEscape(ParameterList[1].Value)), RegexOptions.IgnoreCase);
            Array.Sort(sortable, SortUnknown);
            if (ParameterList[0].Value.Trim().Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                Array.Reverse(sortable);
            SourceData = String.Join(ClipEscape(ParameterList[1].Value), sortable);
        }


        public override string ShortDescription
        {
            get { return "Sorts a string"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Sort
Syntax: sort [sortorder] [delimiter] [ignore case]
Sorts a string based on a string delimiter and a given sort order

Sort order is expecing either asc or desc
sort order is asc by default

Delimiter defaults to new line character but can be any string separator

Ignore case is by default true
this argument accepts either true or false

Example:
    clippy sort ""\t"" asc true
    will sort the tab delimited string in ascending order, ignoring case.
";
            }
        }
    }
}
