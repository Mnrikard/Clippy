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
    public partial class SnippetEditor : Form
    {
        private XmlDocument _snipDocument = null;

        public SnippetEditor()
        {
            InitializeComponent();
        }

        private void SnippetEditor_Load(object sender, EventArgs e)
        {
            List<string> udFunctions = GetSnippets();
            snippetList.DataSource = udFunctions;
        }

        private XmlDocument SnipDocument
        {
            get
            {
                if (_snipDocument == null)
                {
                    RegistryKey hkcu = Registry.CurrentUser;
                    RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
                    object udfLocation = rkUdfLocation.GetValue("snippetsLocation");
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(udfLocation.ToString());
                    _snipDocument = xdoc;
                }
                return _snipDocument;
            }
            set
            {
                RegistryKey hkcu = Registry.CurrentUser;
                RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
                object udfLocation = rkUdfLocation.GetValue("snippetsLocation");
                _snipDocument = value;
                _snipDocument.Save(udfLocation.ToString());
            }
        }


        public List<string> GetSnippets()
        {
            XmlDocument descSnip = SnipDocument;
            XmlNodeList snips = descSnip.SelectNodes("//Snippet/@Name");
            List<string> output = new List<string>();
            foreach (XmlNode snip in snips)
            {
                output.Add(snip.Value);
            }
            return output;
        }

        private void snippetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            snippetDescription.Text = String.Empty;
            snippetContent.Text = String.Empty;
            XmlDocument descUdf = SnipDocument;
            XmlNode passedInSnip = descUdf.SelectSingleNode("//Snippet[translate(@Name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + snippetList.Text.ToLower() + "\"]");
            if (passedInSnip == null)
            {
                return;
            }
            XmlNode desc = passedInSnip.SelectSingleNode("Description");
            if(desc != null)
            {
                snippetDescription.Text = desc.InnerText;
            }
            XmlNode content = passedInSnip.SelectSingleNode("Content");
            if (content == null)
                return;
            snippetContent.Text += content.InnerText;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            XmlDocument snipdoc = SnipDocument;
            XmlNode snip = snipdoc.SelectSingleNode("//Snippet[translate(@Name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" + snippetList.Text.ToLower() + "\"]");
            if (snip == null)
            {
                //new udf
                XmlNode command = snipdoc.CreateElement("Snippet");
                XmlAttribute key = snipdoc.CreateAttribute("Name");
                key.Value = snippetList.Text;
                command.Attributes.Append(key);

                XmlNode desc = snipdoc.CreateElement("Description");
                desc.InnerText = snippetDescription.Text;
                command.AppendChild(desc);

                XmlCDataSection cdatfx = snipdoc.CreateCDataSection(snippetContent.Text);
                XmlNode content = snipdoc.CreateElement("Content");
                content.AppendChild(cdatfx);
                command.AppendChild(content);
                XmlNode snips = _snipDocument.SelectSingleNode("/Snippets");
                snips.AppendChild(command);
            }
            else
            {

                XmlNode desc = snip.SelectSingleNode("Description");
                if (desc == null)
                {
                    desc = snipdoc.CreateElement("Description");
                    snip.AppendChild(desc);
                }
                desc.InnerText = snippetDescription.Text;

                while(snip.SelectSingleNode("Content") != null)
                    snip.RemoveChild(snip.SelectSingleNode("Content"));

                XmlCDataSection cdatfx = snipdoc.CreateCDataSection(snippetContent.Text);
                XmlNode content = snipdoc.CreateElement("Content");
                content.AppendChild(cdatfx);
                snip.AppendChild(content);
                
            }
            SnipDocument = _snipDocument;
            this.Close();
        }
    }
}
