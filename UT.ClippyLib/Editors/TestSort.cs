using System;
using NUnit.Framework;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestSort : AEditorTester
	{
		[Test]
		public void CanSort()
		{
			WhenClipboardContains("7,6,275,1000,10");
			AndCommandIsRan("sort asc ,");
			ThenTheClipboardShouldContain("6,7,10,275,1000");
		}

		[Test]
		public void CanSortDescending()
		{
			WhenClipboardContains("7,6,275,1000,10");
			AndCommandIsRan("sort desc ,");
			ThenTheClipboardShouldContain("1000,275,10,7,6");
		}
		
		[Test]
		public void CanSortWithDefaultSplit()
		{
			WhenClipboardContains("7\n6\n275\n1000\n10");
			AndCommandIsRan("sort asc");
			ThenTheClipboardShouldContain("6\n7\n10\n275\n1000");
		}

		[Test]
		public void CanSortDescendingWithDefaultSplit()
		{
			WhenClipboardContains("7\n6\n275\n1000\n10");
			AndCommandIsRan("sort desc");
			ThenTheClipboardShouldContain("1000\n275\n10\n7\n6");
		}

		[Test]
		public void CanSortWithAllDefaults()
		{
			WhenClipboardContains("7\n6\n275\n1000\n10");
			AndCommandIsRan("sort");
			ThenTheClipboardShouldContain("6\n7\n10\n275\n1000");
		}
	}
}

