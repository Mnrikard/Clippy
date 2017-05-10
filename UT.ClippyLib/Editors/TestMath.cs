using NUnit.Framework;
using System;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestMath : AEditorTester
	{
		[Test]
		public void CanPerformMathOnMultiline()
		{
			WhenClipboardContains(" 123\n" +
				"+456\n" +
			                      "====");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain(" 123\n" +
				"+456\n" +
				"====\n" +
			                              "579");
		}

		[Test]
		public void CanPerformMathOnSingleline()
		{
			WhenClipboardContains("456-123=");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("456-123=333");
		}

		
		[Test]
		public void CanPerformMathOnSinglelineWithoutEquals()
		{
			WhenClipboardContains("456-123");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("456-123=333");
		}

		[Test]
		public void CanMathASlightlyMoreComplicatedExpression()
		{
			WhenClipboardContains("3*3-5+10=");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("3*3-5+10=14");
		}

		[Test]
		public void CanMathWithParenthesis()
		{
			WhenClipboardContains("3*(3-5)+10");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("3*(3-5)+10=4");
		}

		[Test]
		public void CanMathWithMultipleParentheses()
		{			
			WhenClipboardContains("3*((3-5)+10)");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("3*((3-5)+10)=24");
		}

		[Test]
		public void CanMathOnLiteral()
		{
			WhenClipboardContains("-3");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("-3=-3");
		}

		[Test]
		public void CanAddNegativeNumber()
		{
			WhenClipboardContains("7+-3");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("7+-3=4");
		}

		[Test]
		public void CanFailToSubtractPositiveNumber()
		{
			WhenClipboardContains("7-+3");
			AndCommandIsRan("math");
			ThenClippyShouldRespondWith("Error: Could not parse 7-+3");
		}

		[Test]
		public void CanSumList()
		{
			WhenClipboardContains(" 1111\n" +
				" 1111\n" +
				" 3333\n" +
				" 4444\n" +
			    " ====");			
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain(" 1111\n" +
				" 1111\n" +
				" 3333\n" +
				" 4444\n" +
				" ====\n" +
				"9999");
		}

		[Test]
		public void CanPower()
		{
			WhenClipboardContains("2^4");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("2^4=16");
		}

		[Test]
		public void CanModulo()
		{
			WhenClipboardContains("4%3");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("4%3=1");
		}

		[Test]
		public void CanDivideWithRemainder()
		{
			WhenClipboardContains("73/%12");
			AndCommandIsRan("math");
			ThenTheClipboardShouldContain("73/%12=6,1");
		}

		[Test]
		public void CanReturnOnlyAnswer()
		{
			WhenClipboardContains("73/%12");
			AndCommandIsRan("math answer");
			ThenTheClipboardShouldContain("6,1");
		}

	}
}

