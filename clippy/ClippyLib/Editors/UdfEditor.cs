using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace ClippyLib.Editors
{
    public class UdfEditor : AClipEditor
    {
        private string _udfName;
        private XmlDocument _udfSettings;
        private string[] _arguments;

        public override string EditorName
        {
            get { return "Will not be a normal usable function"; }
        }

        #region .ctor
        public UdfEditor()
        {
            _parameterList = new List<Parameter>();
        }
        #endregion

        public override void DefineParameters()
        {
            
        }

        public override string ShortDescription
        {
            get { return "Will not be registered"; }
        }

        public override string LongDescription
        {
            get { return @"Will not be registered"; }
        }

        public override void SetParameters(string[] args)
        {
            _udfName = args[0];
            _arguments = args;
            if (!CommandExists(args))
            {
                throw new UndefinedFunctionException("Function \"{0}\" does not exist", args[0]);
            }
        }

        public override void Edit()
        {
            bool isHelp = _arguments[0].Equals("help", StringComparison.CurrentCultureIgnoreCase);
            if (isHelp)
            {
                EditorManager manager = new EditorManager();
                RespondToExe(manager.Help(_arguments));
            }
            else
            {
                List<string> functions = Udf(_arguments);
                if (functions.Count == 0 && !isHelp)
                    throw new Exception(String.Format("Function:{0} does not exist or is not valid", _udfName));
                EditorManager manager = new EditorManager();

                foreach (string function in functions)
                {
                    string[] fargs = GetArgsFromString(function);
                    manager.GetClipEditor(fargs[0]);

                    manager.ClipEditor.EditorResponse += new EventHandler<EditorResponseEventArgs>(HandleResponseFromClippy);

                    manager.ClipEditor.DefineParameters();
                    manager.ClipEditor.SetParameters(fargs);
                    if (!manager.ClipEditor.HasAllParameters)
                    {
                        throw new Exception(String.Format("Not all parameters are passed in the user defined function {0}, function: {1}", _udfName, function));
                    }
                    manager.ClipEditor.SourceData = SourceData;
                    manager.ClipEditor.Edit();
                    SourceData = manager.ClipEditor.SourceData;

                    manager.ClipEditor.EditorResponse -= HandleResponseFromClippy;

                }
            }
        }

        private void HandleResponseFromClippy(object sender, EditorResponseEventArgs e)
        {
            RespondToExe(e.ResponseString, e.RequiresUserAction);
        }

        private List<string> Udf(string[] key)
        {
            List<string> output = new List<string>();

            if (_udfSettings == null)
            {
                _udfSettings = UdfDocument();
            }
            XmlNodeList cmds = _udfSettings.SelectNodes("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + key[0].ToLower() + "\"]/function");
            foreach (XmlNode cmd in cmds)
            {
                string currcmd = cmd.InnerText;
                for (int i = 1; i < key.Length; i++)
                {
                    currcmd = currcmd.Replace("%" + (i - 1).ToString() + "%", key[i]);
                }
                output.Add(currcmd);
            }
            return output;
        }

        private bool CommandExists(string[] key)
        {
            if (_udfSettings == null)
            {
                _udfSettings = UdfDocument();
            }
            XmlNode cmd = _udfSettings.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + key[0].ToLower() + "\"]");
            return cmd != null;
        }

        public static string[] GetArgsFromString(string input)
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

        public static void DescribeFunctions(StringBuilder output)
        {
            XmlDocument descUdf = UdfDocument();
            XmlNodeList cmds = descUdf.SelectNodes("//command");
            foreach (XmlNode udf in cmds)
            {
                string description = "No description available";
                XmlNode descriptionNode = udf.SelectSingleNode("description");
                if (descriptionNode != null)
                    description = descriptionNode.InnerText;
                XmlAttribute keyNameNode = udf.Attributes["key"];
                output.AppendFormat("{0}  -  {1}\r\n", keyNameNode.Value, description);
            }
        }

        private static XmlDocument UdfDocument()
        {
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
            object udfLocation = rkUdfLocation.GetValue("udfLocation");
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(udfLocation.ToString());
            return xdoc;
        }

        public static List<string> GetFunctions()
        {
            XmlDocument descUdf = UdfDocument();
            XmlNodeList cmds = descUdf.SelectNodes("//command/@key");
            List<string> output = new List<string>();
            foreach (XmlNode udf in cmds)
            {
                output.Add(udf.Value);
            }
            return output;
        }
    }
}
