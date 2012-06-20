using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections.Generic;

namespace Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
                //if (String.IsNullOrEmpty(dlg.RootFolder))
                //{
                //    dlg.RootFolder = Environment.SpecialFolder.ProgramFiles;
                //}
                dlg.Description = "Select a folder to install Clippy";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    installFolder.Text = dlg.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.RootFolder = Environment.SpecialFolder.MyDocuments;
                dlg.Description = "Select a folder to install Clippy";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    udfFolder.Text = dlg.SelectedPath;
                }
            }
        }

        private void Screen1_Load(object sender, EventArgs e)
        {
            udfFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            installFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (String.IsNullOrEmpty(installFolder.Text))
            {
                installFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateSpecialFolder(udfFolder.Text);
            CreateSpecialFolder(installFolder.Text);

            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkClippy = GetRegistryKey(hkcu, "Software\\Rikard\\Clippy");
            rkClippy.SetValue("snippetsLocation", Path.Combine(udfFolder.Text, "snippets.xml"), RegistryValueKind.String);
            rkClippy.SetValue("udfLocation", Path.Combine(udfFolder.Text, "udf.xml"), RegistryValueKind.String);

            foreach (string file in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "installbin")))
            {
                File.Copy(file, Path.Combine(installFolder.Text, Path.GetFileName(file)));
            }
            foreach (string file in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "installuser")))
            {
                File.Copy(file, Path.Combine(udfFolder.Text, Path.GetFileName(file)));
            }

            if (saveToPath.Checked)
            {
                string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                if (!currentPath.ToUpper().Contains(installFolder.Text.ToUpper()))
                {
                    if (!currentPath.EndsWith(";"))
                        currentPath += ";";
                    currentPath += installFolder.Text;
                }
                Environment.SetEnvironmentVariable("PATH", currentPath, EnvironmentVariableTarget.User);
            }
            this.Close();
        }

        private void CreateSpecialFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                try
                {
                    Directory.CreateDirectory(folderName);
                }
                catch
                {
                    MessageBox.Show("Cannot create the folder " + folderName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
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
    }
}
