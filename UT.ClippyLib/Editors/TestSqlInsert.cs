using NUnit.Framework;
using System;
using ClippyLib.Editors;
using System.Text;

namespace UT.ClippyLib.Editors
{
	[TestFixture]
	public class TestSqlInsert : AEditorTester
	{
		string dtnow = DateTime.Now.ToString();

		private string SqlOutputOver1000Rows()
		{
			StringBuilder databaseText = new StringBuilder();
			databaseText.Append("Name\tId\tDate\n");

			for(int i=0;i<2010;i++)
			{
				databaseText.Append(String.Concat("Test\t", i.ToString(), "\t", dtnow, "\n"));
			}
			databaseText.Append(String.Concat("Test\tNaN\t", dtnow));

			return databaseText.ToString();
		}

		[Test]
		public void CanCreateLongInsertStatement ()
		{
			WhenClipboardContains(SqlOutputOver1000Rows());
			AndCommandIsRan("insert tablex");
			ThenTheClipboardShouldContainSubstring(",('Test', 999, '"+dtnow+"')\n" +
			                                       "insert into [tablex] (Name, Id, Date)\nvalues\n " +
			                                       "('Test', 1000, '"+dtnow+"')");		
		}

		[Test]
		public void CanCreateInsertStatement()
		{
			WhenClipboardContains("Name\tId\tDate\n" +
				"Test\t1\t2015-09-22\n" +
				"Test\t2\tNULL");
			AndCommandIsRan("insert tablex");
			ThenTheClipboardShouldContain("insert into [tablex] (Name, Id, Date)\n" +
				"values\n" +
				" ('Test', 1, '2015-09-22')\n" +
				",('Test', 2, NULL)\n");
		}

		[Test]
		public void CanTreatLeadingZerosAsString()
		{
			WhenClipboardContains("Name\tId\tDate\n" +
			                      "Test\t01\t2015-09-22\n" +
			                      "Test\t02\tNULL");
			AndCommandIsRan("insert tablex");
			ThenTheClipboardShouldContain("insert into [tablex] (Name, Id, Date)\n" +
			                              "values\n" +
			                              " ('Test', '01', '2015-09-22')\n" +
			                              ",('Test', '02', NULL)\n");
		}
	}
}

