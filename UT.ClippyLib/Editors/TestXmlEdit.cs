using NUnit.Framework;
using System;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestXmlEdit : AEditorTester
	{
		[Test]
		public void CanPrettyPrintXml()
		{
			WhenClipboardContains("<root><child><grandchild /></child></root>");
			AndCommandIsRan("xml");
			ThenTheClipboardShouldContain("<root>\n\t<child>\n\t\t<grandchild />\n\t</child>\n</root>");
		}

		
		[Test]
		public void CanPrettyPrintPartialXml()
		{
			WhenClipboardContains("<root><child><grandchild />");
			AndCommandIsRan("xml");
			ThenTheClipboardShouldContain("<root>\n\t<child>\n\t\t<grandchild />");
		}

		[Test]
		public void CanPrettyPrintSortofXmlWithWeirdStuff()
		{
			WhenClipboardContains("<root><child><grandchild /><!-- some string\n" +
				"with newline chars in a comment\n" +
				"this should be preserved -->\n" +
				"<textNode>Even though whitespace\n" +
				"is not really defined in xml\n" +
				"these newlines should be preserved</textNode>\n\n\n" +
				"<![CDATA[ likewise,\n" +
				"cdata sections should be preserved whitespace]]></notTheNodeYouOpened>");
			AndCommandIsRan("xml");
			ThenTheClipboardShouldContain("<root>\n" +
			                              "\t<child>\n" +
			                              "\t\t<grandchild />\n" +
			                              "\t\t<!-- some string\n" +
			                              "\t\twith newline chars in a comment\n" +
			                              "\t\tthis should be preserved -->\n" +
			                              "\t\t<textNode>Even though whitespace\n" +
			                              "\t\t\tis not really defined in xml\n" +
			                              "\t\t\tthese newlines should be preserved\n" +
			                              "\t\t</textNode>\n" +
			                              "\t\t<![CDATA[ likewise,\n" +
			                              "\t\tcdata sections should be preserved whitespace]]>\n" +
			                              "\t\t</notTheNodeYouOpened>");

		}
	}
}

