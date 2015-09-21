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
	}
}

