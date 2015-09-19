using NUnit.Framework;
using System;
using ClippyLib.Editors;
using ClippyLib;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestCapitalize
	{
		[Test]
		public void CanCreateObject ()
		{
			Capitalize cap = new Capitalize();
			Assert.IsInstanceOf<AClipEditor>(cap);
		}

		[Test]
		public void CanCapitalize()
		{
			IClipEditor cap = new Capitalize();
			string actual = EditorTester.TestEditor(cap,"abcd","u");
			Assert.AreEqual("ABCD",actual);
		}

		[Test]
		public void CanLowerCase()
		{
			IClipEditor cap = new Capitalize();
			string actual = EditorTester.TestEditor(cap,"ABCD","l");
			Assert.AreEqual("abcd", actual);
		}

		[Test]
		public void CanMixCase()
		{
			IClipEditor cap = new Capitalize();
			string actual = EditorTester.TestEditor(cap,"the quick brown fOX jUMPED ovER the lAzY dOg","m");
			Assert.AreEqual("The Quick Brown Fox Jumped Over The Lazy Dog", actual);
		}

		[Test]
		public void CanMixCaseAsDefault()
		{
			IClipEditor cap = new Capitalize();
			string actual = EditorTester.TestEditor(cap,"the quick brown fOX jUMPED ovER the lAzY dOg");
			Assert.AreEqual("The Quick Brown Fox Jumped Over The Lazy Dog", actual);
		}
	}
}

