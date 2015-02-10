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
                    _udfDocument = new XmlDocument();

                    RegistryKey hkcu = Registry.CurrentUser;
                    RegistryKey rkUdfLocation = hkcu.OpenSubKey("Software\\Rikard\\Clippy", false);
                    
                    if(rkUdfLocation == null)
                    {
                    	_udfDocument.LoadXml("<commands />");
                    	MessageBox.Show("Your User Functions file location has not been set.\r\nOpen Tools > Options to set this file location","Error finding UDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    	Close();
                    	return _udfDocument;
                    }
                    
                    object udfLocation = rkUdfLocation.GetValue("udfLocation");
                    
                    if(udfLocation == null)
                    {
                    	_udfDocument.LoadXml("<commands />");
                    	MessageBox.Show("Your User Functions file location has not been set.\r\nOpen Tools > Options to set this file location","Error finding UDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    	Close();
                    	return _udfDocument;
                    }
                    
                    if(!File.Exists(udfLocation.ToString()))
                    {
                    	using(FileStream fs = File.Create(udfLocation.ToString()))
                    	{
                    		using(StreamWriter sw = new StreamWriter(fs))
                    			sw.Write("<commands />");
                    	}
                    }
                    
                    _udfDocument.Load(udfLocation.ToString());
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

            XmlNodeList parms = passedInFunc.SelectNodes("parameter");
            udParms.Rows.Clear();
            foreach (XmlNode prm in parms)
            {
                string pnm = prm.Attributes["name"].Value;
                string defval = prm.Attributes["default"] == null ? String.Empty : prm.Attributes["default"].Value;
                bool req = prm.Attributes["required"] == null ? false : Boolean.Parse(prm.Attributes["required"].Value);
                udParms.Rows.Add(pnm, defval, req);
            }
            CommandListLeave(fxCommands, EventArgs.Empty);   
        }

        private void SaveButtonClick(object sender, EventArgs e)
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

                while (command.SelectSingleNode("parameter") != null)
                    command.RemoveChild(command.SelectSingleNode("parameter"));

            }

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

                        XmlNode paramnd = udfs.CreateElement("parameter");
                        XmlAttribute pnm = udfs.CreateAttribute("name");
                        pnm.Value = nm;
                        paramnd.Attributes.Append(pnm);
                        XmlAttribute pdv = udfs.CreateAttribute("default");
                        pdv.Value = dv;
                        paramnd.Attributes.Append(pdv);
                        XmlAttribute prq = udfs.CreateAttribute("required");
                        prq.Value = req.ToString();
                        paramnd.Attributes.Append(prq);
                        XmlAttribute pdsc = udfs.CreateAttribute("parmdesc");
                        paramnd.Attributes.Append(pdsc);
                        XmlAttribute pseq = udfs.CreateAttribute("sequence");
                        pseq.Value = seq.ToString();
                        paramnd.Attributes.Append(pseq);
                        seq++;
                        command.AppendChild(paramnd);
                    }
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
