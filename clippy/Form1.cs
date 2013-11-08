﻿/*
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
*/

using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClippyLib;

namespace clippy
{
    public partial class Form1 : Form
    {
        private EditorManager clipManager;
        private string _currentCommand;
        private PersistentInformation _infoBox;

        #region .ctor
        public Form1()
        {
            InitializeComponent();
            clipManager = new EditorManager();
            _currentCommand = String.Empty;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //MainFormLoad(sender, e);
            LoadFunctions();
            functions.Focus();
        }

        private void LoadFunctions()
        {
            functions.Items.Clear();
            string[] editors = clipManager.GetEditors();
            Array.Sort(editors, StringComparer.CurrentCultureIgnoreCase);
            foreach (string editor in editors)
            {
                functions.Items.Add(editor);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            MinimizeForm();
        }
        
        private void HandleResponseFromClippy(object sender, EditorResponseEventArgs e)
        {
            MessageBox.Show(e.ResponseString, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }

        private void HandlePersistentResponse(object sender, EditorResponseEventArgs e)
        {
            if (_infoBox != null)
            {
                _infoBox.Close();
                _infoBox = null;
            }
            _infoBox = new PersistentInformation(e.ResponseString);
            _infoBox.StartPosition = FormStartPosition.CenterParent;
            _infoBox.Show();
        }

        #region menu commands

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreForm();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //actHook.Stop();
            this.FormClosing -= Form1_FormClosing;
            this.Close();
        }
        private void openUserFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UdfEditor ude = new UdfEditor();
            ude.FormClosed += (a, b) => LoadFunctions();
            ude.StartPosition = FormStartPosition.CenterParent;
            ude.ShowDialog(this);
        }

        private void openSnippetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SnippetEditor se = new SnippetEditor();
            se.StartPosition = FormStartPosition.CenterParent;
            se.ShowDialog(this);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.FormClosing -= Form1_FormClosing;
            this.Close();
        }

