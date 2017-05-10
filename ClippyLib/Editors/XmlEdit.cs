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
using ClippyLib.Settings;

namespace ClippyLib.Editors
{
    public class XmlEdit : AClipEditor
    {
        public XmlEdit()
		{
			Name = "Xml";
			Description = "Pretty prints xml inside the string.";
			exampleInput = "<root><child/></root>";
			exampleCommand = "xml";
			exampleOutput = "<root>\n\t<child/>\n</root>";
			DefineParameters();
		}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
        }

        public override void Edit()
        {
            //get rid of spaces in between tags
            var text = InitializeSpaces();

            string[] lines = text.Split('\n');
            int tabcount = 0;
            bool inCommentOrCdata = false;

			SettingsObtainer obt = SettingsObtainer.CreateInstance();
			string tabstr = obt.TabString;
            
			for (int line = 0; line < lines.Length; line++)
            {
                /*
                    * if prev tag is not closer and I'm not a closer and I'm not a comment
                    * ++
                    * if I'm a closer
                    * --
                    */
				if(inCommentOrCdata)
				{
					if (EndOfCommentOrCdata(lines[line]))
					{
						inCommentOrCdata = false;
					}
				}
				else
				{
					inCommentOrCdata = StartsCommentOrCdata(lines[line]);
				}

                if (lines[line].StartsWith("</") && !inCommentOrCdata)
                {
                    tabcount--;
                    if (tabcount < 0)
                        tabcount = 0;
                }

                lines[line] = tabstr.Times(tabcount) + lines[line];

				if (ShouldIncrementTabCount(lines, inCommentOrCdata, line)) 
				{
					tabcount++;
				}

            }
            SourceData = String.Join("\n", lines);
        }

		private bool EndOfCommentOrCdata(string line)
		{
			return line.Trim().EndsWith("-->") || line.Trim().EndsWith("]]>");
		}
		private bool StartsCommentOrCdata(string line)
		{
			line = line.Trim();
			return (line.StartsWith("<!--") && !line.EndsWith("-->")) ||
				(line.StartsWith("<![CDATA[") && !line.EndsWith("]]>"));
		}

		private bool ShouldIncrementTabCount(string[] lines, bool inComment, int line)
		{
			return !inComment && 
					!lines [line].Contains ("</") && 
					!lines [line].Contains ("/>") && 
					!lines [line].StartsWith ("</") && 
					!lines [line].StartsWith ("<?xml") && 
					!lines [line].EndsWith ("-->") && 
					(lines [line].Contains ("<") || lines [line].Contains (">"));
		}
        
        
		private string InitializeSpaces()
		{
			Regex spaceBetweenTags = new Regex(@">\s+<");
			Regex endTags = new Regex("([^>\\n])<");
			Regex textAfterClosedTag = new Regex(@"/>([^\n<])");
			Regex emptyTag = new Regex(@"(<([^\s>]+).+)\n(</\2[\s>])");

			String text = spaceBetweenTags.Replace(SourceData, "><")
				.Replace("><",">\n<");
			text = endTags.Replace(text,"$1\n<");
			text = textAfterClosedTag.Replace(text, "/>\n$1");
			text = emptyTag.Replace(text, "$1$3");
			return text;
		}
    }
}
