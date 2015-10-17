using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ClippyLib.RecentCommands
{
	public class FileStore : Store
	{
		private readonly string _fileStoreLocation;

		public FileStore(string fileStoreLocation)
		{
			_fileStoreLocation = fileStoreLocation;
		}

		public override void SaveThisCommand(string editorName, string parms)
		{
			string currentValue = String.Concat(editorName.Trim(), " ", parms);
			StringBuilder currentList = new StringBuilder();
			currentList.AppendLine(currentValue);

			string[] recentCommands = GetRecentCommandList();
			for(int i=0;i<recentCommands.Length - 1;i++)
			{
				if(recentCommands[i].Trim().Equals(currentValue.Trim(), StringComparison.CurrentCultureIgnoreCase))
					continue;

				currentList.AppendLine(recentCommands[i].Trim());
			}

			File.WriteAllText(_fileStoreLocation, currentList.ToString());
		}

		public override string[] GetRecentCommandList()
		{
			List<string> output = new List<string>();

			using(FileStream rcmds = new FileStream(_fileStoreLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using(StreamReader sr = new StreamReader(rcmds))
				{
					string line;
					while((line = sr.ReadLine()) != null)
					{
						if(line.Trim().Length > 0)
							output.Add(line.Trim());
					}
				}
			}

			return output.ToArray();
		}
	}
}

