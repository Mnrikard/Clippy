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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;
using ClippyLib;
using ClippyLib.Editors;

namespace clippy
{
    public partial class UdfEditor : Form
    {
		private UserFunctionsList _functions;
        public UdfEditor()
        {
			_functions = new UserFunctionsList();
            InitializeComponent();
        }

        private void UdfEditor_Load(object sender, EventArgs e)
        {
            LoadFunctions();
        }

        private void LoadFunctions()
        {
			functionList.DataSource = (from UserFunction func in _functions
			                           orderby func.Name
			                           select func.Name).ToList();
        }

        private void functionList_SelectedIndexChanged(object sender, EventArgs e)
		{

			if(_functions.CommandExists(functionList.Text))
			{
				UserFunction func = _functions.GetUserFunction(functionList.Text);
				fxDescription.Text = func.Description;

				StringBuilder subFunctions = new StringBuilder();
				foreach(string subfunc in func.SubFunctions)
				{
					subFunctions.AppendLine(subfunc);
				}
				fxCommands.Text = subFunctions.ToString();

				foreach (var param in func.Parameters)
				{
					udParms.Rows.Add(param.Name, param.DefaultValue, param.Required);
				}
				CommandListLeave(fxCommands, EventArgs.Empty);
				return;
			}

			fxCommands.Text = String.Empty;
			fxDescription.Text = String.Empty;
			udParms.Rows.Clear();            
        }

        private void SaveButtonClick(object sender, EventArgs e)
		{	
			var parmlist = new List<UserFunction.UserParameter>();
			if (_containsParms)
			{
				int seq = 1;
				foreach (DataGridViewRow dr in udParms.Rows)
				{
					if (dr.Cells["ParmName"] != null && dr.Cells["ParmName"].Value != null && !String.IsNullOrEmpty(dr.Cells["ParmName"].Value.ToString()))
					{
						string nm = dr.Cells["ParmName"].Value.ToString();
						string dv = dr.Cells["defval"] == null || dr.Cells["defval"].Value == null ? String.Empty : dr.Cells["defval"].Value.ToString();
						bool req = dr.Cells["Required"] == null || dr.Cells["Required"].Value == null ? false : (bool)dr.Cells["Required"].Value;

						parmlist.Add(new UserFunction.UserParameter(nm, dv, req, string.Empty, seq));
						seq++;
					}
				}
			}

			UserFunction func = _functions.GetUserFunction(functionList.Text);

			if(func == null)
			{
				_functions.Add(new UserFunction(functionList.Text, fxDescription.Text, fxCommands.Text, parmlist));
			}
			else
			{
				func.Description = fxDescription.Text;
				func.SubFunctions.Clear();
				foreach(string subfunction in fxCommands.Text.Split('\n'))
				{
					func.SubFunctions.Enqueue(subfunction.Trim());
				}
				func.Parameters = parmlist;
			}

			_functions.Save();
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
			UserFunction func = _functions.GetUserFunction(functionName);
			if(func == null)
				return;

			_functions.Remove(func);
			_functions.Save();

			LoadFunctions();            
        }

        private void deleter_Click(object sender, EventArgs e)
        {
            DialogResult candelete = MessageBox.Show(String.Format("Are you sure you wish to delete " + functionList.Text), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (candelete == DialogResult.Yes)
            {
                DeleteUdf(functionList.Text);
            }
        }

        private bool _containsParms = false;
        private void CommandListLeave(object sender, EventArgs e)
        {
            if (Regex.IsMatch(fxCommands.Text, @"%\d+%"))
            {                
                _containsParms = true;
                if (splitContainer1.Panel2Collapsed)
                {
                    splitContainer1.Panel2Collapsed = false;
                }
            }
            else
            {
                _containsParms = false;
                if (!splitContainer1.Panel2Collapsed)
                {
                    splitContainer1.Panel2Collapsed = true;
                }
            }
        }


    }
}
