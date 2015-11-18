/*
 * 
 * Copyright 2012-2015 Matthew Rikard
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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using ClippyLib;
using ClippyLib.Editors;

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
                helpBox.Text = clipManager.Help(new string[] { String.Empty, editorName }).ToString();
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

        public Dictionary<string,string> GetFunctions()
        {
			UserFunctionsList flist = new UserFunctionsList();
			Dictionary<string,string> output = new Dictionary<string, string>();

			foreach (UserFunction uf in flist)
            {
				output.Add(uf.Name, uf.Description);
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
