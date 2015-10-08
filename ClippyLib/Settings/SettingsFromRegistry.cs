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

		public override string UdfLocation 
		{
			get 
			{
				return ClippyRegistryKey == null ? string.Empty :  ClippyRegistryKey.GetValue("udfLocation").ToString();
			}
			set
			{
				if(ClippyRegistryKey != null)
				{
					ClippyRegistryKey.SetValue("udfLocation", value, RegistryValueKind.String);
				}
			}
		}

		public override string SnippetsLocation
		{
			get 
			{
				return ClippyRegistryKey == null ? string.Empty :  ClippyRegistryKey.GetValue("snippetsLocation").ToString();
			}
			set
			{
				if(ClippyRegistryKey != null)
				{
					ClippyRegistryKey.SetValue("snippetsLocation", value, RegistryValueKind.String);
				}
			}
		}

		public override bool ClosesOnExit
		{
			get
			{
				return ClippyRegistryKey == null ? true :  ClippyRegistryKey.GetValue("CloseFunction").ToString().Equals("close",StringComparison.CurrentCultureIgnoreCase);
			}
			set
			{
				ClippyRegistryKey.SetValue("CloseFunction", value ? "close" : "hide", RegistryValueKind.String);
			}

		}

	}
}

