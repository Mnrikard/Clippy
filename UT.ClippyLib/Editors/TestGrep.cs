using NUnit.Framework;
using System;
using ClippyLib.Editors;
using ClippyLib;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestGrep
	{
		[Test]
		public void CanGrep()
		{
			string content = "this is a string with tennis";
			EditorTester.AssertEditor("this,is,tennis", new Grep(), content, @"\w*is\b", ",");
		}

		[Test]
		public void CanReportWhenNoMatchFound()
		{
			string content = "a string";
			IClipEditor grep = new Grep();
			string actualResponse = null;
			grep.EditorResponse += (a,b) => {actualResponse = b.ResponseString;};
			EditorTester.TestEditor(grep, content, @"\w*is\b",",");
			Assert.AreEqual("Pattern did not find a match in the string", actualResponse);
			Assert.AreEqual(content, grep.SourceData);
		}
	}
}

