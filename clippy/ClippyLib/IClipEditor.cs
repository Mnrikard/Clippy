using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClippyLib
{
    public interface IClipEditor
    {
        void Edit();
        void SetParameters(string[] arguments);
        string EditorName { get; }
        string ShortDescription { get; }
        string LongDescription { get; }
        string SourceData { get; set; }

        bool HasAllParameters { get; }
        string GetNextParameterName();
        void SetNextParameter(string parameter);
        void SetParameter(int parameterSequence, string parameter);
        List<Parameter> ParameterList { get; }

        void DefineParameters();

        void GetClipboardContent();
        void SetClipboardContent();

        event EventHandler<EditorResponseEventArgs> EditorResponse;
    }
}
