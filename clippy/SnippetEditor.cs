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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClippyLib;
using ClippyLib.Settings;
using ClippyLib.Editors;

namespace clippy
{
    public partial class SnippetEditor : Form
    {
		internal SnippetsList Snippets;

        public SnippetEditor()
        {
			Snippets = new SnippetsList();
            InitializeComponent();
        }

        private void SnippetEditor_Load(object sender, EventArgs e)
        {
            LoadSnippets();
        }

        private void LoadSnippets()
        {
			snippetList.DataSource = (from Snippet s in Snippets
			                          orderby s.Name
			                          select s.Name).ToList();
        }

        private void snippetList_SelectedIndexChanged(object sender, EventArgs e)
        {
			if(Snippets.SnippetExists(snippetList.Text))
			{
				Snippet snip = Snippets.GetSnippet(snippetList.Text);
				snippetDescription.Text = snip.Description;
				snippetContent.Text = snip.Content;
				return;
			}
			
			snippetDescription.Text = string.Empty;
			snippetContent.Text = string.Empty;
			snippetContent.Text = string.Empty;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
			if(Snippets.SnippetExists(snippetList.Text))
			{
				var snip = Snippets.FirstOrDefault(s => s.Name.Equals(snippetList.Text, StringComparison.CurrentCultureIgnoreCase));
				snip.Content = snippetContent.Text;
				snip.Description = snippetDescription.Text;
			}
			else
			{
				Snippets.Add(new Snippet(snippetList.Text, snippetDescription.Text, snippetContent.Text));
			}
			Snippets.Save();
			this.Close();
        }

        private void DeleteSnippet(string snippetName)
        {
			Snippets.Remove(Snippets.FirstOrDefault(s => s.Name.Equals(snippetName, StringComparison.CurrentCultureIgnoreCase)));
			Snippets.Save();
			LoadSnippets();            
        }

        private void deleter_Click(object sender, EventArgs e)
        {
            DialogResult candelete = MessageBox.Show(String.Format("Are you sure you wish to delete " + snippetList.Text), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (candelete == DialogResult.Yes)
            {
                DeleteSnippet(snippetList.Text);
            }
        }
    }
}
