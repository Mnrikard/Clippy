using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestCapitalize : AEditorTester
	{
		[Test]
		public void CanCapitalizeByDefault()
		{
			WhenClipboardContains("abcd");
			AndCommandIsRan("cap");
			ThenTheClipboardShouldContain("ABCD");
		}
		
		[Test]
		public void CanCapitalizeExplicitly()
		{
			WhenClipboardContains("abcd");
			AndCommandIsRan("cap u");
			ThenTheClipboardShouldContain("ABCD");
		}

		[Test]
		public void CanLowerCase()
		{
			WhenClipboardContains("ABCD");
			AndCommandIsRan("cap l");
			ThenTheClipboardShouldContain("abcd");
		}

		[Test]
		public void CanMixCase()
		{
			WhenClipboardContains("the quick brown fOX jUMPED ovER the lAzY dOg");
			AndCommandIsRan("cap m");
			ThenTheClipboardShouldContain("The Quick Brown Fox Jumped Over The Lazy Dog");
		}

	}
}

