/*
 * 
 * Copyright 2012 Matthew Rikard
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
using System.Xml;
using Microsoft.Win32;

namespace ClippyLib.Editors
{
    public class UdfEditor : AClipEditor
    {
        private string _udfName;
        private XmlDocument _udfSettings;
        private string[] _arguments;
        private Dictionary<string, string> _udfParameters;
        private List<Parameter> _xmlDefinedParms;

        public override string EditorName
        {
            get { return "Will not be a normal usable function"; }
        }

        #region .ctor
        public UdfEditor()
        {
            _udfParameters = new Dictionary<string, string>();
            _xmlDefinedParms = new List<Parameter>();
        }
        public UdfEditor(string udfName)
        {
            _udfName = udfName;
            _udfParameters = new Dictionary<string, string>();
            _xmlDefinedParms = new List<Parameter>();
            Udf(new[] { udfName });
        }
        #endregion

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            
            if(_xmlDefinedParms.Count > 0)
            {
                foreach(Parameter p in _xmlDefinedParms)
                {
                    p.Validator = a => true;

                    _parameterList.Add(p);
                }
            }
            else
            {
                foreach (string key in _udfParameters.Keys)
                {
                    int numberseq = Int32.Parse(Regex.Match(key, @"^\d+").Value)+1;
                    _parameterList.Add(new Parameter()
                    {
                        ParameterName = key,
                        Sequence = numberseq,
                        Validator = (a => true),
                        DefaultValue = String.Empty,
                        Required = false,
                        Expecting = String.Empty
                    });
                }
            }
        }

        public override string ShortDescription
        {
            get { return "Will not be registered"; }
        }

        public override string LongDescription
        {
            get { return @"Will not be registered"; }
        }

        public override void SetParameters(string[] args)
        {
            _udfName = args[0];
            _arguments = args;
            if (!CommandExists(args))
            {
                throw new UndefinedFunctionException("Function \"{0}\" does not exist", args[0]);
            }
            for (int i = 1; i < _arguments.Length; i++)
            {
                SetNextParameter(_arguments[i]);
            }
        }

        public override void Edit()
        {
            EditorManager manager = new EditorManager();
            
            if(_arguments[0].Equals("help", StringComparison.CurrentCultureIgnoreCase))
            {
                RespondToExe(manager.Help(_arguments));
                return;
            }

            List<string> functions = Udf(_arguments);
            if (functions.Count == 0)
                throw new UndefinedFunctionException(String.Format("Function:{0} does not exist or is not valid", _udfName));
            
            //there could be multiple functions per UDF, one per line
            for (int fi = 0; fi < functions.Count; fi++)
            {
                string function = functions[fi];
                
                for (int i = 1; i < _arguments.Length; i++)
                {
                    function = function.Replace("%" + (i - 1).ToString() + "%", _arguments[i]);
                }
				                
                for (int i = 0; i < ParameterList.Count; i++)
                {
                    function = function.Replace("%" + i.ToString() + "%", ParameterList[i].Value);
                }
            
                string[] fargs = GetArgsFromString(function);
                manager.GetClipEditor(fargs[0]);

                manager.ClipEditor.EditorResponse += new EventHandler<EditorResponseEventArgs>(HandleResponseFromClippy);

                manager.ClipEditor.DefineParameters();
                manager.ClipEditor.SetParameters(fargs);
                if (!manager.ClipEditor.HasAllParameters)
                {
                    throw new Exception(String.Format("Not all parameters are passed in the user defined function {0}, function: {1}", _udfName, function));
                }
                manager.ClipEditor.SourceData = SourceData;
                manager.ClipEditor.Edit();
                SourceData = manager.ClipEditor.SourceData;

                manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;

            }
        }

        private void HandleResponseFromClippy(object sender, EditorResponseEventArgs e)
        {
            RespondToExe(e.ResponseString, e.RequiresUserAction);
        }

        private List<string> Udf(string[] key)
        {
            List<string> output = new List<string>();

            if (_udfSettings == null)
            {
                _udfSettings = UdfDocument();
            }
            XmlNode cmdNode = _udfSettings.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + key[0].ToLower() + "\"]");
            if (cmdNode == null)
            {
                return output;
            }

            XmlNodeList cmdParms = cmdNode.SelectNodes("parameter");
            XmlNodeList cmds = cmdNode.SelectNodes("function");
            foreach (XmlNode cmd in cmds)
            {
                string currcmd = cmd.InnerText;
                //for (int i = 1; i < key.Length; i++)
                //{
                //    currcmd = currcmd.Replace("%" + (i - 1).ToString() + "%", key[i]);
                //}
                output.Add(currcmd);

                if (cmdParms.Count == 0)
                {
                    MatchCollection udfparms = Regex.Matches(currcmd, @"%(\d+)%");
                    foreach (Match udfparm in udfparms)
                    {
                        _udfParameters[DescribeParm(udfparm.Groups[1].Value)] = String.Empty;
                    }
                }
            }


            foreach(XmlNode parmnd in cmdParms)
            {
                _xmlDefinedParms.Add(new Parameter()
                {
                    ParameterName = parmnd.Attributes["name"].Value,
                    Sequence = Int32.Parse(parmnd.Attributes["sequence"].Value),
                    DefaultValue = parmnd.Attributes["default"] == null ? String.Empty : parmnd.Attributes["default"].Value,
                    Required = parmnd.Attributes["required"] == null ? false : Boolean.Parse(parmnd.Attributes["required"].Value),
                    Expecting = parmnd.Attributes["parmdesc"] == null ? String.Empty : parmnd.Attributes["parmdesc"].Value
                });
            }

            
            return output;
        }

        
        private string DescribeParm(string number)
        {
            int parmnum;
            if (!Int32.TryParse(number, out parmnum))
            {
                return number;
            }

            //special cases for teen numbers, and short circuit for lower numbers
            if (parmnum > 3 && parmnum < 21)
            {
                return number + "th parameter";
            }

            int remn = parmnum % 10;
            if (remn == 0 || (remn > 3 && remn < 10))
            {
                return number + "th parameter";
            }
            switch(remn)
            {
                case 1:
                    return number + "st parameter";
                case 2:
                    return number + "nd parameter";
                case 3:
                    return number + "rd parameter";
            }

            return number;
        }


        private bool CommandExists(string[] key)
        {
            if (_udfSettings == null)
            {
                _udfSettings = UdfDocument();
            }
            XmlNode cmd = _udfSettings.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + key[0].ToLower() + "\"]");
            return cmd != null;
        }

        public static string[] GetArgsFromString(string input)
        {
            string prevchar = "";
            bool isInString = false;
            string currentString = "";
            List<string> op = new List<string>();

            foreach (char c in input)
            {
                if (c == ' ')
                {
                    //multiple spaces between words
                    if (prevchar == " " && currentString.Length == 0 && !isInString)
                    {
                        currentString = "";
                    }
                    else if (!isInString)
                    {
                        op.Add(currentString);
                        currentString = "";
                    }
                    else
                    {
                        currentString += c;
                    }
                }
                else if (c == '"')
                {
                    if (prevchar != "\\")
                    {
                        isInString = !isInString;
                    }
                }
                else if (c == '\\')
                {
                    if (prevchar == "\\")
                    {
                        prevchar = "backslash";
                        continue;
                    }
                    currentString += c;
                }
                else
                {
                    currentString += c;
                }
                prevchar = c.ToString();
            }

            op.Add(currentString);

            string[] arrop = op.ToArray();
            return arrop;
        }

        public static void DescribeFunctions(StringBuilder output)
        {
            XmlDocument descUdf = UdfDocument();
            XmlNodeList cmds = descUdf.SelectNodes("//command");
            foreach (XmlNode udf in cmds)
            {
                string description = "No description available";
                XmlNode descriptionNode = udf.SelectSingleNode("description");
                if (descriptionNode != null)
                    description = descriptionNode.InnerText;
                XmlAttribute keyNameNode = udf.Attributes["key"];
                output.AppendFormat("{0}  -  {1}\r\n", keyNameNode.Value, description);
            }
        }
        
        public static bool GetUdfDocument(out XmlDocument xdoc)
        {
        	RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
            
            if(rkUdfLocation == null)
            {
            	xdoc = GetNullUdf();
            	return false;
            }
            
            object udfLocation = rkUdfLocation.GetValue("udfLocation");
            
            if(udfLocation == null || !System.IO.File.Exists(udfLocation.ToString()))
            {
            	xdoc = GetNullUdf();
            	return false;
            }
            
            xdoc = new XmlDocument();
            xdoc.Load(udfLocation.ToString());
            return true;
        }

        public static XmlDocument UdfDocument()
        {
            XmlDocument xdoc;
            GetUdfDocument(out xdoc);
            return xdoc;
        }
        
        private static XmlDocument GetNullUdf()
        {
        	XmlDocument xdoc = new XmlDocument();
        	xdoc.LoadXml("<commands />");

        	return xdoc;
        }

        public static List<string> GetFunctions()
        {
            XmlDocument descUdf = UdfDocument();
            XmlNodeList cmds = descUdf.SelectNodes("//command/@key");
            List<string> output = new List<string>();
            foreach (XmlNode udf in cmds)
            {
                output.Add(udf.Value);
            }
            return output;
        }
    }
}
