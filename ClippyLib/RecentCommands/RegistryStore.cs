using System;
using Microsoft.Win32;
using System.Collections.Generic;

namespace ClippyLib.RecentCommands
{
	public class RegistryStore : Store
	{
		public override void SaveThisCommand(string editorName, string parms)
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

		public override string[] GetRecentCommandList()
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

