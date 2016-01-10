using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ClippyLib.Editors;
using ClippyLib;
using NSubstitute;
using NUnit.Framework;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestUserFunctionsList
	{
		[Test]
		public void CanGetUserFunctions()
		{
			UserFunctionsList lst = new UserFunctionsList();
			List<string> actual = lst.GetFunctions();
			Assert.AreEqual(3, actual.Count);
			Assert.AreEqual("NumList",actual[0]);
			Assert.AreEqual("HtmlEncode",actual[1]);
		}

		[Test]
		public void CanGetSpecificUserFunction()
		{
			UserFunctionsList lst = new UserFunctionsList();
			UserFunction actual = lst.GetUserFunction("HtmlEncode");
			Assert.AreEqual("Encodes to html", actual.Description);
		}

		[Test]
		public void CanFailToGetSpecificUserFunction()
		{
			UserFunctionsList lst = new UserFunctionsList();
			UserFunction actual = lst.GetUserFunction("bogus");
			Assert.IsNull(actual);
		}

	}
}

