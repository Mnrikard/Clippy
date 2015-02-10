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
using Microsoft.Win32;

namespace ClippyLib
{
    public class RecentCommands
    {
        public static void SaveThisCommand(string editorName, string parms)
        {

            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkClippy = GetRegistryKey(hkcu, "Software\\Rikard\\Clippy\\MRU");

            string[] names = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            string currentValue = editorName.Trim() + " " + parms;
            List<string> keyValues = new List<string>(27);

            keyValues.Add(currentValue);

            foreach (string name in names)
            {
                object prevValue = rkClippy.GetValue(name) ?? String.Empty;
                if (prevValue.ToString().Equals(currentValue, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                keyValues.Add(prevValue.ToString());
            }

            for (int i = keyValues.Count; i < names.Length; i++)
            {
                keyValues.Add(String.Empty);
            }

            for (int i = 0; i < names.Length; i++)
            {
                rkClippy.SetValue(names[i], keyValues[i], RegistryValueKind.String);
            }

            rkClippy.Close();
            hkcu.Close();
        }

        public static string[] GetRecentCommandList()
        {
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkClippy = GetRegistryKey(hkcu, "Software\\Rikard\\Clippy\\MRU");

            string[] names = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] commands = new string[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                object ocommand = rkClippy.GetValue(names[i]);
                ocommand = ocommand ?? String.Empty;
                commands[i] = ocommand.ToString();
            }


            rkClippy.Close();
            hkcu.Close();

            return commands;
        }

        private static RegistryKey GetRegistryKey(RegistryKey parentKey, string subKeyPath)
        {
            List<RegistryKey> keys = new List<RegistryKey>();
            keys.Add(parentKey);
            try
            {
                foreach (string keyname in subKeyPath.Split('\\'))
                {
                    keys.Add(keys[keys.Count - 1].CreateSubKey(keyname));
                }
                return keys[keys.Count - 1];
            }
            finally
            {
                for (int i = 1; i < keys.Count - 1; i++)
                {
                    keys[i].Close();
                }
            }
        }
    }
}
