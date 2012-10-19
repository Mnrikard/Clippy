using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClippyLib
{
    public abstract class AClipEditor : IClipEditor
    {
        #region abstract methods

        public abstract void Edit();

        public abstract string EditorName { get; }

        public abstract string ShortDescription { get; }

        public abstract string LongDescription { get; }

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


        protected string ClipEscape(string input)
        {
            return input.Replace("\\q", "\"")
                .Replace("\\t", "\t")
                .Replace("\\n", "\n");
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

        public string GetNextParameterName()
        {
            Parameter parm = (from p in ParameterList
                              where p.IsValued == false
                              orderby p.Sequence
                              select p).FirstOrDefault();
            if (parm == null)
                return null;
            return parm.ParameterName;
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
