/*
 * 
 * Copyright 2012 Matthew Rikard
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
                int tmpChSz = chunkSize;
                if (i + tmpChSz > data.Length)
                {
                    output.Append(System.Uri.UnescapeDataString(data.Substring(i)));
                }
                else
                {
                    if (data[i + tmpChSz - 2] == '%')
                    {
                        tmpChSz -= 2;
                    }
                    if (data[i + tmpChSz - 1] == '%')
                    {
                        tmpChSz -= 1;
                    }
                    output.Append(System.Uri.UnescapeDataString(data.Substring(i, tmpChSz)));
                }
                i += tmpChSz;
            }
            return output.ToString();
        }
        
    }
}