        private void clipNotify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RestoreForm();
            }
        }

        #endregion

        #region form events

        private void FunctionOnLeave(object sender, EventArgs e)
        {
            errorLabel.Text = String.Empty;
            functions.BackColor = Color.White;

            string[] arguments = clipManager.GetArgumentsFromString(functions.Text);
            if (arguments.Length == 0)
                return;

            if (functions.Text.Equals(_currentCommand, StringComparison.CurrentCultureIgnoreCase))
            {
                string[] newArgs = new string[parametersGrid.Rows.Count + 1];
                newArgs[0] = arguments[0];
                for (int ia = 1; ia < newArgs.Length; ia++)
                {
                    newArgs[ia] = parametersGrid.Rows[ia - 1].Cells[1].Value.ToString();
                }
                arguments = newArgs;
            }

            _currentCommand = arguments[0];

            clipManager.GetClipEditor(arguments[0]);
            clipManager.ClipEditor.EditorResponse += HandleResponseFromClippy;
            clipManager.ClipEditor.PersistentEditorResponse += HandlePersistentResponse;
            try
            {
                clipManager.ClipEditor.SetParameters(arguments);
            }
            catch (ClippyLib.InvalidParameterException pe)
            {
                MessageBox.Show(pe.ParameterMessage, "Error creating parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                functions.Focus();
            }
            catch (ClippyLib.UndefinedFunctionException udfe)
            {
                errorLabel.Text = udfe.FunctionMessage;
                functions.BackColor = Color.Yellow;
                functions.Focus();
                return;
            }

            DataTable parms = new DataTable();
            parms.Columns.Add("Parameter");
            parms.Columns.Add("Value");
            foreach (Parameter p in clipManager.ClipEditor.ParameterList)
            {
                DataRow dr = parms.NewRow();
                dr["Parameter"] = p.ParameterName;
                if (p.IsValued)
                    dr["Value"] = ParmEscape(p.Value);
                else if (!p.Required)
                    dr["Value"] = ParmEscape(p.DefaultValue);
                else
                    dr["Value"] = String.Empty;
                parms.Rows.Add(dr);
            }
            parametersGrid.DataSource = parms;
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            int i=1;
            StringBuilder parmString = new StringBuilder();
            foreach (DataGridViewRow dr in parametersGrid.Rows)
            {
                string parmValue = dr.Cells["Value"].Value == null ? String.Empty : dr.Cells["Value"].Value.ToString();
                try
                {
                    clipManager.ClipEditor.SetParameter(i, parmValue);
                }
                catch (ClippyLib.InvalidParameterException pe)
                {
                    MessageBox.Show(pe.ParameterMessage, "Error with passed parameter: \"" + parmValue + "\"", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (IndexOutOfRangeException)
                {
                	MessageBox.Show("Error with command, possibly not a function of clippy", "Error with command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                parmString.AppendFormat(" \"{0}\"",parmValue.Replace("\"","\\q").Replace("\t","\\t"));
                i++;
            }

            try
            {
                clipManager.ClipEditor.GetClipboardContent();
                clipManager.ClipEditor.Edit();
                clipManager.ClipEditor.SetClipboardContent();
            }
            catch (UndefinedFunctionException udfex)
            {
                MessageBox.Show(udfex.FunctionMessage);
            }

            SaveThisCommand(_currentCommand, parmString.ToString());

            clipManager.ClipEditor.EditorResponse -= HandleResponseFromClippy;
            functions.Focus();

            try
            {
                _infoBox.Close();
                _infoBox = null;
            }
            catch { }
            this.Close();
        }


        #endregion

        

        private void SaveThisCommand(string editorName, string parms)
        {
            ClippyLib.RecentCommands.SaveThisCommand(editorName, parms);
        }

        private string[] GetRecentCommandList()
        {
            return ClippyLib.RecentCommands.GetRecentCommandList();
        }

        private string ParmEscape(string value)
        {
            return value.Replace("\t", "\\t")
                .Replace("\n", "\\n");
        }
        
        private void RestoreForm()
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void MinimizeForm()
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm ofrm = new OptionsForm();
            ofrm.FormClosed += (a, b) => LoadFunctions();
            ofrm.StartPosition = FormStartPosition.CenterParent;
            ofrm.ShowDialog(this);
        }

        //private RegistryKey GetRegistryKey(RegistryKey parentKey, string subKeyPath)
        //{
        //    List<RegistryKey> keys = new List<RegistryKey>();
        //    keys.Add(parentKey);
        //    try
        //    {
        //        foreach (string keyname in subKeyPath.Split('\\'))
        //        {
        //            keys.Add(keys[keys.Count - 1].CreateSubKey(keyname));
        //        }
        //        return keys[keys.Count - 1];
        //    }
        //    finally
        //    {
        //        for (int i = 1; i < keys.Count - 1; i++)
        //        {
        //            keys[i].Close();
        //        }
        //    }
        //}

        private void recentCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRecentCommands();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowRecentCommands();
        }

        private void ShowRecentCommands()
        {
            RecentCommands rcnt = new RecentCommands();
            rcnt.FormClosing += (a, b) => functions.Text = rcnt.SelectedCommand;
            rcnt.StartPosition = FormStartPosition.CenterParent;
            rcnt.ShowDialog(this);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpCenter hc = new HelpCenter();
            hc.FormClosing += (a, b) => { if (!String.IsNullOrEmpty(hc.ChosenOne)) { functions.Text = hc.ChosenOne; functions.Focus(); } };
            hc.StartPosition = FormStartPosition.CenterParent;
            hc.ShowDialog(this);
        }
        
        void AboutClippyToolStripMenuItemClick(object sender, System.EventArgs e)
        {
        	About abt = new About();
        	abt.StartPosition = FormStartPosition.CenterParent;
        	abt.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        
        void FuncKeyUp(object sender, KeyEventArgs e)
        {
        	if(e.KeyCode == Keys.Enter)
        	{
        		e.Handled=true;
        		FunctionOnLeave(sender, EventArgs.Empty);
        		executeButton.PerformClick();
        	}
        }
    }
    //UserActivityHook actHook;
    //        void MainFormLoad(object sender, System.EventArgs e)
    //        {
    //#if Release
    //            actHook = new UserActivityHook(); // create an instance with global hooks
    //            // hang on events
    //            actHook.KeyDown += new KeyEventHandler(MyKeyDown);
    //            actHook.KeyUp += new KeyEventHandler(MyKeyUp);

    //            actHook.Start();
    //#endif
    //        }

    //public void MyKeyDown(object sender, KeyEventArgs e)
    //    {
    //        if (e.KeyCode == (Keys.RButton | Keys.Space | Keys.F17)) IsCtrlDown = true;
    //        if (e.KeyCode == (Keys.MButton | Keys.Space | Keys.F17)) IsAltDown = true;
    //        if (e.KeyCode == Keys.RWin || e.KeyCode == Keys.LWin) IsWinDown = true;
    //    }


    //    private bool IsCtrlDown = false;
    //    private bool IsAltDown = false;
    //    private bool IsWinDown = false;

    //    public void MyKeyUp(object sender, KeyEventArgs e)
    //    {
    //        if (e.KeyCode == (Keys.RButton | Keys.Space | Keys.F17)) IsCtrlDown = false;
    //        if (e.KeyCode == (Keys.MButton | Keys.Space | Keys.F17)) IsAltDown = false;
    //        if (e.KeyCode == Keys.RWin || e.KeyCode == Keys.LWin) IsWinDown = false;

    //        if (e.KeyCode == Keys.C && IsWinDown)
    //        {
    //            RestoreForm();
    //        }
    //    }

}
