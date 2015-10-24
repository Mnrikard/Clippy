using System;
using System.Xml.Linq;

namespace ClippyLib
{
	public class Snippet
	{
		public string Name{get;set;}
		public string Description {get;set;}
		public string Content {get;set;}

		public Snippet (XElement snipNode)
		{
			Name = snipNode.Attribute("Name").Value;
			Description = snipNode.Element("Description") == null ? string.Empty : snipNode.Element("Description").Value;
			Content = snipNode.Element("Content").Value;
		}

		public Snippet(string name, string description, string content)
		{
			Name = name;
			Description = description;
			Content = content;
		}
	}
}

