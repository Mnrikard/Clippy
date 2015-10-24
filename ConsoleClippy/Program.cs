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
        [STAThread]
        static void Main(string[] args)
        {
            EditorManager manager = new EditorManager();
            if (args.Length > 0 && (args[0].Equals("help", StringComparison.CurrentCultureIgnoreCase) || args[0].Equals("/?", StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine(manager.Help(args));
                Console.ReadLine();
            }
            else if(args.Length > 0 && 
                    (args[0].Equals("--udfeditor", StringComparison.CurrentCultureIgnoreCase)
                     || args[0].Equals("--udf", StringComparison.CurrentCultureIgnoreCase)))
            {
            	clippy.UdfEditor editorForm = new clippy.UdfEditor();
            	editorForm.ShowDialog();
            }
            else
            {
                if (args.Length == 0)
                {
                    Console.WriteLine(manager.Help(args));
                    Console.WriteLine("Awaiting command");
                    args = manager.GetArgumentsFromString(Console.ReadLine());
                }
                manager.GetClipEditor(args[0]);
                manager.ClipEditor.EditorResponse += HandleResponseFromClippy;
                manager.ClipEditor.PersistentEditorResponse += HandleResponseFromClippy;

                SetParameters(manager, args);
                
                // if you've supplied at least one parameter, then set the rest, otherwise prompt
                if(args.Length > 1)
                {
	                foreach (Parameter parmWithDefault in (from Parameter p in manager.ClipEditor.ParameterList
	                                                       where !p.IsValued && p.DefaultValue != null
	                                                       select p))
	                {
	                	parmWithDefault.Value = parmWithDefault.DefaultValue;
	                }
                }

                while (!manager.ClipEditor.HasAllParameters)
                {
                	Parameter nextOne = manager.ClipEditor.GetNextParameter();
                	Console.WriteLine(String.Concat(nextOne.ParameterName, " [",nextOne.Expecting,"]:"));
                	
                    try
                    {
                        manager.ClipEditor.SetNextParameter(Console.ReadLine());
                    }
                    catch (InvalidParameterException ipe)
                    {
                        Console.WriteLine(ipe.ParameterMessage);
                        continue;
                    }
                }
                manager.ClipEditor.GetClipboardContent();
                manager.ClipEditor.Edit();
                manager.ClipEditor.SetClipboardContent();
                manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
                manager.ClipEditor.PersistentEditorResponse -= HandleResponseFromClippy;

                SaveThisCommand(args[0], manager.ClipEditor);
            }
        }

        private static void SaveThisCommand(string commandName, IClipEditor editor)
        {
            string[] parms = (from Parameter p in editor.ParameterList
                              orderby p.Sequence
                              select "\"" + (p.Value ?? p.DefaultValue ?? String.Empty).Replace("\"","\\q")+"\"").ToArray();
			string args = String.Join(" ", parms);

			var commandStore = ClippyLib.RecentCommands.Store.GetInstance();
			commandStore.SaveThisCommand(commandName, args);
        }

        private static void SetParameters(EditorManager manager, string[] args)
        {
            while (true)
            {
                try
                {
                    manager.ClipEditor.SetParameters(args);
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
                    manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
                    Console.WriteLine(udfe.FunctionMessage);
                    Console.WriteLine(manager.Help(args));
                    Console.WriteLine("Awaiting command");
                    args = manager.GetArgumentsFromString(Console.ReadLine());
                    manager.GetClipEditor(args[0]);
                    manager.ClipEditor.EditorResponse += new EventHandler<EditorResponseEventArgs>(HandleResponseFromClippy);
                }
            }
        }

        static void HandleResponseFromClippy(object sender, EditorResponseEventArgs e)
        {
            Console.WriteLine(e.ResponseString);
            if(e.RequiresUserAction)
                Console.ReadKey();
        }
    }
}
