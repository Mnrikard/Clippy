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
    public partial class RecentCommands : Form
    {
        public RecentCommands()
        {
            InitializeComponent();
        }

        public string SelectedCommand { get; set; }

        private void CommandListClick(object sender, EventArgs e)
        {
            SelectedCommand = commandList.Items[commandList.SelectedIndex].ToString();
            this.Close();
        }

        private string[] GetRecentCommandList()
        {
            RegistryKey hkcu = Registry.CurrentUser;
            RegistryKey rkClippy = GetRegistryKey(hkcu, "Software\\Rikard\\Clippy\\MRU");

            string[] names = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] commands = new string[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                object ocommand = rkClippy.GetValue(names[i]);
                ocommand = ocommand ?? String.Empty;
                commands[i] = ocommand.ToString();
            }


            rkClippy.Close();
            hkcu.Close();

            return commands;
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

        private void RecentCommands_Load(object sender, EventArgs e)
        {
            commandList.DoubleClick -= CommandListClick;

            commandList.Items.Clear();

            string[] commands = GetRecentCommandList();
            foreach (string command in commands)
            {
                if (!String.IsNullOrWhiteSpace(command))
                {
                    commandList.Items.Add(command);
                }
            }

            commandList.DoubleClick += CommandListClick;
        }

    }
}
