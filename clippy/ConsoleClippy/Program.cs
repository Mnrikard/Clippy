using System;
using ClippyLib;

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
                manager.ClipEditor.GetClipboardContent();
                manager.ClipEditor.Edit();
                manager.ClipEditor.SetClipboardContent();
                manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
                manager.ClipEditor.PersistentEditorResponse -= HandleResponseFromClippy;
            }
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
