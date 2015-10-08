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
			get { return GetValue("udfLocation:"); }
			set { SetValue("udfLocation:", value); }
		}

		public override string SnippetsLocation
		{ 
			get { return GetValue("snippetsLocation:"); }
			set { SetValue("snippetsLocation:", value); }
		}

		public override bool ClosesOnExit
		{ 
			get { return GetValue("CloseFunction:") == "close"; }
			set { SetValue("CloseFunction:", value ? "close" : "hide" ); }
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

			return null;
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
				string line;
				while((line = settingsReader.ReadLine()) != null)
				{
					if(line.StartsWith(valueName, StringComparison.CurrentCultureIgnoreCase))
					{
						settingsValues.AppendLine(String.Concat(valueName,value));
					}
					else
					{
						settingsValues.AppendLine(line.Trim());
					}
				}
			}

			File.WriteAllText(_settingsFile, settingsValues.ToString());
		}


	}
}

