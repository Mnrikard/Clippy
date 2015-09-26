using NUnit.Framework;
using System;
using ClippyLib.Editors;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestDedupe
	{
		[Test]
		public void CanDedupeWithComma()
		{
			string duped = "abcd,abcd,abcd,abcd,defg,deFG";

			string actual = EditorTester.TestEditor(new Dedupe(), duped, ",");
			string expected = "abcd,defg,deFG";
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CanDedupeWithNewLine()
		{
			string duped = "abcd\nabcd\nABCD";
			string actual = EditorTester.TestEditor(new Dedupe(), duped, "\n");
			string expected = "abcd\nABCD";
			Assert.AreEqual(expected, actual);
		}
	}
}

