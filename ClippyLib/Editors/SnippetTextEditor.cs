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
using System.Xml;

namespace ClippyLib.Editors
{
    public class SnippetTextEditor : AClipEditor
    {
		
		private SnippetsList _snippets;

		public SnippetTextEditor()
		{
			Name = "Snippet";
			Description = "Returns a string of text defined in the snippet.xml file.";
			exampleInput = "doesn't matter";
			exampleCommand = "snippet thingIAlwaysWrite";
			exampleOutput = "this is the text I always write";
			DefineParameters();
			_snippets = new SnippetsList();
		}
		
		public override void DefineParameters()
		{
			_parameterList = new List<Parameter>();
			_parameterList.Add(new Parameter()
			                   {
				ParameterName = "Snippet to use",
				Sequence = 1,
				Validator = (a => true),
				DefaultValue = "",
				Required = true,
				Expecting = "A snippet Name defined in snippet.xml"
			});
		}



        public override void SetParameters(string[] args)
        {
            if (args.Length > 1)
            {
                SetParameter(1, args[1]);
            }
            else
            {
                string exeMessage = String.Format("Available Snippets:\r\n{0}", DisplaySnippetsToChoose());
                PersistentRespondToExe(exeMessage, false);
            }
        }

        private string DisplaySnippetsToChoose()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
			_snippets.DescribeSnippet(output);
			return output.ToString();
        }
		        
        public override void Edit()
        {
			Snippet chosen = _snippets.GetSnippet(ParameterList[0].GetValueOrDefault());

            if (chosen == null)
                RespondToExe(String.Format("Snippet requested ({0}) not found", ParameterList[0].Value));
            else
                SourceData = chosen.Content;
        }       
        
    }
}
