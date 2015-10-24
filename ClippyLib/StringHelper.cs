using System;
using System.Collections.Generic;

namespace ClippyLib
{
	public static class StringHelper
	{
		public static string[] ParseArguments(this string input)
		{
			string prevchar = "";
			bool isInString = false;
			string currentString = "";
			List<string> op = new List<string>();

			foreach (char c in input)
			{
				if (c == ' ')
				{
					//multiple spaces between words
					if (prevchar == " " && currentString.Length == 0 && !isInString)
					{
						currentString = "";
					}
					else if (!isInString)
					{
						op.Add(currentString);
						currentString = "";
					}
					else
					{
						currentString += c;
					}
				}
				else if (c == '"')
				{
					if (prevchar != "\\")
					{
						isInString = !isInString;
					}
				}
				else if (c == '\\')
				{
					if (prevchar == "\\")
					{
						prevchar = "backslash";
						continue;
					}
					currentString += c;
				}
				else
				{
					currentString += c;
				}
				prevchar = c.ToString();
			}

			op.Add(currentString);

			string[] arrop = op.ToArray();
			return arrop;
		}
	}
}

