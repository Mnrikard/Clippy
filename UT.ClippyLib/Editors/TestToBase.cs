using NUnit.Framework;
using System;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestToBase : AEditorTester
	{
		[Test]
		public void CanConvertToBase16()
		{
			WhenClipboardContains("254");
			AndCommandIsRan("tobase 16");
			ThenTheClipboardShouldContain("FE");
		}

		[Test]
		public void CanConvertFromBase16()
		{
			WhenClipboardContains("FE");
			AndCommandIsRan("tobase 16 reverse");
			ThenTheClipboardShouldContain("254");
		}
	}
}

