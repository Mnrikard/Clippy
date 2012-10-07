﻿namespace clippy
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.udfLocation = new System.Windows.Forms.TextBox();
            this.udfBrowse = new System.Windows.Forms.Button();
            this.browseSnippets = new System.Windows.Forms.Button();
            this.snippetsLocation = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Functions File";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Snippets File";
            // 
            // udfLocation
            // 
            this.udfLocation.Location = new System.Drawing.Point(7, 17);
            this.udfLocation.Name = "udfLocation";
            this.udfLocation.Size = new System.Drawing.Size(353, 20);
            this.udfLocation.TabIndex = 2;
            // 
            // udfBrowse
            // 
            this.udfBrowse.Location = new System.Drawing.Point(367, 15);
            this.udfBrowse.Name = "udfBrowse";
            this.udfBrowse.Size = new System.Drawing.Size(75, 23);
            this.udfBrowse.TabIndex = 3;
            this.udfBrowse.Text = "Browse";
            this.udfBrowse.UseVisualStyleBackColor = true;
            this.udfBrowse.Click += new System.EventHandler(this.udfBrowse_Click);
            // 
            // browseSnippets
            // 
            this.browseSnippets.Location = new System.Drawing.Point(367, 56);
            this.browseSnippets.Name = "browseSnippets";
            this.browseSnippets.Size = new System.Drawing.Size(75, 23);
            this.browseSnippets.TabIndex = 5;
            this.browseSnippets.Text = "Browse";
            this.browseSnippets.UseVisualStyleBackColor = true;
            this.browseSnippets.Click += new System.EventHandler(this.browseSnippets_Click);
            // 
            // snippetsLocation
            // 
            this.snippetsLocation.Location = new System.Drawing.Point(7, 58);
            this.snippetsLocation.Name = "snippetsLocation";
            this.snippetsLocation.Size = new System.Drawing.Size(353, 20);
            this.snippetsLocation.TabIndex = 4;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(7, 85);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 122);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.browseSnippets);
            this.Controls.Add(this.snippetsLocation);
            this.Controls.Add(this.udfBrowse);
            this.Controls.Add(this.udfLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(483, 147);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(483, 147);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox udfLocation;
        private System.Windows.Forms.Button udfBrowse;
        private System.Windows.Forms.Button browseSnippets;
        private System.Windows.Forms.TextBox snippetsLocation;
        private System.Windows.Forms.Button saveButton;
    }
}