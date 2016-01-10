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
		private string snippetsLocation;

		[SetUp]
		public void InitializeTests()
		{
			rcPath = Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				".clippyrc");

			udfPath = Path.Combine(Environment.CurrentDirectory,"udf.xml");
			recentLoc = Path.Combine(Environment.CurrentDirectory,"clippyRecentCommands");
			snippetsLocation = Path.Combine(Environment.CurrentDirectory,"snippets.xml");

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
  <command key=""connstring"">
    <description>creates a connection string</description>
    <function><![CDATA[rep [\d\D]+ ""Driver={SQL Server};Server=%0%;Database=%1%;IntegratedSecurity=true;""]]></function>
    <parameter name=""Server"" default=""MyServer"" required=""True"" parmdesc="""" sequence=""1"" />
    <parameter name=""Database"" default=""MyDatabase"" required=""False"" parmdesc=""The database"" sequence=""2"" />
  </command>
</commands>");

			File.WriteAllText(snippetsLocation, @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Snippets>
  <Snippet Name=""random"">
    <Description>randomized for testing</Description>
    <Content>
      <![CDATA[random string from snippet]]>      
    </Content>
  </Snippet>
</Snippets>");
			
			File.WriteAllText(recentLoc, "rep a b\n" +
				"fakeCommand\n" +
				"grep c\n");

			File.WriteAllText(rcPath, String.Concat("udfLocation:",udfPath,"\n" ,
			                                        "snippetsLocation:",snippetsLocation,"\n" ,
			                                        "recentCommandsLocation:",recentLoc,"\n" ,
			                                         "CloseFunction:close\n" +
			                                         "tabString:{TAB}"));
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
			if(File.Exists(snippetsLocation))
				File.Delete(snippetsLocation);

		}

	}
}

