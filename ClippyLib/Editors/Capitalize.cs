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
        public Capitalize()
		{
			Name = "Cap";
			Description = "Sets case to upper, lower or first letter of each word.";
			exampleInput = "this is lower case";
			exampleCommand = "cap u";
			exampleOutput = "THIS IS LOWER CASE";
			DefineParameters();
		}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Case Type",
                Sequence = 1,
                Validator = (a => (a.Length > 0 && "ulm".Contains(a.ToLower().Substring(0,1)))),
                DefaultValue = "u",
                Required = false,
                Expecting = "Either U, L, or M"
            });
        }

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
            string ctype = ParameterList[0].GetValueOrDefault();
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
