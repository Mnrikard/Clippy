using NUnit.Framework;
using System;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestSnippetTextEditor : AEditorTester
	{
		[Test]
		public void CanGetSnippet()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("snippet random");
			ThenTheClipboardShouldContain("random string from snippet");
		}
	}
}

