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
    public class XmlEdit : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "Xml"; }
        }

        public override string ShortDescription
        {
            get { return "Pretty prints xml inside the string."; }
        }

        public override string LongDescription
        {
            get
            {
                return @"Xml
Syntax: xml
Tabs over the xml inside each node.

No parameters

Example:
    clippy xml
    nests your nodes inside their parents.
";
            }
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            
        }

        #endregion

        

        public override void Edit()
        {
            //get rid of spaces in between tags
            String text = Regex.Replace(SourceData, @">\s+<", "><");
            //new line on tag joins
            text = Regex.Replace(text, "><", ">\n<");
            //new line on tag ends
            text = Regex.Replace(text, "([^>\\n])<", "$1\n<");
            //content after a self closing tag
            text = Regex.Replace(text, @"/>([^\n<])", "/>\n$1");
            //turns <tag>\r\n</tag> into <tag></tag>
            text = Regex.Replace(text, @"(<([^\s>]+).+)\n(</\2[\s>])", "$1$3");
            string[] lines = text.Split('\n');
            int tabcount = 0;
            bool inComment = false;
            string tabstr = System.Configuration.ConfigurationManager.AppSettings["tabString"];
            for (int line = 0; line < lines.Length; line++)
            {
                /*
                    * if prev tag is not closer and I'm not a closer and I'm not a comment
                    * ++
                    * if I'm a closer
                    * --
                    */
                if (lines[line].Trim().StartsWith("<!--") && !lines[line].Trim().EndsWith("-->"))
                {
                    inComment = true;
                }
                else if (inComment && lines[line].Trim().EndsWith("-->"))
                {
                    inComment = false;
                }

                if (lines[line].StartsWith("</") && !inComment)
                {
                    tabcount--;
                    if (tabcount < 0)
                        tabcount = 0;
                }

                lines[line] = tabstr.Times(tabcount) + lines[line];

                if (
                    !inComment
                    && !lines[line].Contains("</")
                    && !lines[line].Contains("/>")
                    && !lines[line].StartsWith("</")
                    && !lines[line].StartsWith("<?xml")
                    && !lines[line].EndsWith("-->")
                    && (lines[line].Contains("<") || lines[line].Contains(">"))
                    )
                {
                    tabcount++;
                }

            }
            SourceData = String.Join("\n", lines);
        }

        
        
    }
}
