using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestEncode : AEditorTester
	{
		[Test]
		public void CanEncodeUrl()
		{
			WhenClipboardContains("this&that! $10.00*%^");
			AndCommandIsRan("encode url");
			ThenTheClipboardShouldContain("this%26that%21%20%2410.00%2A%25%5E");
		}

		[Test]
		public void CanDecodeUrl()
		{
			WhenClipboardContains("this%26that%21%20%2410.00%2A%25%5E");
			AndCommandIsRan("encode url reverse");
			ThenTheClipboardShouldContain("this&that! $10.00*%^");
		}

		[Test]
		public void CanEncodeBase64()
		{
			WhenClipboardContains("this&that! $10.00*%^");
			AndCommandIsRan("encode base64");
			ThenTheClipboardShouldContain("dGhpcyZ0aGF0ISAkMTAuMDAqJV4=");
		}

		[Test]
		public void CanDecodeBase64()
		{
			WhenClipboardContains("dGhpcyZ0aGF0ISAkMTAuMDAqJV4=");
			AndCommandIsRan("encode base64 reverse");
			ThenTheClipboardShouldContain("this&that! $10.00*%^");
		}

		[Test]
		public void CanEncodeHtml()
		{
			WhenClipboardContains("<root>test</root>");
			AndCommandIsRan("encode html");
			ThenTheClipboardShouldContain("&lt;root&gt;test&lt;/root&gt;");
		}

		[Test]
		public void CanEncodeXml()
		{
			WhenClipboardContains("<root>test</root>");
			AndCommandIsRan("encode xml");
			ThenTheClipboardShouldContain("&lt;root&gt;test&lt;/root&gt;");
		}

		[Test]
		public void CanDecodeHtml()
		{
			WhenClipboardContains("&lt;root&gt;test&lt;/root&gt;");
			AndCommandIsRan("encode html reverse");
			ThenTheClipboardShouldContain("<root>test</root>");
		}

		[Test]
		public void CanDecodeXml()
		{
			WhenClipboardContains("&lt;root&gt;test&lt;/root&gt;");
			AndCommandIsRan("encode xml reverse");
			ThenTheClipboardShouldContain("<root>test</root>");
		}
	}
}

