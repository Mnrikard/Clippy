using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Installer
{
    public partial class Screen1 : UserControl
    {
        public Screen1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
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
                dlg.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(udfFolder.Text))
            {
                try
                {
                    Directory.CreateDirectory(udfFolder.Text);
                }
                catch
                {
                    MessageBox.Show("Cannot create the folder " + udfFolder.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (!Directory.Exists(installFolder.Text))
            {
                try
                {
                    Directory.CreateDirectory(installFolder.Text);
                }
                catch
                {
                    MessageBox.Show("Cannot create the folder " + installFolder.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkSoftware = hkcu.OpenSubKey("Software", true);
            rkSoftware.DeleteSubKey("Rikard");
            RegistryKey rkRikard = rkSoftware.CreateSubKey("Rikard", RegistryKeyPermissionCheck.ReadWriteSubTree);
            RegistryKey rkClippy = rkRikard.CreateSubKey("Clippy", RegistryKeyPermissionCheck.ReadWriteSubTree);
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


        }
    }
}
