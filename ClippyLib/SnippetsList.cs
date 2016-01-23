using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ClippyLib;
using ClippyLib.Settings;

namespace ClippyLib
{
	public class SnippetsList : List<Snippet>
	{
		SettingsObtainer _settings;

		public SnippetsList()
		{
			_settings = SettingsObtainer.CreateInstance();
			InitFunctions();
		}

		private void InitFunctions()
		{
			GetListOfSnippetsFromFile();
		}

		private void GetListOfSnippetsFromFile()
		{
			if(!File.Exists(_settings.SnippetsLocation))
				return;

			XDocument snippetsDoc = XDocument.Load(_settings.SnippetsLocation);

			XElement root = snippetsDoc.Root;
			if(root != null)
			{
				foreach(var snippet in root.Elements("Snippet"))
				{
					this.Add(new Snippet(snippet));
				}
			}
		}

		public void Save()
		{

			XElement root = new XElement("Snippets");

			foreach(Snippet snip in this.OrderBy(s => s.Name))
			{
				root.Add(new XElement("Snippet",new XAttribute("Name",snip.Name),
					new XElement("Description",snip.Description),
					new XElement("Content",snip.Content)
				));
			}
			XDocument saveFile = new XDocument(root);

			saveFile.Save(_settings.SnippetsLocation);
		}

		public Snippet GetSnippet(string name)
		{
			return this.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
		}

		public bool SnippetExists(string name)
		{
			return GetSnippet(name) != null;
		}

		public void DescribeSnippet(StringBuilder output)
		{
			foreach (Snippet snip in this)
			{
				output.AppendLine(String.Concat(
					snip.Name,
					"  -  ",
					string.IsNullOrEmpty(snip.Description) ? "No description available" : snip.Description));
			}
		}

		public List<string> GetSnippets()
		{
			return (from snip in this
			        select snip.Name).ToList();
		}

	}
}

