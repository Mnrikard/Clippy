/*
 * Created by SharpDevelop.
 * Author: Matthew
 * Date: 8/17/2013 11:42 PM
 * 
 */
using System;
using System.IO;

namespace list
{
	/// <summary>
	/// Description of ListItem.
	/// </summary>
	public class ListItem
	{
		public string FullPath{get;set;}
		public string Name{get;set;}
		public int Column{get;set;}
		public int Row{get;set;}
		
		public ListItem(string path)
		{
			FullPath = path;
			Name = Path.GetFileName(path);
		}
	}
}
