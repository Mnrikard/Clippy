using System;
using ClippyLib;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace UT.ClippyLib
{
	public abstract class AEditorTester
	{
		string actual;
		string contents;
		string editorResponse;
		string persistentEditorResponse;

		protected void WhenClipboardContains(string contents)
		{
			this.contents = contents;
		}

		protected void AndCommandIsRan(string editorWithCommands)
		{
			EditorManager manager = new EditorManager();
			string[] args = manager.GetArgumentsFromString(editorWithCommands);
			IClipEditor editor = manager.GetClipEditor(args[0]);
			manager.ClipEditor.EditorResponse += (a,b) => {editorResponse = b.ResponseString;};
			manager.ClipEditor.PersistentEditorResponse += (a,b) => {persistentEditorResponse = b.ResponseString;};

			string[] realArgs = new string[args.Length-1];
			for(int i=0;i<realArgs.Length;i++)
			{
				realArgs[i] = args[i+1];
			}

			actual = EditorTester.TestEditor(editor, this.contents, realArgs);
		}

		protected void ThenTheClipboardShouldContain(string expected)
		{
			Assert.AreEqual(expected, this.actual);
		}

		protected void ThenTheClipboardShouldContainSubstring(string substringExpected)
		{
			Assert.IsTrue(this.actual.Contains(substringExpected));
		}

		protected void ThenClipboardShouldMatchRegex(string pattern)
		{
			Assert.IsTrue(Regex.IsMatch(this.actual, pattern, RegexOptions.IgnoreCase));
		}

		protected void ThenClippyShouldRespondWith(string expectedResponse)
		{
			Assert.AreEqual(expectedResponse, this.editorResponse);
		}

		protected void ThenClippyShouldRespondAndStayOpenWithMessage(string expectedResponse)
		{
			Assert.AreEqual(expectedResponse, this.persistentEditorResponse);
		}

	}
}

