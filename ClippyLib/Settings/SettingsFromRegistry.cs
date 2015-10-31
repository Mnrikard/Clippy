using System;
using Microsoft.Win32;

namespace ClippyLib.Settings
{
	public class SettingsFromRegistry : SettingsObtainer
	{
		RegistryKey hkcu = Registry.CurrentUser;
		RegistryKey ClippyRegistryKey;

		public SettingsFromRegistry()
		{
			ClippyRegistryKey = hkcu.OpenSubKey("Software\\Rikard\\Clippy", true);
		}

		public override string RecentCommandsLocation 
		{
			get { return GetRegistryValue(recentCommandsKey); }
			set { SetRegistryValue(recentCommandsKey, value); }
		}

		public override string UdfLocation 
		{
			get { return GetRegistryValue(udfKey); }
			set { SetRegistryValue(udfKey, value); }
		}

		public override string SnippetsLocation
		{
			get { return GetRegistryValue(snippetsKey); }
			set { SetRegistryValue(snippetsKey, value); }
		}

		public override bool ClosesOnExit
		{
			get { return GetRegistryValue(closeOptionKey).Equals("close", StringComparison.CurrentCultureIgnoreCase); }
			set { SetRegistryValue(closeOptionKey, value ? "close" : "hide"); }
		}

		public override string TabString
		{
			get { return UnescapeTabString(GetRegistryValue(tabStringKey)); }
			set { SetRegistryValue(tabStringKey, EscapeTabString(value)); }
		}

		private string GetRegistryValue(string key)
		{
			return ClippyRegistryKey == null ? string.Empty :  ClippyRegistryKey.GetValue(key).ToString();
		}

		private void SetRegistryValue(string key, string value)
		{
			if(ClippyRegistryKey != null)
			{
				ClippyRegistryKey.SetValue(key, value, RegistryValueKind.String);
			}
		}

	}
}

