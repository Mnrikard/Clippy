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
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class ToBase : AClipEditor
    {
        #region boilerplate

        public override string EditorName
        {
            get { return "ToBase"; }
        }

        public override string ShortDescription
        {
            get { return "Converts from/to base integers"; }
        }

        public override string LongDescription
        {
            get
            {
                return @"ToBase
Syntax: clippy tobase [baseint] [reverse]
Description

Converts from decimal to a base number system and back
For instance 
clippy tobase 16 will convert 255 to FF
clippy tobase 16 -1 will convert FF to 255

baseint - The base number: for example binary would be 2
          Octal would be 8
          Hexidecimal would be 16

reverse - To convert back from a base to the decimal use ""reverse""
";
            }
        }

        private static bool IsBaseNumber(string b)
        {
            byte bb;
            if (Byte.TryParse(b, out bb))
            {
                if (bb <= 36 && bb >= 2)
                    return true;
            }
            return false;
        }

        public override void DefineParameters()
        {
            _parameterList = new List<Parameter>();
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Base Number",
                Sequence = 1,
                Validator = IsBaseNumber,
                DefaultValue = "16",
                Required = true,
                Expecting = "A number between 2 and 36"
            });
            _parameterList.Add(new Parameter()
            {
                ParameterName = "Reverse",
                Sequence = 2,
                Validator = a => (a.Trim()==String.Empty || "reverse".Equals(a, StringComparison.CurrentCultureIgnoreCase)),
                DefaultValue = String.Empty,
                Required = false,
                Expecting = "the word \"reverse\""
            });
        }

        #endregion

        //you don't need to override this
        public override void SetParameters(string[] args)
        {
            for(int i=1;i<ParameterList.Count;i++)
                ParameterList[i].Value = ParameterList[i].DefaultValue;
            if (args.Length > 1)
            {
                SetParameter(1, args[1]);
                if (args.Length > 2)
                    SetParameter(2,args[2]);
            }
        }

        public override void Edit()
        {
            byte basenum;
            if (!Byte.TryParse(ParameterList[0].Value, out basenum))
            {
                RespondToExe("Base number is not a number");
                return;
            }
            if (ParameterList[1].Value.Equals("reverse", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    long output = ToDecimal(SourceData, basenum);
                    SourceData = output.ToString();
                }
                catch
                {
                    RespondToExe("Source data cannot be converted to a number");
                    return;
                }
            }
            else
            {
                int sourceint;
                if (!Int32.TryParse(SourceData, out sourceint))
                {
                    RespondToExe("Cannot convert source data to an integer(64 bit)");
                    return;
                }
                SourceData = ConvertToBase(sourceint, basenum);
            }
        }

        private const string baseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static string ConvertToBase(int decNum, int baseNum)
        {
            if (decNum < baseNum)
                return baseChars[decNum].ToString();
            return ConvertToBase((int)Math.Floor((decimal)(decNum / baseNum)), baseNum) + baseChars[decNum % baseNum];
        }

        private static long ToDecimal(string baseString, int baseNum)
        {
            if (baseString.Length == 1)
                return baseChars.IndexOf(baseString.ToUpper());
            return (baseChars.IndexOf(baseString.ToUpper()[0]) * ((long)Math.Pow(baseNum, (baseString.Length - 1)))) + ToDecimal(baseString.Substring(1), baseNum);
        }

        
    }
}
