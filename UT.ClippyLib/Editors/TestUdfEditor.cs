using NUnit.Framework;
using System;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestUdfEditor : AEditorTester
	{
		[Test]
		public void CanExecuteSimpleUdf()
		{
			WhenClipboardContains("1 2 3 4 5 10 23 57 99");
			AndUdfCommandIsRan("NumList");
			ThenTheClipboardShouldContain("(1,2,3,4,5,10,23,57,99)");
		}
	}
}

