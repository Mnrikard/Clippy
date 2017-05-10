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

namespace ClippyLib
{
    public interface IClipEditor
    {
        void Edit();
        void SetParameters(string[] arguments);
        string EditorName { get; }
        string ShortDescription { get; }
		EditorDescription LongDescription { get; }
        string SourceData { get; set; }

        bool HasAllParameters { get; }
        Parameter GetNextParameter();
        void SetNextParameter(string parameter);
        void SetParameter(int parameterSequence, string parameter);
        List<Parameter> ParameterList { get; }

        void DefineParameters();

        void GetClipboardContent();
        void SetClipboardContent();

        event EventHandler<EditorResponseEventArgs> EditorResponse;
        event EventHandler<EditorResponseEventArgs> PersistentEditorResponse;
    }
}
