using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestEncode
	{
		[Test]
		public void CanEncodeUrl()
		{
			string urlContent = "this&that! $10.00*%^";

			string actual = EditorTester.TestEditor(new Encode(), urlContent, "url");
			string expected = "this%26that%21%20%2410.00%2A%25%5E";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanDecodeUrl()
		{
			string urlContent = "this%26that%21%20%2410.00%2A%25%5E";

			string actual = EditorTester.TestEditor(new Encode(), urlContent, "url","reverse");
			string expected = "this&that! $10.00*%^";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanEncodeBase64()
		{
			string content = "this&that! $10.00*%^";

			string actual = EditorTester.TestEditor(new Encode(), content, "base64");
			string expected = "dGhpcyZ0aGF0ISAkMTAuMDAqJV4=";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanDecodeBase64()
		{
			string content = "dGhpcyZ0aGF0ISAkMTAuMDAqJV4=";

			string actual = EditorTester.TestEditor(new Encode(), content, "base64","reverse");
			string expected = "this&that! $10.00*%^";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanEncodeHtml()
		{
			string content = "<root>test</root>";
			string actual = EditorTester.TestEditor(new Encode(), content, "html");
			string expected = "&lt;root&gt;test&lt;/root&gt;";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanEncodeXml()
		{
			string content = "<root>test</root>";
			string actual = EditorTester.TestEditor(new Encode(), content, "xml");
			string expected = "&lt;root&gt;test&lt;/root&gt;";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanDecodeHtml()
		{
			string content = "&lt;root&gt;test&lt;/root&gt;";
			string actual = EditorTester.TestEditor(new Encode(), content, "html","reverse");
			string expected = "<root>test</root>";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanDecodeXml()
		{
			string content = "&lt;root&gt;test&lt;/root&gt;";
			string actual = EditorTester.TestEditor(new Encode(), content, "xMl","reverse");
			string expected = "<root>test</root>";
			Assert.AreEqual(expected, actual);
		}
	}
}

