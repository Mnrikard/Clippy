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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    internal class Count : AClipEditor
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
				int lines = 0;
				if(!String.IsNullOrEmpty(SourceData))
				{
					lines = SourceData.Split('\n').Length;
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
