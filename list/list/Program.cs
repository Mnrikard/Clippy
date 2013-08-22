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
			Console.CancelKeyPress += (a,b) => {
				System.IO.Directory.SetCurrentDirectory(ls.CurrentDirectory);
				Console.WriteLine("|");
				Console.WriteLine(ls.CurrentDirectory);
				Console.WriteLine("|");
				System.Environment.Exit(0);
			};
			
			ls.RepaintConsole();
		}
	}
}