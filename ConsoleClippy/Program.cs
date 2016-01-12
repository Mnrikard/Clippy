/*
 * 
 * Copyright 2012-2015 Matthew Rikard
 * This file is part of Clippy.
 * 
 *  Clippy is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Clippy is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Clippy.  If not, see <http://www.gnu.org/licenses/>.
 *
*/

using System;
using System.Linq;
using clippy;
using ClippyLib;
using ClippyLib.Editors;
using System.Windows.Forms;

namespace ConsoleClippy
{
    class Program
    {
		private static EditorManager _manager;
        [STAThread]
        static void Main(string[] args)
        {
            _manager = new EditorManager();
			if(ShowingHelp(args) || ShowingUdfEditor(args))
			{
				return;
			}

            if (args.Length == 0)
            {
                Console.WriteLine(_manager.Help(args));
                Console.WriteLine("Awaiting command");
                args = _manager.GetArgumentsFromString(Console.ReadLine());
            }

            _manager.GetClipEditor(args[0]);
            _manager.ClipEditor.EditorResponse += HandleResponseFromClippy;
            _manager.ClipEditor.PersistentEditorResponse += HandleResponseFromClippy;

			SetParametersObtainedFromConsole(args);

			if(AtLeastOneParameterPassedIn(args))
				UseDefaultForRemainingParameters(args);

			PromptForRemainingParameters();
			
			_manager.ClipEditor.GetClipboardContent();
            _manager.ClipEditor.Edit();
            _manager.ClipEditor.SetClipboardContent();

            SaveThisCommand(args[0]);

			
			_manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
			_manager.ClipEditor.PersistentEditorResponse -= HandleResponseFromClippy;
        }

		private static bool AtLeastOneParameterPassedIn(string[] args)
		{
			return (args.Length > 1);
		}

		private static bool ShowingHelp(string[] args)
		{
			if (args.Length > 0 && (args[0].Equals("help", StringComparison.CurrentCultureIgnoreCase) || args[0].Equals("/?", StringComparison.CurrentCultureIgnoreCase)))
			{
				_manager.Help(args).PrintToConsole();
				Console.ReadLine();
				return true;
			}

			return false;
		}

		private static bool ShowingUdfEditor(string[] args)
		{
			if(args.Length > 0 && 
			   (args[0].Equals("--udfeditor", StringComparison.CurrentCultureIgnoreCase)
			 || args[0].Equals("--udf", StringComparison.CurrentCultureIgnoreCase)))
			{
				clippy.UdfEditor editorForm = new clippy.UdfEditor();
				editorForm.ShowDialog();
				return true;
			}

			return false;
		}

		private static void UseDefaultForRemainingParameters(string[] args)
		{
			if(args.Length > 1)
			{
				foreach (Parameter parmWithDefault in (from Parameter p in _manager.ClipEditor.ParameterList
				                                       where !p.IsValued && p.DefaultValue != null
				                                       select p))
				{
					parmWithDefault.Value = parmWithDefault.DefaultValue;
				}
			}
		}

		private static void PromptForRemainingParameters()
		{
			while (!_manager.ClipEditor.HasAllParameters)
			{
				Parameter nextOne = _manager.ClipEditor.GetNextParameter();
				Console.WriteLine(String.Concat(nextOne.ParameterName, " [",nextOne.Expecting,"]:"));

				try
				{
					_manager.ClipEditor.SetNextParameter(Console.ReadLine());
				}
				catch (InvalidParameterException ipe)
				{
					Console.WriteLine(ipe.ParameterMessage);
					continue;
				}
			}
		}

        private static void SaveThisCommand(string commandName)
        {
            string[] parms = (from Parameter p in _manager.ClipEditor.ParameterList
                              orderby p.Sequence
                              select "\"" + (p.Value ?? p.DefaultValue ?? String.Empty).Replace("\"","\\q")+"\"").ToArray();
			string args = String.Join(" ", parms);

			var commandStore = ClippyLib.RecentCommands.Store.GetInstance();
			commandStore.SaveThisCommand(commandName, args);
        }

        private static void SetParametersObtainedFromConsole(string[] args)
        {
            while (true)
            {
                try
                {
                    _manager.ClipEditor.SetParameters(args);
                    break;
                }
                catch (ClippyLib.InvalidParameterException pe)
                {
                    Console.WriteLine("Error: " + pe.ParameterMessage);
                    Console.WriteLine("Press enter to continue, results are not guaranteed\r\n");
                    Console.ReadLine();
                    break;
                }
                catch (ClippyLib.UndefinedFunctionException udfe)
                {
                    _manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
                    Console.WriteLine(udfe.FunctionMessage);
                    Console.WriteLine(_manager.Help(args));
                    Console.WriteLine("Awaiting command");
                    args = _manager.GetArgumentsFromString(Console.ReadLine());
                    _manager.GetClipEditor(args[0]);
                    _manager.ClipEditor.EditorResponse += HandleResponseFromClippy;
                }
            }
        }

        static void HandleResponseFromClippy(object sender, EditorResponseEventArgs e)
        {
			if(e.ResponseDescription == null)
			{
				Console.WriteLine(e.ResponseString);
			}
			else
			{
				e.ResponseDescription.PrintToConsole();
			}

            if(e.RequiresUserAction)
                Console.ReadKey();
        }
    }
}
