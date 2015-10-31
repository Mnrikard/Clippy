using System;
using System.IO;
using Microsoft.Win32;
using ClippyLib;

namespace ClippyLib.Settings
{
	public abstract class SettingsObtainer
	{
		public abstract string UdfLocation { get;set; }
		public abstract string SnippetsLocation { get;set; }
		public abstract string RecentCommandsLocation { get;set; }
		public abstract string TabString {get;set;}
		public string RawTabString
		{
			get { return EscapeTabString(TabString); }
			set { TabString = UnescapeTabString(value); }
		}

		private bool _closesOnExit = true;
		public virtual bool ClosesOnExit { get{return _closesOnExit;}set{_closesOnExit = value;} }


		protected string snippetsKey = "snippetsLocation";
		protected string udfKey = "udfLocation";
		protected string recentCommandsKey = "recentCommandsLocation";
		protected string closeOptionKey = "CloseFunction";
		protected string tabStringKey = "tabString";

		public static SettingsObtainer CreateInstance()
		{
			string rcfileLocation = Extensions.GetLocalFile(".clippyrc");

			if(File.Exists(rcfileLocation))
			{
				return new SettingsFromFile();
			}

			if(Registry.CurrentUser != null && Registry.CurrentUser.OpenSubKey("Software\\Rikard\\Clippy", false) != null)
			{
				return new SettingsFromRegistry();
			}

			return null;			 
		}

		protected string EscapeTabString(string value)
		{
			return value.Replace("\t","{TAB}").Replace(" ","{SPACE}");
		}

		protected string UnescapeTabString(string value)
		{
			if(value.Length == 0)
				return "\t";

			return value.Replace("{TAB}","\t").Replace("{SPACE}"," ");
		}
	}
}

