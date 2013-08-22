/*
 * Created by SharpDevelop.
 * User: Matthew
 * Date: 8/17/2013
 * Time: 10:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace list
{
	/// <summary>
	/// Description of Ls.
	/// </summary>
	public class Ls
	{
		public  string CurrentDirectory {get;private set;}
		
		private int _columns = 4;
		public int Columns {
			get {return _columns;}
			set { _columns = value; }
		}
				
		public Ls(string currentDirectory)
		{
			CurrentDirectory = currentDirectory;
		}
		
		private List<ListItem> _items;
		
		public void RepaintConsole()
		{
			Environment.CurrentDirectory = CurrentDirectory; 
			
			List<string> dirs = new List<string>();				
			dirs.AddRange((from string dir in Directory.GetDirectories(CurrentDirectory)
			               select dir));
			dirs.AddRange((from string fil in Directory.GetFiles(CurrentDirectory)
						   select fil));
			
			Console.Clear();
			_curRow = null;
			_curCol = null;
			
			int row=0;
			int colIndex=0;
			
			
			_items = new List<ListItem>();
			_items.Add(new ListItem(Path.GetDirectoryName(CurrentDirectory)){Name="..",Column=colIndex++,Row=row});
			
			foreach(string dir in dirs)
			{	
				ListItem itm = new ListItem(dir){Column=colIndex,Row=row};
				_items.Add(itm);				
				colIndex++;
				
				int thisRow = row;
				
				if(colIndex >= _columns)
				{
					row++;
					colIndex=0;
				}				
			}
			
			DisplayItems();
			

		}
		
		private void DisplayItems(int startRow = 0)
		{
			Console.Clear();
			
			if(_curRow.HasValue && _curRow+1 >= Console.WindowHeight + startRow)
			{
				startRow = _curRow.Value - Console.WindowHeight + 2;
				if(startRow < 0) startRow = 0;
			}
			
			
			int charEach = (Console.WindowWidth - 2*(_columns-1))/_columns;
			
			foreach(ListItem itm in _items)
			{
				if(itm.Row < startRow)
				{
					continue;
				}
				
				if(itm.Row+1 >= Console.WindowHeight + startRow)
				{
					continue;
				}
				
				Console.SetCursorPosition((charEach+2) * itm.Column, itm.Row - startRow);
				
				if(itm.Column == _curCol && itm.Row == _curRow)
				{
					ConsoleColor fgColor = Console.ForegroundColor;
					Console.ForegroundColor = Console.BackgroundColor;
					Console.BackgroundColor = fgColor;
				}
				
				Console.Write(Left(itm.Name, charEach));
				
				Console.ResetColor();
			}
			
			Console.SetCursorPosition(0,Console.WindowHeight-1);
			Console.Write(CurrentDirectory);
			Console.SetCursorPosition(0,Console.WindowHeight-1);
			
			ReadInput();
		}
		
		private void ReadInput()
		{
			string word = String.Empty;
			while(true)
			{
				ConsoleKeyInfo key = Console.ReadKey();
				if(key.Key == ConsoleKey.LeftArrow)
				{
					ProcessMovement(0,-1);
					break;
				}
				else if(key.Key == ConsoleKey.RightArrow)
				{
					ProcessMovement(0,1);
					break;
				}
				else if(key.Key == ConsoleKey.UpArrow)
				{
					ProcessMovement(-1,0);
					break;
				}
				else if(key.Key == ConsoleKey.DownArrow)
				{
					ProcessMovement(1,0);
					break;
				}
				else if(key.Key == ConsoleKey.Enter)
				{
					ProcessWord(word);
					break;
				}
				else
				{
					if(key.Key == ConsoleKey.Backspace)
						word = word.Substring(0,word.Length-1);
					else
						word += key.KeyChar.ToString();
					
					Console.SetCursorPosition(0,Console.WindowHeight-1);
					Console.Write(new String(' ',Console.WindowWidth-1));
					Console.SetCursorPosition(0,Console.WindowHeight-1);
					Console.Write(word);
					Console.SetCursorPosition(word.Length,Console.WindowHeight-1);
			
				}
			}
		}
		
		private int? _curRow = null;
		private int? _curCol = null;
		
		private void ProcessMovement(int rowMove, int colMove)
		{
			int maxRow = (from b in _items select b.Row).Max();
			int maxColLastRow = (from x in _items where x.Row == maxRow select x.Column).Max();
			
			if(!_curRow.HasValue)
			{
				_curRow = maxRow + 1;
			}
			if(!_curCol.HasValue)
			{
				_curCol = (from b in _items select b.Column).Max()+1;
			}
			
			_curCol += colMove;
			_curRow += rowMove;
			
			//special logic for the last row, if it has fewer columns
			
			if(_curRow == maxRow && _curCol > maxColLastRow)
			{
				_curCol = maxColLastRow;
			}
			
			if(_curRow > maxRow || _curCol >= Columns)
			{
				_curRow = 0;
				_curCol = 0;
			}
			
			DisplayItems();
			ReadInput();
		}
		
		private void ProcessWord(string word)
		{
			if(word == String.Empty)
			{
				ListItem selectedItem = _items.FirstOrDefault(i => i.Row == _curRow && i.Column == _curCol);
				if(selectedItem != null)
				{
					word = selectedItem.Name;
				}
			}
			
			string newDir = Path.Combine(CurrentDirectory,word);
			if(word == "..")
			{
				newDir = Path.GetDirectoryName(CurrentDirectory);
			}
			
			if(Directory.Exists(newDir))
			{
				CurrentDirectory = newDir;
				RepaintConsole();
			}
		}
		
		
		
		private string Left(string itm, int len)
		{
			if(itm.Length <= len) return itm;
			return itm.Substring(0,len);
		}
	}
}
