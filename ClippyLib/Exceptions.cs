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

namespace ClippyLib
{
    public class InvalidParameterException : System.Exception
    {
        public string ParameterMessage { get; private set; }
        public InvalidParameterException(string formatMessage, params object[] formatItems)
        {
            ParameterMessage = String.Format(formatMessage, formatItems);
        }
    }

    public class UndefinedFunctionException : System.Exception
    {
        public string FunctionMessage { get; private set; }
        public UndefinedFunctionException(string formatMessage, params object[] formatItems)
        {
            FunctionMessage = String.Format(formatMessage, formatItems);
        }
    }
}
