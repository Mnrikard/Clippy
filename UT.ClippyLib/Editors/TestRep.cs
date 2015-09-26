using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib
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
			string content = "replace each word with it's capitalized second letter";
			string expected = "rEplace eAch wOrd wIth iT's cApitalized sEcond lEtter";
			EditorTester.AssertEditor(expected, new Rep(), content, "/(\\w)(\\w)([\\w']+)/i", "$0\\u$1$2");
		}
	}
}

