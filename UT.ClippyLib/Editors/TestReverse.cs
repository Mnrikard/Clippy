using System;
using NUnit.Framework;

namespace UT.ClippyLib.Editors
{	
	[TestFixture]
	public class TestReverse : AEditorTester
	{
		[Test]
		public void CanReverseText()
		{
			WhenClipboardContains("apple\nbanana\ncrabapple\ndonut");
			AndCommandIsRan("reverse");
			ThenTheClipboardShouldContain("donut\ncrabapple\nbanana\napple");
		}

		[Test]
		public void CanReverseTextSplitByComma()
		{
			WhenClipboardContains("apple,banana,crabapple,donut");
			AndCommandIsRan("reverse ,");
			ThenTheClipboardShouldContain("donut,crabapple,banana,apple");
		}
	}
}

