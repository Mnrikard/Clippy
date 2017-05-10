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
using System.IO;

namespace ClippyLib
{
    public static class Extensions
    {
        public static string Times(this string baseString, int multiplier)
        {
            if (multiplier < 1)
            {
                return String.Empty;
            }
            char[] cc = new char[baseString.Length * multiplier];
            char[] baseStringchars = baseString.ToCharArray();
            int ccindex = 0;
            for (int i = 0; i < multiplier; i++)
            {
                for (int j = 0; j < baseStringchars.Length; j++)
                {
                    cc[ccindex++] = baseStringchars[j];
                }
            }
            return new String(cc);
        }

        public static Regex ToRegex(this string spattern)
        {
            Regex repper;
            Match pattern = Regex.Match(spattern, "^/(?<pattern>.+)/(?<options>[mi]*)$");
            if (pattern.Success)
            {
                RegexOptions rxOptions = 0;
                string opts = pattern.Groups["options"].Value.ToLower();
                if (opts.Contains("m"))
                    rxOptions |= RegexOptions.Multiline;
                if (opts.Contains("i"))
                    rxOptions |= RegexOptions.IgnoreCase;
                
                repper = new Regex(pattern.Groups["pattern"].Value, rxOptions);
            }
            else
                repper = new Regex(spattern, RegexOptions.IgnoreCase);
            return repper;
        }

        public static SuperRegex ToSuperRegex(this string spattern)
        {
            SuperRegex repper;
            Match pattern = Regex.Match(spattern, "^/(?<pattern>.+)/(?<options>[mi]*)$");
            if (pattern.Success)
            {
                RegexOptions rxOptions = 0;
                string opts = pattern.Groups["options"].Value.ToLower();
                if (opts.Contains("m"))
                    rxOptions |= RegexOptions.Multiline;
                if (opts.Contains("i"))
                    rxOptions |= RegexOptions.IgnoreCase;

                repper = new SuperRegex(pattern.Groups["pattern"].Value, rxOptions);
            }
            else
                repper = new SuperRegex(spattern, RegexOptions.IgnoreCase);
            return repper;
        }

        public static bool IsInteger(this string possibleNumber)
        {
            int output;
            return Int32.TryParse(possibleNumber, out output);
        }

		public static string GetLocalFile(string filename)
		{
			string currentAppDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			return Path.Combine(currentAppDir,filename);
		}
    }
}
