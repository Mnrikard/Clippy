﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClippyLib.Editors;

namespace ClippyLib
{
    public class EditorManager
    {
        public readonly List<IClipEditor> Editors;

        public EditorManager()
        {
            Editors = new List<IClipEditor>();

            Editors.Add(new Capitalize());
            Editors.Add(new ClipSort());
            Editors.Add(new ChunkText());
            Editors.Add(new ColumnAlign());
            Editors.Add(new Count());
            Editors.Add(new Dedupe());
            Editors.Add(new Encode());
            Editors.Add(new Grep());
            Editors.Add(new Rep());
            Editors.Add(new Reverse());
            Editors.Add(new Snippet());
            Editors.Add(new SqlInsert());
            Editors.Add(new TabRight());
            Editors.Add(new ToBase());
            Editors.Add(new XmlEdit());
        }

        private IClipEditor _clipEditor;
        public IClipEditor ClipEditor
        {
            get { return _clipEditor; }
            protected set { _clipEditor = value; }
        }

        public IClipEditor GetClipEditor(string editorName)
        {
            ClipEditor = (from e in Editors
                          where e.EditorName.Equals(editorName, StringComparison.CurrentCultureIgnoreCase)
                          select e).FirstOrDefault();
            if (ClipEditor == null)
                ClipEditor = new UdfEditor();
            ClipEditor.DefineParameters();
            return ClipEditor;
        }

        public string[] GetEditors()
        {
            List<string> eds = (from e in Editors
                                select e.EditorName).ToList();
            eds.AddRange(UdfEditor.GetFunctions());
            return eds.ToArray();
        }

        public string Help(string[] arguments)
        {
            if (arguments.Length > 1)
            {
                ClipEditor = (from e in Editors
                              where e.EditorName.Equals(arguments[1], StringComparison.CurrentCultureIgnoreCase)
                              select e).FirstOrDefault();
                if(ClipEditor != null)
                    return ClipEditor.LongDescription;
            }
            StringBuilder output = new StringBuilder();
            foreach (IClipEditor ce in Editors)
            {
                output.AppendFormat("{0}  -  {1}\r\n", ce.EditorName, ce.ShortDescription);
            }
            output.Append("---------------------\r\nUser Defined\r\n---------------------\r\n");
            UdfEditor.DescribeFunctions(output);

            return output.ToString();
        }

        public string[] GetArgumentsFromString(string arglist)
        {
            return UdfEditor.GetArgsFromString(arglist);
        }
    }
}