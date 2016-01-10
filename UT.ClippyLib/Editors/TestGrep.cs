using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestGrep : AEditorTester
	{
		
		[Test]
		public void CanDescribeItself()
		{
			Grep g = new Grep();
			Assert.IsTrue(g.LongDescription.ToString().Contains("PatternType - One of [regex|sql|text]"));
		}

		[Test]
		public void CanGrep()
		{
			WhenClipboardContains("this is a string with tennis");
			AndCommandIsRan(@"grep ""\w*is\b"" ,");
			ThenTheClipboardShouldContain("this,is,tennis");
		}

		[Test]
		public void CanGrepWithOptions()
		{
			WhenClipboardContains("first word\n"+
			                      "second word\n"+
			                      "third word");
			AndCommandIsRan("grep /^\\w+/m");
			ThenTheClipboardShouldContain("first\n"+
			                              "second\n"+
			                              "third");
		}

		[Test]
		public void CanGrepWithPlainText()
		{
			WhenClipboardContains("this is some text");
			AndCommandIsRan("grep \"Some Text\" text");
			ThenTheClipboardShouldContain("some text");
		}

		[Test]
		public void CanGrepWithSqlPattern()
		{
			WhenClipboardContains("We sh0uld\nfind each line\nwith a d1git\nbut not this one");
			AndCommandIsRan("grep %[0-9]% , sql");
			ThenTheClipboardShouldContain("We sh0uld,with a d1git");
		}

		[Test]
		public void CanReportWhenNoMatchFound()
		{
			WhenClipboardContains("any random string");
			AndCommandIsRan("grep \"some pattern that won't match\"");
			ThenClippyShouldRespondWith("Pattern did not find a match in the string");
		}
	}
}

