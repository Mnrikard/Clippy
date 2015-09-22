using NUnit.Framework;
using System;
using ClippyLib.Editors;
using System.Text;

namespace UT.ClippyLib
{
	[TestFixture]
	public class TestSqlInsert
	{
		[Test]
		public void CanCreateLongInsertStatement ()
		{
			StringBuilder databaseText = new StringBuilder();
			databaseText.Append("Name\tId\tDate\n");
			string dtnow = DateTime.Now.ToString();

			for(int i=0;i<2010;i++)
			{
				databaseText.Append(String.Concat("Test\t", i.ToString(), "\t", dtnow, "\n"));
			}
			databaseText.Append(String.Concat("Test\tNaN\t", dtnow));

			var actual = EditorTester.TestEditor(new SqlInsert(), databaseText.ToString(),"tablex","\t");
			Assert.IsTrue(actual.Contains(",('Test', 999, '"+dtnow+"')\n" +
				                          "insert into [tablex] (Name, Id, Date)\nvalues\n " +
			                              "('Test', 1000, '"+dtnow+"')"));
		}

		[Test]
		public void CanCreateInsertStatement ()
		{
			StringBuilder databaseText = new StringBuilder();
			databaseText.Append("Name\tId\tDate\n");
			databaseText.Append(String.Concat("Test\t1\t2015-09-22\n"));
			databaseText.Append(String.Concat("Test\t2\tNULL"));

			var actual = EditorTester.TestEditor(new SqlInsert(), databaseText.ToString(),"tablex","\t");
			string expected = "insert into [tablex] (Name, Id, Date)\n" +
				              "values\n" +
				              " ('Test', 1, '2015-09-22')\n" +
					          ",('Test', 2, NULL)\n";
			Assert.AreEqual(expected, actual);
		}
	}
}

