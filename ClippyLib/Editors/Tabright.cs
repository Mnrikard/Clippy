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
using System.IO;
using ClippyLib.Settings;

namespace ClippyLib.Editors
{
    public class TabRight : AClipEditor
    {
        public TabRight()
		{
			Name = "TabRight";
			Description = "Tabs c style and vb style text into correctly nested values.";
			exampleInput = "public int main(){ printf(\"hello\");}";
			exampleCommand = "tabright c";
			exampleOutput = "public int main()\n{\n\tprintf(\"hello\");\n}";
			DefineParameters();
		}

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Language Type",
                Sequence = 1,
                Validator = (a => (a.Equals("vb",StringComparison.CurrentCultureIgnoreCase) || a.Equals("c",StringComparison.CurrentCultureIgnoreCase))),
                DefaultValue = "c",
                Required = false,
                Expecting = "vb or c"
            });
        }

        public override void SetParameters(string[] args)
        {
            for(int i=0;i<ParameterList.Count;i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                SetParameter(1, args[1]);
            }
        }

        public override void Edit()
        {
            if (ParameterList[0].Value.Equals("vb", StringComparison.CurrentCultureIgnoreCase))
                SourceData = TabRightVB(SourceData);
            else
                SourceData = TabRightC(SourceData);

            
        }

        /// <summary>
        /// Tabs a file based on c style syntax
        /// </summary>
        private static string TabRightC(string text)
        {
            return TabRightC(text, 0);
        }

        /// <summary>
        /// Tabs a file based on c style syntax
        /// </summary>
        /// <param name="text">Action string</param>
        /// <param name="tabcount">Number of tabs to assume for base.</param>
        private static string TabRightCold(string text, int tabcount)
        {
			SettingsObtainer obt = SettingsObtainer.CreateInstance();
			string tabstr = obt.TabString;
            text = Regex.Replace(text, @"\n\s*", String.Empty).Replace("{", "{\n").Replace("}", "\n}\n").Replace(";", ";\n");
            string[] rows = text.Split('\n');

            for (int i = 0; i < rows.Length; i++)
            {
                if (Regex.IsMatch(rows[i], @"^\s*//")) continue;//#if its a commented line, skip it
                rows[i] = Regex.Replace(Regex.Replace(rows[i], @"\s+$", ""), @"^\s+", "");
                if (rows[i].Length > 0 && rows[i].StartsWith("}"))
                {
                    tabcount--;
                }
                for (int t = 0; t < tabcount; t++)
                {
                    rows[i] = tabstr + rows[i];
                }
                if (rows[i].Length > 0 && rows[i].EndsWith("{"))
                {
                    tabcount++;
                }
            }
            return String.Join("\n", rows);
        }
        //were going to have to create a lexer for this guy
        private static string TabRightC(string code, int tabcount)
        {
            string[] keywords = { "abstract","as","base","break","case","catch","checked","continue","default","delegate","do","else","event","explicit","extern","false","finally","fixed","for","foreach","goto","if","implicit","in","interface","internal","is","lock","namespace","new","null","object","operator","out","override","params","private","protected","public","readonly","ref","return","sealed","sizeof","stackalloc","switch","this","throw","true","try","typeof","unchecked","unsafe","using","virtual","while" };
            string[] types = { "bool", "byte", "char", "class", "const", "decimal", "double", "enum", "float", "int", "long", "sbyte", "short", "static", "string", "struct", "uint", "ulong", "ushort", "void" };

            bool inString = false;
            bool inComment = false;
            bool blockString = false;
            bool blockComment = false;
            string prevChar = String.Empty;
            string setPrvChr = String.Empty;

			SettingsObtainer obt = SettingsObtainer.CreateInstance();
			string tabstr = obt.TabString;

            StringReader sr = new StringReader(code);
            StringBuilder codeOut = new StringBuilder();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                int subtracttab = 0;
                StringBuilder buildLine = new StringBuilder();
                if(!blockString && !blockComment)
                    line.Trim();
                prevChar = "\r\n";
                for (int i = 0; i < line.Length; i++ )
                {
                    char c = line[i];
                    setPrvChr = c.ToString();
                    
                    #region strings

                    if (c == '"' || blockString)
                    {
                        buildLine.Append(c);
                        inString = true;
                        if (prevChar == "@")
                        {
                            blockString = true;
                            int concurrentQuot = 0;
                            while (++i < line.Length)
                            {
                                if (line[i] == '"')
                                {
                                    concurrentQuot++;
                                    buildLine.Append(line[i]);
                                }
                                else if (concurrentQuot > 0)
                                {
                                    if (concurrentQuot % 2 == 1)
                                    {
                                        i--;
                                        inString = false;
                                        blockString = false;
                                        break;
                                    }
                                    else
                                        buildLine.Append(line[i]);
                                    concurrentQuot = 0;
                                }
                                else
                                    buildLine.Append(line[i]);
                            }
                        }
                        else //simple string
                        {
                            int concurrentSlash = 0;
                            
                            while (++i < line.Length)
                            {
                                buildLine.Append(line[i]);

                                if (line[i] == '\\')
                                    concurrentSlash++;
                                else
                                {
                                    if(line[i] == '"')
                                    {
                                        if (concurrentSlash % 2 == 0)
                                        {
                                            inString = false;
                                            break;
                                        }
                                    }
                                    concurrentSlash = 0;
                                }
                            }
                        }
                    }
                    #endregion

                    #region block comments

                    else if ((prevChar=="/" && c == '*') || blockComment)
                    {
                        buildLine.Append(c);
                        if(!blockComment)
                        {
                            i++;
                            prevChar = "*";
                        }
                        inComment = true;
                        blockComment = true;
                        while (++i < line.Length)
                        {
                            buildLine.Append(line[i]);

                            if (line[i] == '/' && prevChar == "*")
                            {
                                inComment = false;
                                blockComment = false;
                                break;
                            }
                            prevChar = line[i].ToString();
                        }
                    }
                    #endregion

                    #region comments

                    else if (prevChar=="/" && c == '/')
                    {
                        buildLine.Append(c);
                        while (++i < line.Length)
                        {
                            buildLine.Append(line[i]);
                        }
                    }

                    #endregion

                    else if (c == '{' && !inComment && !inString)
                    {
                        buildLine.Append(c + "\n");
                        tabcount++;
                        //subtracttab--;
                    }
                    else if (c == '}' && !inComment && !inString)
                    {
                        tabcount--;
                        buildLine.AppendFormat("\n{0}{1}\n",tabstr.Times(tabcount),c);
                    }
                    else if (c == ';' && !inComment && !inString)
                    {
                        buildLine.AppendFormat("{0}\n", c);
                    }
                    else
                        buildLine.Append(c);

                    prevChar = setPrvChr;
                }
                if (blockString || blockComment)
                {
                    subtracttab = tabcount * -1;
                }
    
                codeOut.AppendFormat("{0}{1}\n",tabstr.Times(tabcount+subtracttab), buildLine.ToString());
            }

            string output = codeOut.ToString();
            output = Regex.Replace(output, "\n+", "\n");

            return output;

            //return TabRightCold(code, tabcount);
        }


        /// <summary>
        /// Tabs a file based on Visual Basic style syntax
        /// </summary>
        private static string TabRightVB(string text)
        {
        return TabRightVB(text, 0);
        }
        /// <summary>
        /// Tabs a file based on Visual Basic style syntax
        /// </summary>
        /// <param name="tabcount">Number of tabs to assume for base</param>
        private static string TabRightVB(string text, int tabcount)
        {
            text = Regex.Replace(text,@"_\s*\n\s*", " ");
            text = Regex.Replace(text,@"else\s*('.+)", "else\n$1",RegexOptions.IgnoreCase);
            text = Regex.Replace(text,@"then\s*('.+)", "Then\n$1", RegexOptions.IgnoreCase);

            Regex tplus = new Regex(@"^(while|for|if|elseif|class|function|sub|select\s+case|do|(private\s+|public\s+|friend\s+|protected\s+)?(shared\s+|mustinherit\s+|sealed\s+)?(function|class|sub|property|module))[\s]", RegexOptions.IgnoreCase);
            Regex singleLineIf = new Regex("then$", RegexOptions.IgnoreCase);
            Regex tabBecauseIf = new Regex(@"^if", RegexOptions.IgnoreCase);
            //Regex negateIf2 = new Regex(@"then$", RegexOptions.IgnoreCase);
            Regex tminus = new Regex(@"^(wend|until|loop|next|elseif|end\s+(if|function|sub|class|select|property))[\s]?", RegexOptions.IgnoreCase);
            Regex isLabel = new Regex(@"[^\s]:$");
            Regex elif = new Regex(@"^elseif.+then$", RegexOptions.IgnoreCase);

            //special instructions for "select case" statements
            Regex casebound = new Regex(@"^(select case|end select)", RegexOptions.IgnoreCase);
            Regex caseitem = new Regex(@"^case\s", RegexOptions.IgnoreCase);

			SettingsObtainer obt = SettingsObtainer.CreateInstance();
			string tabstr = obt.TabString;
            bool incase = false;

            //get rid of multiline rows
            text = Regex.Replace(text, "_[ \t]+\n[ \t]*", " ");

            string[] rows = text.Split('\n');
            for (int ii = 0; ii < rows.Length; ii++)
            {

                int currTabc = 0;
                rows[ii] = Regex.Replace(Regex.Replace(rows[ii], @"\s+$", ""), @"^\s+", "");
                rows[ii] = rows[ii].Replace("\n", "").Replace("\r", "");

                if (casebound.IsMatch(rows[ii]))
                {
                    if (incase)
                    {
                        tabcount--;
                        incase = false;
                    }
                    else
                    {
                        incase = true;
                        tabcount++;
                        //I know I'm doubling the tab count, I want to
                    }
                }

                if (tminus.IsMatch(rows[ii]) && !isLabel.IsMatch(rows[ii]))
                {
                    tabcount--;
                }
                if (tabcount < 0) { tabcount = 0; }
                currTabc = tabcount;

                string currentLine = "";
                //if (incase && !caseitem.IsMatch(rows[ii])) {
                //	currTabc += 1;
                //}
                if (isLabel.IsMatch(rows[ii]))
                {
                    currentLine = rows[ii];
                }
                else if (incase && caseitem.IsMatch(rows[ii]) && tabcount > 0)
                {
                    currentLine = tabstr.Times(tabcount - 1) + rows[ii];
                }
                else if (rows[ii].ToLower().Trim() == "else" && tabcount > 0)
                {
                    currentLine = tabstr.Times(tabcount - 1) + rows[ii];
                }
                else
                {
                    currentLine = tabstr.Times(currTabc) + rows[ii];
                }

                if (tplus.IsMatch(rows[ii]) && !isLabel.IsMatch(rows[ii]))
                {
                    if (tabBecauseIf.IsMatch(rows[ii]))
                    {
                        if (singleLineIf.IsMatch(rows[ii]))
                        {
                            tabcount++;
                        }
                    }
                    else
                    {
                        tabcount++;
                    }
                }
                currTabc = tabcount;
                if (currTabc < 0) { currTabc = 0; }
                rows[ii] = currentLine;
            }
            
            return Regex.Replace(String.Join("\n", rows), "(\t|    )Select\\s+Case","Select Case",RegexOptions.IgnoreCase);
        }

        
    }
}
