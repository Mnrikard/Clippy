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
        private Dictionary<string, string> _udfParameters;
        private List<Parameter> _xmlDefinedParms;
		private UserFunctionsList _userFunctions;
		private UserFunction _selectedFunction;

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
			_userFunctions = new UserFunctionsList();
			DefineParameters();
        }
        public UdfEditor(string udfName) : this()
        {
            _udfName = udfName;
            _selectedFunction = _userFunctions.GetUserFunction(udfName);
			ConvertUserParameterToParameter(_selectedFunction);
			//DiscoverUnnamedParameters(_selectedFunction);
			//DefineParameters();
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

        public override void SetParameters(string[] args)
        {
            _udfName = args[0];
			_selectedFunction = _selectedFunction ?? _userFunctions.GetUserFunction(_udfName);

            if (null == _selectedFunction)
            {
                throw new UndefinedFunctionException("Function \"{0}\" does not exist", _udfName);
            }

			//DiscoverUnnamedParameters(_selectedFunction);
			//ConvertUserParameterToParameter(_selectedFunction);
            
			for (int i = 1; i < args.Length; i++)
            {
                SetNextParameter(args[i]);
            }
        }

        public override void Edit()
        {
            EditorManager manager = new EditorManager();
            
			if(ParameterList.Count > 0 && ParameterList[0].Value.Equals("help", StringComparison.CurrentCultureIgnoreCase))
            {
				string[] parmlist = new string[ParameterList.Count];
				for(int i=0;i<parmlist.Length;i++)
				{
					parmlist[i] = ParameterList[i].Value;
				}
                RespondToExe(manager.Help(parmlist));
                return;
            }

			_selectedFunction = _selectedFunction ?? _userFunctions.GetUserFunction(ParameterList[0].Value);
			if (_selectedFunction == null || _selectedFunction.SubFunctions.Count == 0)
				throw new UndefinedFunctionException(String.Format("Function:{0} does not exist or is not valid", _udfName));


            ExecuteSubFunctions (manager);
        }

		private void ExecuteSubFunctions (EditorManager manager)
		{

			while (_selectedFunction.SubFunctions.Any()) 
			{
				string function = ReplaceDynamicParameters(_selectedFunction.SubFunctions.Dequeue());

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
			if(ParameterList != null)
			{
				for (int i = 0; i < ParameterList.Count; i++) 
				{
					string dynamicParameter = String.Concat("%",i.ToString(),"%");
					function = function.Replace (dynamicParameter, ParameterList[i].Value);
				}
			}

			return function;
		}

		private void DiscoverUnnamedParameters(UserFunction function)
		{
			if(null == function)
			{
				return;
			}

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

		private void ConvertUserParameterToParameter(UserFunction command)
		{
			if (null == command)
			{
				return;
			}

			foreach(UserFunction.UserParameter parm in command.Parameters)
			{
				Parameter p = new Parameter()
			    {
					ParameterName = parm.Name,
					Sequence = parm.Sequence,
					DefaultValue = parm.DefaultValue ?? string.Empty, 
					Required = parm.Required,
					Expecting = parm.Description
				};
				_xmlDefinedParms.Add(p);
				ParameterList.Add(p);
			}
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


    }
}
