using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ClippyLib.Settings
{
	public class SettingsFromFile : SettingsObtainer
	{
		private string _settingsFile =  Path.Combine(
			Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
			".clippyrc");

		public override string UdfLocation 
		{ 
			get { return GetValue(udfKey); }
			set { SetValue(udfKey, value); }
		}

		public override string SnippetsLocation
		{ 
			get { return GetValue(snippetsKey); }
			set { SetValue(snippetsKey, value); }
		}

		public override string RecentCommandsLocation
		{
			get { return GetValue(recentCommandsKey); }
			set { SetValue(recentCommandsKey, value); }
		}

		public override bool ClosesOnExit
		{ 
			get { return GetValue(closeOptionKey).Trim().Equals("close", StringComparison.CurrentCultureIgnoreCase); }
			set { SetValue(closeOptionKey, value ? "close" : "hide" ); }
		}

		public override string TabString
		{
			get { return UnescapeTabString(GetValue(tabStringKey)); }
			set { SetValue(tabStringKey, EscapeTabString(value)); }
		}

		private string GetValue(string valueName)
		{
			valueName = valueName.Trim();
			if(!valueName.EndsWith(":"))
			{
				valueName += ":";
			}

			string[] settingsValues;
			using(StreamReader settingsReader = new StreamReader(_settingsFile))
			{
				settingsValues = settingsReader.ReadToEnd().Split('\n');
			}

			foreach(string valueLine in settingsValues)
			{
				if(valueLine.StartsWith(valueName, StringComparison.CurrentCultureIgnoreCase))
				{
					return valueLine.Substring(valueName.Length).Trim();
				}
			}

			return string.Empty;
		}

		private void SetValue(string valueName, string value)
		{
			
			valueName = valueName.Trim();
			if(!valueName.EndsWith(":"))
			{
				valueName += ":";
			}

			StringBuilder settingsValues = new StringBuilder();
			using(StreamReader settingsReader = new StreamReader(_settingsFile))
			{
				bool valueSet = false;
				string line;
				while((line = settingsReader.ReadLine()) != null)
				{
					if(line.StartsWith(valueName, StringComparison.CurrentCultureIgnoreCase))
					{
						settingsValues.AppendLine(String.Concat(valueName,value));
						valueSet = true;
					}
					else
					{
						settingsValues.AppendLine(line.Trim());
					}
				}

				if(!valueSet)
					settingsValues.AppendLine(String.Concat(valueName,value));
			}

			File.WriteAllText(_settingsFile, settingsValues.ToString());
		}


	}
}

