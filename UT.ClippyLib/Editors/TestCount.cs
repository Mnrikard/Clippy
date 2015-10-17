using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestCount : AEditorTester
	{
		[Test]
		public void CanCountLines ()
		{
			WhenClipboardContains("\r\n\r\n\r\n\r\n\r\n\r\na\r\nb\r\nc\r\n\r\n");
			AndCommandIsRan("count lines");
			ThenClippyShouldRespondWith("11 lines");
			AndSourceDataShouldNotHaveChanged();
		}

		[Test]
		public void CanCountChars()
		{
			WhenClipboardContains("abcdefg");
			AndCommandIsRan("count");
			ThenClippyShouldRespondWith("7 characters");
			AndSourceDataShouldNotHaveChanged();
		}
	}
}

