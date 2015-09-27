using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestDedupe : AEditorTester
	{
		[Test]
		public void CanDedupeWithComma()
		{
			WhenClipboardContains("abcd,abcd,abcd,abcd,defg,deFG");
			AndCommandIsRan("dedup ,");
			ThenTheClipboardShouldContain("abcd,defg,deFG");
		}

		[Test]
		public void CanDedupeWithNewLine()
		{
			WhenClipboardContains("abcd\nabcd\nABCD");
			AndCommandIsRan("dedup");
			ThenTheClipboardShouldContain("abcd\nABCD");
		}
	}
}

