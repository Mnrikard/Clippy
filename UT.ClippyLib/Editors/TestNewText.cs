using NUnit.Framework;
using System;
using ClippyLib.Editors;
using System.Text.RegularExpressions;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestNewText
	{
		[Test]
		public void CanGetNewGuid()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "id");
			Assert.IsTrue(Regex.IsMatch(actual,@"[a-f\d]{8}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{12}", RegexOptions.IgnoreCase));
		}

		[Test]
		public void CanGetOnlyDate()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "date");
			Assert.IsTrue(Regex.IsMatch(actual,@"\d\d\d\d-\d\d-\d\d", RegexOptions.IgnoreCase));
		}

		[Test]
		public void CanGet24HourFormatTime()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "Time");
			Assert.IsTrue(Regex.IsMatch(actual,@"\d\d:\d\d:\d\d", RegexOptions.IgnoreCase));
		}

		[Test]
		public void CanGetAMPMFormatTime()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "time");
			Assert.IsTrue(Regex.IsMatch(actual,@"\d{1,2}:\d\d:\d\d [ap]m", RegexOptions.IgnoreCase));
		}

		[Test]
		public void CanGet24HourFormatDateTime()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "dT");
			Assert.IsTrue(Regex.IsMatch(actual,@"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d", RegexOptions.IgnoreCase));
		}

		[Test]
		public void CanGetAMPMFormatDateTime()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "dt");
			Assert.IsTrue(Regex.IsMatch(actual,@"\d\d\d\d-\d{1,2}-\d{1,2} \d{1,2}:\d\d:\d\d [ap]m", RegexOptions.IgnoreCase));
		}

		[Ignore("Slashdot moved their rss maybe, I'll find out what happened.")]
		[Test]
		public void CanGetRandomSlashDot()
		{
			string actual = EditorTester.TestEditor(new NewText(), "", "/.");
			Assert.IsTrue(actual.Length > 0);
		}
	}
}

