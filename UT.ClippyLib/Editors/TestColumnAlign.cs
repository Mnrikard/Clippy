using NUnit.Framework;
using System;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestColumnAlign : AEditorTester
	{
		const string ColumnsWithHeaders = "Column1\tColumn2\n" +
		                                  "abcdefghijklmnopqrstuvwxyz\talphabet\n" +
				                          "12345\tnumbers1-5";
		[Test]
		public void CanAlignColumns()
		{
			WhenClipboardContains(ColumnsWithHeaders);
			AndCommandIsRan("columnAlign");
			ThenTheClipboardShouldContain("Column1                     Column2\n" +
			                  "abcdefghijklmnopqrstuvwxyz  alphabet\n" +
			                  "12345                       numbers1-5");
		}

		[Test]
		public void CanAlignColumnsWithExtraSpace()
		{
			WhenClipboardContains(ColumnsWithHeaders);
			AndCommandIsRan("columnAlign 4");
			ThenTheClipboardShouldContain("Column1                       Column2\n" +
			                              "abcdefghijklmnopqrstuvwxyz    alphabet\n" +
			                              "12345                         numbers1-5");
		}
	}
}

