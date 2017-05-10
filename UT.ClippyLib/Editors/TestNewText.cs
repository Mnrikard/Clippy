using NUnit.Framework;
using System;
using ClippyLib.Editors;
using System.Text.RegularExpressions;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestNewText : AEditorTester
	{
		[Test]
		public void CanGetNewGuid()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("newtext id");
			ThenClipboardShouldMatchRegex(@"[a-f\d]{8}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{12}");
		}

		[Test]
		public void CanGetOnlyDate()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("newtext date");
			ThenClipboardShouldMatchRegex(@"\d\d\d\d-\d\d-\d\d");
		}

		[Test]
		public void CanGet24HourFormatTime()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("newtext Time");
			ThenClipboardShouldMatchRegex(@"\d\d:\d\d:\d\d");
		}

		[Test]
		public void CanGetAMPMFormatTime()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("newtext time");
			ThenClipboardShouldMatchRegex(@"\d{1,2}:\d\d:\d\d [ap]m");
		}

		[Test]
		public void CanGet24HourFormatDateTime()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("newtext dT");
			ThenClipboardShouldMatchRegex(@"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d");
		}

		[Test]
		public void CanGetAMPMFormatDateTime()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("newtext dt");
			ThenClipboardShouldMatchRegex(@"\d\d\d\d-\d{1,2}-\d{1,2} \d{1,2}:\d\d:\d\d [ap]m");
		}
	}
}

