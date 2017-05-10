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
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class Reverse : AClipEditor
    {
		public Reverse()
		{
			Name = "Reverse";
			Description = "Reverses a delimited list";
			exampleInput = "1,2,3,4";
			exampleCommand = "reverse \",\"";
			exampleOutput = "4,3,2,1";
			DefineParameters();
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
            string[] sortable = Regex.Split(SourceData, ParameterList[0].GetEscapedValueOrDefault());
            Array.Reverse(sortable);
            SourceData = String.Join(ParameterList[0].GetEscapedValueOrDefault(), sortable);
        }
    }
}
