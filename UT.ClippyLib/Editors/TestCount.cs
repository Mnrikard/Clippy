using NUnit.Framework;
using System;
using ClippyLib;
using ClippyLib.Editors;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestCount
	{
		[Test]
		public void CanCountLines ()
		{
			string linecounter = @"





a
b
c

";
			IClipEditor cnt = new Count();
			string actualResponse=null;
			cnt.EditorResponse += (sender, e) => {actualResponse = e.ResponseString;};
			EditorTester.TestEditor(cnt, linecounter, "lines");

			Assert.AreEqual("11 lines", actualResponse);
			Assert.AreEqual(linecounter, cnt.SourceData);
		}

		[Test]
		public void CanCountChars()
		{
			string charcounter = "abcdefg";
			IClipEditor cnt = new Count();
			string actualResponse=null;
			cnt.EditorResponse += (sender, e) => {actualResponse = e.ResponseString;};
			EditorTester.TestEditor(cnt, charcounter, "chars");

			Assert.AreEqual("7 characters", actualResponse);
			Assert.AreEqual(charcounter, cnt.SourceData);
		}
	}
}

