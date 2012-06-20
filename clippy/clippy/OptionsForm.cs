using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace clippy
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void udfBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dlg.Filter = " XML Files|*.xml";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    udfLocation.Text = dlg.FileName;
                }
            }
        }

        private void browseSnippets_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dlg.Filter = " XML Files|*.xml";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    snippetsLocation.Text = dlg.FileName;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey clippy = GetRegistryKey(hkcu, "Software\\Rikard\\Clippy");
            clippy.SetValue("udfLocation", udfLocation.Text, RegistryValueKind.String);
            clippy.SetValue("snippetsLocation", snippetsLocation.Text, RegistryValueKind.String);
            this.Close();
        }

        private RegistryKey GetRegistryKey(RegistryKey parentKey, string subKeyPath)
        {
            List<RegistryKey> keys = new List<RegistryKey>();
            keys.Add(parentKey);
            try
            {
                foreach (string keyname in subKeyPath.Split('\\'))
                {
                    keys.Add(keys[keys.Count - 1].CreateSubKey(keyname));
                }
                return keys[keys.Count - 1];
            }
            finally
            {
                for (int i = 1; i < keys.Count - 1; i++)
                {
                    keys[i].Close();
                }
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey clippy = GetRegistryKey(hkcu, "Software\\Rikard\\Clippy");
            udfLocation.Text = (clippy.GetValue("udfLocation") ?? String.Empty).ToString();
            snippetsLocation.Text = (clippy.GetValue("snippetsLocation") ?? String.Empty).ToString();
        }
    }
}
