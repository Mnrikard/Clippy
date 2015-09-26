using NUnit.Framework;
using System;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestXmlEdit : AEditorTester
	{
		[Test]
		public void CanPrettyPrintXml()
		{
			System.Configuration.ConfigurationManager.AppSettings.Add("tabString","\t");

			WhenClipboardContains("<root><child><grandchild /></child></root>");
			AndCommandIsRan("xml");
			ThenTheClipboardShouldContain("<root>\n\t<child>\n\t\t<grandchild />\n\t</child>\n</root>");
		}
	}
}

