/*
 * 
 * Copyright 2012 Matthew Rikard
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
 * Created by SharpDevelop.
 * User: Matthew
 * Date: 6/12/2013
 * Time: 11:15 PM
*/

using System;
using System.Text;
using ClippyLib;
using System.Linq;

namespace Manip
{
    class Program
    {
        static void Main(string[] args)
        {
        	StringBuilder contentb = new StringBuilder();
        	string line;
        	while((line = Console.In.ReadLine()) != null)
        	{
        		contentb.AppendLine(line);
        	}
        	string content = contentb.ToString();
        	
            EditorManager manager = new EditorManager();
            if (args.Length > 0 && (args[0].Equals("help", StringComparison.CurrentCultureIgnoreCase) || args[0].Equals("/?", StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine(manager.Help(args));
                Console.ReadLine();
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

                while (!manager.ClipEditor.HasAllParameters)
                {
                    Console.WriteLine(manager.ClipEditor.GetNextParameterName() + ":");
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
                manager.ClipEditor.SourceData = content;
                manager.ClipEditor.Edit();
                content = manager.ClipEditor.SourceData;
                manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
                manager.ClipEditor.PersistentEditorResponse -= HandleResponseFromClippy;

            }
            
            Console.Write(content);
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
            //if(e.RequiresUserAction)
                //Console.ReadKey();
        }
    }
}
