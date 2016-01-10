using System;
using System.Collections.Generic;
using System.Text;

namespace ClippyLib
{
	public class EditorDescription
	{
		private List<DescriptionItem> _text;
		public EditorDescription ()
		{
			_text = new List<DescriptionItem>();
		}

		public void AppendLine()
		{
			AppendLine(String.Empty);
		}

		public void AppendLine(string text)
		{
			AppendLine(Category.PlainText, text);
		}

		public void AppendLine(Category category, string text)
		{
			_text.Add(new DescriptionItem(category, text, true));
		}

		public void Append(string text)
		{
			Append(Category.PlainText, text);
		}

		public void Append(Category category, string text)
		{
			_text.Add(new DescriptionItem(category, text, false));
		}

		public bool Contains(string text)
		{
			return (ToString().IndexOf(text, StringComparison.CurrentCultureIgnoreCase) > -1);
		}

		public override string ToString()
		{
			StringBuilder output = new StringBuilder();
			foreach(DescriptionItem di in _text)
			{
				if(di.NewLine)
				{
					output.AppendLine(di.Data);
				}
				else
				{
					output.Append(di.Data);
				}
			}
			return output.ToString();
		}

		public void PrintToConsole()
		{
			foreach(DescriptionItem di in _text)
			{
				SetColor(di.DescriptionCategory);
				if(di.NewLine)
				{
					Console.WriteLine(di.Data);
				}
				else
				{
					Console.Write(di.Data);
				}
				Console.ResetColor();
			}
		}

		private void SetColor(Category cat)
		{
			switch (cat)
			{
				case Category.Emphasized:
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				case Category.Warning:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case Category.Header:
					Console.ForegroundColor = ConsoleColor.Magenta;
					break;
				default:
					Console.ResetColor();
					break;
			}
		}

		public enum Category
		{
			PlainText,
			Emphasized,
			Header,
			Warning
		}

		public struct DescriptionItem
		{
			public Category DescriptionCategory;
			public string Data;
			public bool NewLine;

			public DescriptionItem(Category descriptionCategory, string data, bool newLine = true)
			{
				DescriptionCategory=descriptionCategory;
				Data = data;
				NewLine = newLine;
			}
		}
	}
}

