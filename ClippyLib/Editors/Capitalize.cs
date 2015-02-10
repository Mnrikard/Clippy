/*
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
 * 
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Capitalize : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "Cap"; }
        }

        public override string ShortDescription
        {
            get { return "Sets case to upper, lower or first letter of each word."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Cap
Syntax: clippy cap [U|L|M]

Sets the capitalization of the source data

U|L|M - U: returns the string in all upper case characters
        L: returns the string in all lower case characters
        M: returns the string in mixed case characters
           for instance: ""the quick brown dog"" becomes
           ""The Quick Brown Dog""

Defaults to upper case

Example:
    clippy cap L
    will set the source data to lower case text.
";
            }
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Case Type (U|L|M)",
                Sequence = 1,
                Validator = (a => (a.Length > 0 && "ulm".Contains(a.ToLower().Substring(0,1)))),
                DefaultValue = "u",
                Required = false,
                Expecting = "Either U, L, or M"
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
                SetParameter(1,args[1].Substring(0,1).ToLower());
            }
        }

        public override void Edit()
        {
            string ctype = ParameterList[0].Value;
            if (ctype == "u")
                SourceData = SourceData.ToUpper();
            else if (ctype == "l")
                SourceData = SourceData.ToLower();
            else
            {
                string text = SourceData;
                MatchCollection matches = Regex.Matches(text, "\\w+");
                foreach (Match m in matches)
                {
                    text = text.Replace(m.Value, m.Value.Substring(0, 1).ToUpper() + m.Value.Substring(1).ToLower());
                }
                SourceData = text;
            }

        }       
        
    }
}
