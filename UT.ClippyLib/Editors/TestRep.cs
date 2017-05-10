using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestRep : AEditorTester
	{
		[Test]
		public void CanReplaceRegex()
		{
			WhenClipboardContains("r3eplac3e a1l1l 1234 n4umb3ers wi1th 3empty 5str1in9g");
			AndCommandIsRan(@"rep ""\d"" """"");
			ThenTheClipboardShouldContain("replace all  numbers with empty string");
		}

		[Test]
		public void CanReplaceSuperRegex()
		{
			WhenClipboardContains("replace each word with it's capitalized second letter");
			AndCommandIsRan("rep /(\\w)(\\w)([\\w']+)/i $1\\u$2$3");
			ThenTheClipboardShouldContain("rEplace eAch wOrd wIth iT's cApitalized sEcond lEtter");
		}
	}
}

