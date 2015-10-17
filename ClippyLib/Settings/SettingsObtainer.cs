using System;
using System.IO;
using Microsoft.Win32;

namespace ClippyLib.Settings
{
	public abstract class SettingsObtainer
	{
		public abstract string UdfLocation { get;set; }
		public abstract string SnippetsLocation { get;set; }
		public abstract string RecentCommandsLocation { get;set; }
		public abstract bool ClosesOnExit { get;set; }


		protected string snippetsKey = "snippetsLocation";
		protected string udfKey = "udfLocation";
		protected string recentCommandsKey = "recentCommandsLocation";
		protected string closeOptionKey = "CloseFunction";

		public static SettingsObtainer CreateInstance()
		{
			string currentAppDir =  Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			if(File.Exists(Path.Combine(currentAppDir, ".clippyrc")))
			{
				return new SettingsFromFile();
			}

			if(Registry.CurrentUser != null && Registry.CurrentUser.OpenSubKey("Software\\Rikard\\Clippy", false) != null)
			{
				return new SettingsFromRegistry();
			}

			return null;			 
		}
	}
}

