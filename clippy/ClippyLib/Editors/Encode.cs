using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Encode : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "Encode"; }
        }

        public override string ShortDescription
        {
            get { return "Encodes/Decodes urls and html"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Encode
Syntax: clippy encode [url|html] [reverse]

Encodes/Decodes urls and html

url|html - Encodes either by url or html
reverse - Decodes instead of encodes

Example:
    clippy encode url
    will change the source data from
    ""specialchars=none""
    to
    ""specialchars%3Dnone""
";
            }
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Code Type (url|html)",
                Sequence = 1,
                Validator = (a => ("url".Equals(a.ToLower()) || "html".Equals(a.ToLower()))),
                DefaultValue = "url",
                Required = true,
                Expecting = "url or html"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Reverse",
                Sequence = 2,
                Validator = (a => (String.IsNullOrWhiteSpace(a) || "reverse".Equals(a.ToLower()))),
                DefaultValue = String.Empty,
                Required = false,
                Expecting = "reverse or empty string"
            });
        }

        #endregion

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            base.SetParameters(args);

            if (!ParameterList[1].IsValued)
                ParameterList[1].Value = ParameterList[1].DefaultValue;
            
        }

        delegate string EncodingFunction(string input);

        public override void Edit()
        {
            EncodingFunction f;
            bool decode = ParameterList[1].Value.Equals("reverse", StringComparison.CurrentCultureIgnoreCase);
            if (ParameterList[0].Value.Equals("url", StringComparison.CurrentCultureIgnoreCase))
            {
                if(decode)
                    f = System.Uri.UnescapeDataString;
                else
                    f = System.Uri.EscapeDataString;
            }
            else
            {
                if (decode)
                    f = System.Web.HttpUtility.HtmlDecode;
                else
                    f = System.Web.HttpUtility.HtmlEncode;                
            }
            try
            {
                SourceData = f(SourceData);
            }
            catch
            {
                RespondToExe("Unable to " + (decode ? "decode" : "encode") + " data");
            }
        }       
        
    }
}
