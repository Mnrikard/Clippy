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
    public class ChunkText : AClipEditor
    {
		public ChunkText()
		{
			Name = "Chunk";
			Description = "Chunks text into n character length chunks";
			exampleInput = "123456789A";
			exampleCommand = "chunk 5";
			exampleOutput = "12345\n6789A";
			DefineParameters();
		}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Number of Characters",
                Sequence = 1,
                Validator = (a => a.IsInteger()),
                Required = true,
                Expecting = "An integer"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Separator",
                Sequence = 2,
                Validator = (a => true),
                DefaultValue = "\n",
                Required = false,
                Expecting = "A string separator"
            });
        }

        public override void SetParameters(string[] args)
        {
            for(int i=1;i<ParameterList.Count;i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                SetParameter(1, args[1]);
                if (args.Length > 2)
                {
                    SetParameter(2, args[2]);
                }
            }
        }

        public override void Edit()
        {
			SourceData = Chunk(SourceData, Int32.Parse(ParameterList[0].GetValueOrDefault()), ClipEscape(ParameterList[1].GetValueOrDefault()));
        }

        public static string Chunk(string text, int nchar, string sep)
        {
            string[] chunkparts = new string[(int)System.Math.Ceiling(text.Length / (double)nchar)];
            for (var i = 0; i < chunkparts.Length; i++)
            {
                if (nchar * (i + 1) > text.Length)
                {
                    chunkparts[i] = text.Substring(nchar * i);
                }
                else
                {
                    chunkparts[i] = text.Substring(nchar * i, nchar);
                }
            }
            text = String.Join(sep, chunkparts);
            return text;

        }
        
    }
}
