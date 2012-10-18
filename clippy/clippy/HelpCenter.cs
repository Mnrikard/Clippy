using ClippyLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;

namespace clippy
{
    public partial class HelpCenter : Form
    {

        private EditorManager clipManager;

        public HelpCenter()
        {
            InitializeComponent();
            clipManager = new EditorManager();
        }

        public string ChosenOne { get; set; }
        
        private void HelpCenterLoad(object sender, EventArgs e)
        {
            LoadFunctions();
        }

        private void LoadFunctions()
        {
            functionList.Items.Clear();
            string[] editors = clipManager.GetEditors();
            Array.Sort(editors, StringComparer.CurrentCultureIgnoreCase);
            foreach (string editor in editors)
            {
                functionList.Items.Add(editor);
            }
        }

        private void FunctionListSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayHelp();
        }

        private void FunctionListLeave(object sender, EventArgs e)
        {
            DisplayHelp();
        }

        private void DisplayHelp()
        {
            string editorName = functionList.Text;
            bool isUdf = true;
            for (int i = 0; i < clipManager.Editors.Count; i++)
            {
                if (clipManager.Editors[i].EditorName.Equals(editorName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    isUdf = false;
                    break;
                }
            }
            if (isUdf)
                helpBox.Text = DisplayUdf(editorName);
            else
                helpBox.Text = clipManager.Help(new string[] { String.Empty, editorName });
        }

        private string DisplayUdf(string editorName)
        {
            Dictionary<string,string> udFuncs = GetFunctions();
            string baseDesc = "User defined function: see Tools > Open User Functions\r\n------------------------------------------------------\r\n\r\n";
            foreach(string key in udFuncs.Keys)
            {
                if (editorName.Trim().Equals(key.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if(udFuncs[key] == String.Empty)
                        break;
                    return baseDesc + udFuncs[key];
                }
            }
            return baseDesc + clipManager.Help(new string[] { });
        }

        private XmlDocument _udfDocument = null;

        private XmlDocument UdfDocument
        {
            get
            {
                if (_udfDocument == null)
                {
                    RegistryKey hkcu = Registry.CurrentUser;
                    RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
                    object udfLocation = rkUdfLocation.GetValue("udfLocation");
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(udfLocation.ToString());
                    _udfDocument = xdoc;
                }
                return _udfDocument;
            }
            set
            {
                RegistryKey hkcu = Registry.CurrentUser;
                RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
                object udfLocation = rkUdfLocation.GetValue("udfLocation");
                _udfDocument = value;
                _udfDocument.Save(udfLocation.ToString());
            }
        }

        public Dictionary<string,string> GetFunctions()
        {
            XmlDocument descUdf = UdfDocument;
            XmlNodeList cmds = descUdf.SelectNodes("//command");          
            Dictionary<string, string> output = new Dictionary<string, string>();
            foreach (XmlNode udf in cmds)
            {
                XmlNode cmdNameNd = udf.SelectSingleNode("@key");
                XmlNode cmdDescNd = udf.SelectSingleNode("description");
                if(cmdNameNd != null)
                {
                    string desc = cmdDescNd == null ? String.Empty : cmdDescNd.InnerText;
                    output.Add(cmdNameNd.Value, desc);
                }
            }
            return output;
        }

        private void choicerBtn_Click(object sender, EventArgs e)
        {
            ChosenOne = functionList.Text;
            Close();
        }
    }
}
