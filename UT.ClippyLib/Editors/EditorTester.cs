using System;
using ClippyLib;

namespace UT.ClippyLib
{
	public class EditorTester
	{
		public static string TestEditor(IClipEditor editor, string input)
		{
			return TestEditor(editor, input, new string[0]);
		}
		public static string TestEditor(IClipEditor editor, string input, string command)
		{
			return TestEditor(editor, input, new []{command});
		}

		public static string TestEditor(IClipEditor editor, string input, params string[] commands)
		{
			editor.SourceData = input;
			editor.DefineParameters();
			for(int i=0;i<commands.Length;i++)
			{
				editor.SetNextParameter(commands[i]);
			}
			editor.Edit();
			return editor.SourceData;
		}
	}
}

