using System;
using ClippyLib.Settings;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text;

namespace ClippyLib
{
	public class UserFunctionsList : List<UserFunction>
	{
		SettingsObtainer _settings;

		public UserFunctionsList ()
		{
			_settings = SettingsObtainer.CreateInstance();
			InitFunctions();
		}

		private void InitFunctions()
		{
			GetListOfUserFunctionsFromFile();
		}

		private void GetListOfUserFunctionsFromFile()
		{
			if(!File.Exists(_settings.UdfLocation))
				return;

			XDocument udfDoc = XDocument.Load(_settings.UdfLocation);

			XElement root = udfDoc.Root;
			if(root != null)
			{
				foreach(var command in root.Elements("command"))
				{
					this.Add(new UserFunction(command));
				}
			}
		}

		public UserFunction GetUserFunction(params string[] key)
		{
			if(key.Length < 1)
				throw new ArgumentNullException("key","No argument was passed to find function");

			return this.FirstOrDefault(f => f.Name.Equals(key[0], StringComparison.CurrentCultureIgnoreCase));
		}

		public bool CommandExists(params string[] key)
		{
			return GetUserFunction(key) != null;
		}

		public void DescribeFunctions(EditorDescription output)
		{
			foreach (UserFunction udf in this.OrderBy(u => u.Name))
			{
				output.Append(EditorDescription.Category.Emphasized, udf.Name);
				output.AppendLine(EditorDescription.Category.PlainText, 
					String.Concat(
						"  -  ",
						string.IsNullOrEmpty(udf.Description) ? "No description available" : udf.Description
					)
				);
			}
		}

		public List<string> GetFunctions()
		{
			return (from func in this
			        select func.Name).ToList();
		}

		
		public void Save()
		{
			XElement root = new XElement("commands");
			XDocument saveFile = new XDocument(root);

			foreach(UserFunction uf in this.OrderBy(u => u.Name))
			{
				XElement command = new XElement("command",new XAttribute("key",uf.Name),
					new XElement("description",uf.Description)
				);

				foreach(string subfunc in uf.SubFunctions)
				{
					command.Add(new XElement("function",subfunc));
				}

				foreach(UserFunction.UserParameter param in uf.Parameters)
				{
					command.Add(new XElement("parameter",
						new XAttribute("name", param.Name),
						new XAttribute("default", param.DefaultValue),
						new XAttribute("parmdesc", param.Description),
						new XAttribute("sequence", param.Sequence.ToString()),
						new XAttribute("required", param.Required.ToString())
					));
				}

				root.Add(command);
			}

			saveFile.Save(_settings.UdfLocation);
		}

	}
}

