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
using System.Text.RegularExpressions;

namespace ClippyLib.Editors
{
    public class ToBase : AClipEditor
    {
        public ToBase()
		{
			Name = "ToBase";
			Description = "Converts from decimal to a base number system and back";
			exampleInput = "254";
			exampleCommand = "tobase 16";
			exampleOutput = "FE";
			DefineParameters();
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
            if (!Byte.TryParse(ParameterList[0].GetValueOrDefault(), out basenum))
            {
                RespondToExe("Base number is not a number");
                return;
            }

			Func<string,int,string> numberConverter;
			if(ParameterList[1].GetValueOrDefault().Equals("reverse", StringComparison.CurrentCultureIgnoreCase))
				numberConverter = ConvertToDecimal;
			else
				numberConverter = ConvertToBase;

			try
            {
				SourceData = numberConverter(SourceData, basenum);
            }
            catch
            {
                RespondToExe("Source data is not a number");
                return;
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

        private const string baseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string ConvertToBase(string input, int baseNum)
        {
			long decNum = Int64.Parse(input);
            if (decNum < baseNum)
                return baseChars[(int)decNum].ToString();
            return ConvertToBase(
				((int)System.Math.Floor((decimal)(decNum / baseNum))).ToString(), 
				baseNum) 
				+ baseChars[(int)(decNum % baseNum)];
        }

		private string ConvertToDecimal(string input, int baseNum)
		{
			return ToDecimal(input, baseNum).ToString();
		}

        private long ToDecimal(string baseString, int baseNum)
        {
            if (baseString.Length == 1)
                return baseChars.IndexOf(baseString.ToUpper());
            return (baseChars.IndexOf(baseString.ToUpper()[0]) * ((long)System.Math.Pow(baseNum, (baseString.Length - 1)))) + ToDecimal(baseString.Substring(1), baseNum);
        }

        
    }
}
