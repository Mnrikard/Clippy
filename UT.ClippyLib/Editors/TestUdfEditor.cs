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

		[Test]
		public void CanExecuteUdfWithParameters()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("connstring siervier diatiabiase");
			ThenTheClipboardShouldContain("Driver={SQL Server};Server=siervier;Database=diatiabiase;IntegratedSecurity=true;");
		}

		[Test]
		public void CanExecuteUDFWithParametersNotSuppliedInitially()
		{
			RegardlessOfClipboardContent();
			WhenCommandIsRan("connstring");
			ThenClippyShouldRespondAndStayOpenWithMessage("Server:");
		}
	}
}

