﻿/*
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
using System.Windows.Forms;
using System.Text;

namespace ClippyLib
{
    public abstract class AClipEditor : IClipEditor
    {
        #region abstract methods

        public abstract void Edit();

		protected string Name;
		protected string Description;
		internal string exampleInput;
		internal string exampleCommand;
		internal string exampleOutput;

		public virtual string EditorName { get {return Name;} }

		public virtual string ShortDescription { get { return Description; } }


		// todo: oooh, what if the example somehow got to be part of the unit test!!!
        public virtual string LongDescription 
		{ 
			get
			{
				return String.Concat(Name, Environment.NewLine,
				                     "Syntax: ", Name.ToLower() ,SyntaxParameters(), Environment.NewLine,
				                     Environment.NewLine,
				                     ShortDescription, Environment.NewLine,
				                     DescribeParameters(),
				                     Environment.NewLine,
				                     "Example:", Environment.NewLine,
				                     "Original content: ", ClipUnEscape(exampleInput), Environment.NewLine,
				                     "Command: ", ClipUnEscape(exampleCommand), Environment.NewLine,
				                     "New content: ", ClipUnEscape(exampleOutput));

			}
		}

		private string SyntaxParameters()
		{
			StringBuilder output = new StringBuilder();
			foreach(Parameter p in ParameterList)
			{
				output.Append(" ");
				output.Append(p.Required ? "\"" : "[");
				output.Append(p.ParameterName.Replace(" ",""));
				output.Append(p.Required ? "\"" : "]");
			}
			return output.ToString();
		}

		private string DescribeParameters()
		{
			StringBuilder output = new StringBuilder();
			foreach(Parameter p in ParameterList)
			{
				output.Append(p.ParameterName.Replace(" ",""));
				output.Append(" - ");
				output.Append(p.Expecting);
				if(!p.Required && p.DefaultValue != null)
				{
					output.AppendFormat(". Defaults to \"{0}\"", ClipUnEscape(p.DefaultValue));
				}
				output.AppendLine();
			}
			return output.ToString();
		}


        #endregion

        #region public Properties

        public string SourceData { get; set; }
        protected List<Parameter> _parameterList;
        public List<Parameter> ParameterList { get { return _parameterList; } }

        #endregion

        #region reusable methods

        public virtual void SetParameters(string[] arguments)
        {
            for (int i = 1; i < arguments.Length; i++ )
            {
                SetNextParameter(arguments[i]);
            }
        }

        protected void RespondToExe(string message)
        {
            RespondToExe(message, true);
        }

        protected void RespondToExe(string message, bool requiresUserAction)
        {
            EditorResponseEventArgs e = new EditorResponseEventArgs()
            {
                ResponseString = message,
                RequiresUserAction = requiresUserAction
            };
            OnEditorResponse(e);
        }

        protected void PersistentRespondToExe(string message, bool requiresUserAction)
        {
            EditorResponseEventArgs e = new EditorResponseEventArgs()
            {
                ResponseString = message,
                RequiresUserAction = requiresUserAction
            };
            OnPersistentEditorResponse(e);
        }

		//todo: remove this after replacing all refs with Parameter.GetEscapedValue
        protected string ClipEscape(string input)
        {
            return input.Replace("\\q", "\"")
                .Replace("\\t", "\t")
                .Replace("\\n", "\n");
        }

		protected string ClipUnEscape(string input)
		{
			return input.Replace("\"", "\\q")
				.Replace("\t", "\\t")
				.Replace("\n", "\\n");
		}

        public bool HasAllParameters
        {
            get
            {
                foreach (Parameter p in ParameterList)
                {
                    if (!p.IsValued && p.Required)
                        return false;
                }
                return true;
            }
        }

        public Parameter GetNextParameter()
        {
            return (from Parameter p in ParameterList
			        where p.IsValued == false
			        orderby p.Sequence
			        select p).FirstOrDefault();            
        }

        public void SetNextParameter(string parameterValue)
        {
            Parameter parm = (from p in ParameterList
                              where p.IsValued == false
                              orderby p.Sequence
                              select p).FirstOrDefault();
            if (parm == null)
                return;
            parm.Value = parameterValue;
        }

        public void SetParameter(int parameterSequence, string parameterValue)
        {
            Parameter parm = (from p in ParameterList
                              where p.Sequence == parameterSequence
                              select p).FirstOrDefault();
            if (parm == null)
                throw new IndexOutOfRangeException("ParameterList out of range.");
            parm.Value = parameterValue;
        }

        public abstract void DefineParameters();

        #endregion

        #region Clipboard access

        public void GetClipboardContent()
        {
            if (Clipboard.ContainsText(TextDataFormat.UnicodeText))
            {
                SourceData = Clipboard.GetText(TextDataFormat.UnicodeText);
                SourceData = SourceData.Replace("\r", String.Empty);
            }
            else
                SourceData = String.Empty;
        }

        public void SetClipboardContent()
        {
            string newData = SourceData
                .Replace("\r", String.Empty)
                .Replace("\n", "\r\n");
            if (String.IsNullOrEmpty(newData))
                return;
            Clipboard.SetText(newData);
        }
        public void SetClipboardContent(string input)
        {
            SourceData = input;
            SetClipboardContent();
        }

        #endregion

        #region events

        public event EventHandler<EditorResponseEventArgs> EditorResponse;

        public virtual void OnEditorResponse(EditorResponseEventArgs e)
        {
            if (this.EditorResponse != null)
            {
                this.EditorResponse(this, e);
            }
        }

        public event EventHandler<EditorResponseEventArgs> PersistentEditorResponse;

        public virtual void OnPersistentEditorResponse(EditorResponseEventArgs e)
        {
            if (this.PersistentEditorResponse != null)
            {
                this.PersistentEditorResponse(this, e);
            }
        }

        #endregion

    }
}
