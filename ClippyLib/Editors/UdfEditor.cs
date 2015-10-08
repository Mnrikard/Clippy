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
using System.Xml;
using ClippyLib.Settings;
using System.IO;
using System.Xml.Linq;

namespace ClippyLib.Editors
{
    public class UdfEditor : AClipEditor
    {
        private string _udfName;
        private XmlDocument _udfSettings;
        private string[] _arguments;
        private Dictionary<string, string> _udfParameters;
        private List<Parameter> _xmlDefinedParms;
		private SettingsObtainer _settings;
		private List<UserFunction> _userFunctions;

		public override string EditorName
        {
            get { return "Will not be a normal usable function"; }
        }

        #region .ctor
        public UdfEditor()
        {
			Name = "Will not be a normal usable function";
			Description = "#";

            _udfParameters = new Dictionary<string, string>();
            _xmlDefinedParms = new List<Parameter>();
			_settings = SettingsObtainer.CreateInstance();
			GetListOfUserFunctions();
        }
        public UdfEditor(string udfName) : this()
        {
            _udfName = udfName;
            GetCommandsForFunction(new[] { udfName });
        }
        #endregion

		private void GetListOfUserFunctions()
		{
			_userFunctions = new List<UserFunction>();

			SettingsObtainer obt = SettingsObtainer.CreateInstance();
			string udfLocation = obt.UdfLocation;

			if(!File.Exists(udfLocation))
				throw new UndefinedFunctionException("No UDF file is set.");


			XDocument udfDoc = new XDocument(udfLocation);
			foreach(XElement command in udfDoc.Root.Elements("command"))
			{
				_userFunctions.Add(new UserFunction(command));
			}
		}

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

            Queue<string> functions = GetCommandsForFunction(_arguments);
            if (functions.Count == 0)
                throw new UndefinedFunctionException(String.Format("Function:{0} does not exist or is not valid", _udfName));
            
            ExecuteSubFunctions (manager, functions);
        }

		private void ExecuteSubFunctions (EditorManager manager, Queue<string> functions)
		{
			while (functions.Any()) 
			{
				string function = ReplaceDynamicParameters(functions.Dequeue());

				string[] fargs = function.ParseArguments();

				manager.GetClipEditor (fargs[0]);
				manager.ClipEditor.EditorResponse += HandleResponseFromClippy;
				manager.ClipEditor.DefineParameters();
				manager.ClipEditor.SetParameters(fargs);

				if (!manager.ClipEditor.HasAllParameters) {
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

		private string ReplaceDynamicParameters(string function)
		{
			for (int i = 1; i < _arguments.Length; i++) 
			{
				string dynamicParameter = String.Concat ("%", (i - 1), "%");
				function = function.Replace (dynamicParameter, _arguments[i]);
			}

			for (int i = 0; i < ParameterList.Count; i++) 
			{
				string dynamicParameter = String.Concat("%",i.ToString(),"%");
				function = function.Replace (dynamicParameter, ParameterList[i].Value);
			}

			return function;
		}

		private UserFunction GetUserFunction(string[] key)
		{
			_userFunctions.Where(f => f.Name.Equals(key[0], StringComparison.CurrentCultureIgnoreCase));
		}

		private void InitializeSubParameters(UserFunction function)
		{
			foreach(string cmd in function.SubFunctions)
			{
				MatchCollection udfparms = Regex.Matches(cmd, @"%(\d+)%");
				foreach (Match udfparm in udfparms)
				{
					string parameterName = DescribeParm(udfparm.Groups[1].Value);
					_udfParameters[parameterName] = String.Empty;
				}
			}
		}

		private void SetTheseParameters(UserFunction command)
		{
			foreach(UserFunction.UserParameter parm in command.Parameters)
			{
				_xmlDefinedParms.Add(new Parameter()
			    {
					ParameterName = parm.Name,
					Sequence = parm.Sequence,
					DefaultValue = parm.DefaultValue ?? string.Empty, 
					Required = parm.Required,
					Expecting = parm.Description
				});
			}
		}

        private Queue<string> GetCommandsForFunction(string[] key)
        {
            Queue<string> output = new Queue<string>();

            if (_udfSettings == null)
            {
                _udfSettings = UdfDocument();
            }
            XmlNode cmdNode = _udfSettings.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + key[0].ToLower() + "\"]");
			//XmlNode nd = _udfSettings.Nodes
            if (cmdNode == null)
            {
                return output;
            }

            XmlNodeList cmdParms = cmdNode.SelectNodes("parameter");
            XmlNodeList cmds = cmdNode.SelectNodes("function");
            foreach (XmlNode cmd in cmds)
            {
                string currcmd = cmd.InnerText;
				output.Enqueue(currcmd);

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
			if(key.Length < 1)
				return false;

            if (_udfSettings == null)
            {
                _udfSettings = UdfDocument();
            }
            
			XmlNode cmd = _udfSettings.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + key[0].ToLower() + "\"]");
            return cmd != null;
        }

        

        public void DescribeFunctions(StringBuilder output)
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
        
        public bool GetUdfDocument(out XmlDocument xdoc)
        {
			string udfLocation = _settings.UdfLocation;

			xdoc = new XmlDocument();

			if(File.Exists(udfLocation))
            {
				xdoc.Load(udfLocation.ToString());
				return true;
            }
            
			xdoc.LoadXml("<commands />");
            return false;            
        }

        public XmlDocument UdfDocument()
        {
            XmlDocument xdoc;
            GetUdfDocument(out xdoc);
            return xdoc;
        }
        
        public List<string> GetFunctions()
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
