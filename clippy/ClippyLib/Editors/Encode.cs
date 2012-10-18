using System;
using System.Collections.Generic;
using System.Text;
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

url|html|base64 - Encodes either by url, html or base64
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
                ParameterName = "Code Type (url|html|base64)",
                Sequence = 1,
                Validator = (a => ("url".Equals(a.ToLower()) || "html".Equals(a.ToLower()) || "base64".Equals(a.ToLower()))),
                DefaultValue = "url",
                Required = true,
                Expecting = "url, html or base64"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Reverse",
                Sequence = 2,
                Validator = (a => (String.IsNullOrEmpty(a) || a.Trim().Length == 0 || "reverse".Equals(a, StringComparison.CurrentCultureIgnoreCase))),
                DefaultValue = String.Empty,
                Required = false,
                Expecting = "reverse or empty string"
            });
        }

        #endregion

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            for (int i = 0; i < ParameterList.Count; i++)
            {
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            }
            
            if (args.Length > 1)
            {
                //Console.WriteLine(args[1]);
                ParameterList[0].Value = args[1];
                if (args.Length > 2)
                    ParameterList[1].Value = args[2];
            }
            
        }

        public override void Edit()
        {
            bool decode = ParameterList[1].Value.Equals("reverse", StringComparison.CurrentCultureIgnoreCase);
            try
            {
                if (ParameterList[0].Value.Equals("url", StringComparison.CurrentCultureIgnoreCase))
                {
                    if(decode)
                        SourceData = SafeUrlDecode(SourceData);
                    else
                        SourceData = SafeUrlEncode(SourceData);
                }
                else if (ParameterList[0].Value.Equals("base64", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (decode)
                        SourceData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(SourceData));
                    else
                        SourceData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(SourceData));
                }
                else
                {
                    if (decode)
                        SourceData = System.Web.HttpUtility.HtmlDecode(SourceData);
                    else
                        SourceData = System.Web.HttpUtility.HtmlEncode(SourceData);
                }   
            }
            catch
            {
                RespondToExe("Unable to " + (decode ? "decode" : "encode") + " data");
            }
        }

        private string SafeUrlEncode(string data)
        {
            // urls have a size limit, this method chunks out the data and encode it a piece at a time
            int i = 0;
            int chunkSize = 255;
            StringBuilder output = new StringBuilder();
            while (i < data.Length)
            {
                if (i + chunkSize > data.Length)
                {
                    output.Append(System.Uri.EscapeDataString(data.Substring(i)));
                }
                else
                {
                    output.Append(System.Uri.EscapeDataString(data.Substring(i, chunkSize)));
                }
                i += chunkSize;
            }
            return output.ToString();
        }

        private string SafeUrlDecode(string data)
        {
            int i = 0;
            int chunkSize = 255;
            StringBuilder output = new StringBuilder();
            while (i < data.Length)
            {
                if (i + chunkSize > data.Length)
                {
                    output.Append(System.Uri.UnescapeDataString(data.Substring(i)));
                }
                else
                {
                    output.Append(System.Uri.UnescapeDataString(data.Substring(i, chunkSize)));
                }
                i += chunkSize;
            }
            return output.ToString();
        }
        
    }
}
