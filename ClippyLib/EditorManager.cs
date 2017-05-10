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
using ClippyLib.Editors;

namespace ClippyLib
{
    public class EditorManager
    {
        public readonly List<IClipEditor> Editors;

        public EditorManager()
        {
            Editors = new List<IClipEditor>();
			
			//todo: don't make each editor a singleton, make it in an abstract factory
            Editors.Add(new Capitalize());
            Editors.Add(new ClipSort());
            Editors.Add(new ChunkText());
            Editors.Add(new ColumnAlign());
            Editors.Add(new Count());
            Editors.Add(new Dedupe());
            Editors.Add(new Encode());
            Editors.Add(new Grep());
			Editors.Add(new Math());
            Editors.Add(new NewText());
            Editors.Add(new Rep());
            Editors.Add(new Reverse());
            Editors.Add(new SnippetTextEditor());
            Editors.Add(new SqlInsert());
            Editors.Add(new TabRight());
            Editors.Add(new ToBase());
            Editors.Add(new XmlEdit());
			Editors.Add(new SetSourceData());
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
                ClipEditor = new UdfEditor(editorName);
			return ClipEditor;
        }

        public string[] GetEditors()
        {
            List<string> eds = (from e in Editors
                                select e.EditorName).ToList();
			UserFunctionsList ufl = new UserFunctionsList();
            eds.AddRange(ufl.GetFunctions());
            return eds.ToArray();
        }
        
        const string Disclaimer = @"Copyright 2012-2015 Matthew Rikard.
Clippy is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
at your option) any later version.

Clippy is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

To obtain a copy of the GNU General Public License, see:
<http://www.gnu.org/licenses/>.

";

        public EditorDescription Help(string[] arguments)
        {
            if (arguments.Length > 1)
            {
                ClipEditor = (from e in Editors
                              where e.EditorName.Equals(arguments[1], StringComparison.CurrentCultureIgnoreCase)
                              select e).FirstOrDefault();
                if(ClipEditor == null)
                {
					EditorDescription foundEditors = new EditorDescription();
                	foreach(IClipEditor ci in (from e in Editors
                	                           where e.EditorName.ToLower().Contains(arguments[1].ToLower())
                	                           || arguments[1].ToLower().Contains(e.EditorName.ToLower())
                	                           || e.LongDescription.Contains(arguments[1].ToLower())
                	                           select e))
                	{
                		foundEditors.Append(EditorDescription.Category.Emphasized, ci.EditorName);
						foundEditors.AppendLine(EditorDescription.Category.PlainText, String.Concat(" - ", ci.ShortDescription));
                	}
					return foundEditors;
                }
            	return ClipEditor.LongDescription;
            }

			var allEditors = new EditorDescription();
			allEditors.AppendLine(EditorDescription.Category.Warning, Disclaimer);
            foreach (IClipEditor ce in Editors)
            {				
				allEditors.Append(EditorDescription.Category.Emphasized, ce.EditorName);
				allEditors.AppendLine(EditorDescription.Category.PlainText, String.Concat(" - ", ce.ShortDescription));
            }
			allEditors.AppendLine("---------------------");
			allEditors.AppendLine("User Defined");
			allEditors.AppendLine("---------------------");

			UserFunctionsList ufl = new UserFunctionsList();
            ufl.DescribeFunctions(allEditors);

			return allEditors;
        }

        public string[] GetArgumentsFromString(string arglist)
        {
            return arglist.ParseArguments();
        }
                
    }
}
