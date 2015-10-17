using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace UT.ClippyLib
{
	[SetUpFixture]
	public class AllTestSetup
	{
		private string rcPath;
		private string udfPath;
		private string recentLoc;

		[SetUp]
		public void InitializeTests()
		{
			rcPath = Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				".clippyrc");

			udfPath = Path.Combine(Environment.CurrentDirectory,"udf.xml");
			recentLoc = Path.Combine(Environment.CurrentDirectory,"clippyRecentCommands");

			DestroyTestArtifacts();

			File.WriteAllText(udfPath, @"<?xml version=""1.0"" encoding=""utf-8""?>
<commands>
  <command key=""NumList"">
    <description>Lists all numeric values in a string.</description>
    <function>rep ""\D+"" "",""</function>
    <function>rep ""([\d,]{80,90},)"" ""$1\n""</function>
	<function>rep ""[\d\D]+"" ""($0)""</function>
  </command>
  <command key=""HtmlEncode"">
    <description>Encodes to html</description>
    <function>encode html</function>
  </command>
</commands>");

			File.WriteAllText(recentLoc, "rep a b\n" +
				"fakeCommand\n" +
				"grep c\n");

			File.WriteAllText(rcPath, String.Concat("udfLocation:",udfPath,"\n" ,
			                                        "snippetsLocation:null\n" ,
			                                        "recentCommandsLocation:",recentLoc,"\n" ,
			                                         "CloseFunction:close\n"));
		}

		[TearDown]
		public void DestroyTestArtifacts()
		{
			if(File.Exists(udfPath))
				File.Delete(udfPath);
			if(File.Exists(rcPath))
				File.Delete(rcPath);
			if(File.Exists(recentLoc))
				File.Delete(recentLoc);
		}

	}
}

