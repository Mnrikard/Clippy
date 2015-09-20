using NUnit.Framework;
using System;
using ClippyLib.Editors;
using ClippyLib;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestColumnAlign
	{
		const string _columns = "Column1\tColumn2\n" +
		                        "abcdefghijklmnopqrstuvwxyz\talphabet\n" +
		                        "12345\tnumbers1-5";
		[Test]
		public void CanCreateInstance()
		{
			ColumnAlign ca = new ColumnAlign();
			Assert.IsInstanceOf<IClipEditor>(ca);
		}

		[Test]
		public void CanAlignColumns()
		{
			string actual = EditorTester.TestEditor(new ColumnAlign(), _columns, "2","\t");
			string expected = "Column1                     Column2\n" +
			                  "abcdefghijklmnopqrstuvwxyz  alphabet\n" +
			                  "12345                       numbers1-5";
			Assert.AreEqual(expected, actual);
		}
	}
}

