using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;

namespace clippy
{
    public partial class UdfEditor : Form
    {
        public UdfEditor()
        {
            InitializeComponent();
        }


        private XmlDocument _udfDocument = null;

        private void UdfEditor_Load(object sender, EventArgs e)
        {
            LoadFunctions();
        }

        private void LoadFunctions()
        {
            List<string> udFunctions = GetFunctions();
            udFunctions.Sort();
            functionList.DataSource = udFunctions;
        }

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


        public List<string> GetFunctions()
        {
            XmlDocument descUdf = UdfDocument;
            XmlNodeList cmds = descUdf.SelectNodes("//command/@key");
            List<string> output = new List<string>();
            foreach (XmlNode udf in cmds)
            {
                output.Add(udf.Value);
            }
            return output;
        }

        private void functionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            fxDescription.Text = String.Empty;
            fxCommands.Text = String.Empty;
            XmlDocument descUdf = UdfDocument;
            XmlNode passedInFunc = descUdf.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + functionList.Text.ToLower() + "\"]");
            if (passedInFunc == null)
            {
                return;
            }
            XmlNode desc = passedInFunc.SelectSingleNode("description");
            if(desc != null)
            {
                fxDescription.Text = desc.InnerText;
            }
            XmlNodeList fxs = passedInFunc.SelectNodes("function");
            foreach(XmlNode fx in fxs)
            {
                fxCommands.Text += fx.InnerText + Environment.NewLine;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            XmlDocument udfs = UdfDocument;
            XmlNode command = udfs.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + functionList.Text.ToLower() + "\"]");

            if (command == null)
            {
                //new udf
                command = udfs.CreateElement("command");
                XmlAttribute key = udfs.CreateAttribute("key");
                key.Value = functionList.Text;
                command.Attributes.Append(key);

                XmlNode desc = udfs.CreateElement("description");
                desc.InnerText = fxDescription.Text;
                command.AppendChild(desc);

                string[] fxs = fxCommands.Text.Split('\n');
                foreach (string fx in fxs)
                {
                    if (String.IsNullOrEmpty(fx.Trim()))
                        continue;
                    XmlCDataSection cdatfx = udfs.CreateCDataSection(fx.Trim());
                    XmlNode fxNd = udfs.CreateElement("function");
                    fxNd.AppendChild(cdatfx);
                    command.AppendChild(fxNd);
                }
                XmlNode cmds = _udfDocument.SelectSingleNode("/commands");
                cmds.AppendChild(command);
            }
            else
            {
                
                XmlNode desc = command.SelectSingleNode("description");
                if (desc == null)
                {
                    desc = udfs.CreateElement("description");
                    command.AppendChild(desc);
                }
                desc.InnerText = fxDescription.Text;

                while(command.SelectSingleNode("function") != null)
                    command.RemoveChild(command.SelectSingleNode("function"));

                string[] fxs = fxCommands.Text.Split('\n');
                foreach (string fx in fxs)
                {
                    if (String.IsNullOrEmpty(fx.Trim()))
                        continue;
                    XmlCDataSection cdatfx = udfs.CreateCDataSection(fx.Trim());
                    XmlNode fxNd = udfs.CreateElement("function");
                    fxNd.AppendChild(cdatfx);
                    command.AppendChild(fxNd);
                }
            }
            UdfDocument = _udfDocument;
            this.Close();
        }

        private void loadMruButton_Click(object sender, EventArgs e)
        {
            RecentCommands rcnt = new RecentCommands();
            rcnt.FormClosing += new FormClosingEventHandler(RecentCommandClose);
            rcnt.ShowDialog();
        }

        void RecentCommandClose(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(((RecentCommands)sender).SelectedCommand) && ((RecentCommands)sender).SelectedCommand.Trim().Length > 0)
            {
                fxCommands.Text = fxCommands.Text.Trim() + "\r\n" + ((RecentCommands)sender).SelectedCommand;
            }
        }

        private void DeleteUdf(string functionName)
        {
            XmlDocument udfs = _udfDocument;
            XmlNode command = udfs.SelectSingleNode("//command[translate(@key,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + functionName.ToLower() + "\"]");

            if (command != null)
            {
                command.ParentNode.RemoveChild(command);
                UdfDocument = _udfDocument;
            }
            functionList.Text = String.Empty;
            fxDescription.Text = String.Empty;
            fxCommands.Text = String.Empty;
            int position = functionList.SelectedIndex;
            LoadFunctions();
            if(position < functionList.Items.Count)
                functionList.SelectedIndex = position;
        }

        private void deleter_Click(object sender, EventArgs e)
        {
            DialogResult candelete = MessageBox.Show(String.Format("Are you sure you wish to delete " + functionList.Text), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (candelete == DialogResult.Yes)
            {
                DeleteUdf(functionList.Text);
            }
        }


    }
}
