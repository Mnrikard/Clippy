/*
 * Created by SharpDevelop.
 * User: Matthew
 * Date: 8/11/2013
 * Time: 12:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace list
{
	class Program
	{
		public static void Main(string[] args)
		{
			var cd = Environment.CurrentDirectory;
			Ls ls = new Ls(cd);
			ls.RepaintConsole();
		}
	}
}