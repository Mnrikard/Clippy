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
using System.Text;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Encode : AClipEditor
    {
		public Encode()
		{
			Name = "Encode";
			Description = "Encodes/Decodes text with different formats";
			exampleInput = "<xml/>";
			exampleCommand = "encode xml";
			exampleOutput = "&lt;xml/&gt;";
			DefineParameters();
		}
        
        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Code Type",
                Sequence = 1,
                Validator = (a => (Regex.IsMatch(a,"(url|html|xml|base64)", RegexOptions.IgnoreCase))),
                DefaultValue = "url",
                Required = true,
                Expecting = "url, xml, html or base64"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Reverse",
                Sequence = 2,
                Validator = (a => (String.IsNullOrEmpty(a) || a.Trim().Length == 0 || "reverse".Equals(a, StringComparison.CurrentCultureIgnoreCase))),
                DefaultValue = String.Empty,
                Required = false,
                Expecting = "either \"reverse\", \"decode\" or empty string"
            });
        }

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
            bool decode = ParameterList[1].GetValueOrDefault().Equals("reverse", StringComparison.CurrentCultureIgnoreCase) || 
				ParameterList[1].GetValueOrDefault().Equals("decode", StringComparison.CurrentCultureIgnoreCase);

			string typeDirection = String.Concat(ParameterList[0].GetValueOrDefault().ToLower()," ",ParameterList[1].GetValueOrDefault().ToLower());

			switch (typeDirection.Trim())
			{
				case "url reverse":
					SourceData = ModifiedUrlDecode(SourceData);
					break;
				case "url":
					SourceData = SafeUrlEncode(SourceData);
					break;
				case "base64":
					SourceData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(SourceData));
					break;
				case "base64 reverse":
					SourceData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(SourceData));
					break;
				case "html":
				case "xml":
					SourceData = System.Web.HttpUtility.HtmlEncode(SourceData);
					break;
				case "html reverse":
				case "xml reverse":
					SourceData = System.Web.HttpUtility.HtmlDecode(SourceData);
					break;
				default:
					RespondToExe("Unable to " + (decode ? "decode" : "encode") + " data");
					break;
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

		private string ModifiedUrlDecode(string data)
		{
			for(int i=0;i<256;i++)
			{
				string replacement = String.Concat("%",i.ToString("x").PadLeft(2,'0'));
				if(data.Contains(replacement.ToUpper()))
				{
					data = data.Replace(replacement.ToUpper(), ((char)i).ToString());
				}
				if(data.Contains(replacement))
				{
					data = data.Replace(replacement, ((char)i).ToString());
				}
			}

			return data;
		}
        
    }
}
