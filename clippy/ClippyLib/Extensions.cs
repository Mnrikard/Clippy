using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    }
}
