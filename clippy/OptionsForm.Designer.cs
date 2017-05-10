namespace clippy
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
        	this.label3 = new System.Windows.Forms.Label();
        	this.hideAtX = new System.Windows.Forms.RadioButton();
        	this.closeAtX = new System.Windows.Forms.RadioButton();
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
        	this.saveButton.Location = new System.Drawing.Point(7, 132);
        	this.saveButton.Name = "saveButton";
        	this.saveButton.Size = new System.Drawing.Size(75, 23);
        	this.saveButton.TabIndex = 9;
        	this.saveButton.Text = "Save";
        	this.saveButton.UseVisualStyleBackColor = true;
        	this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
        	// 
        	// label3
        	// 
        	this.label3.Location = new System.Drawing.Point(7, 85);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(135, 23);
        	this.label3.TabIndex = 7;
        	this.label3.Text = "Close button function";
        	// 
        	// hideAtX
        	// 
        	this.hideAtX.Location = new System.Drawing.Point(7, 102);
        	this.hideAtX.Name = "hideAtX";
        	this.hideAtX.Size = new System.Drawing.Size(50, 24);
        	this.hideAtX.TabIndex = 6;
        	this.hideAtX.TabStop = true;
        	this.hideAtX.Text = "Hide";
        	this.hideAtX.UseVisualStyleBackColor = true;
        	// 
        	// closeAtX
        	// 
        	this.closeAtX.Location = new System.Drawing.Point(63, 102);
        	this.closeAtX.Name = "closeAtX";
        	this.closeAtX.Size = new System.Drawing.Size(54, 24);
        	this.closeAtX.TabIndex = 7;
        	this.closeAtX.TabStop = true;
        	this.closeAtX.Text = "Close";
        	this.closeAtX.UseVisualStyleBackColor = true;
			//
			// tabStringLabel
			//
			this.tabStringLabel = new System.Windows.Forms.Label();
			this.tabStringLabel.Location = new System.Drawing.Point(150,85);
			this.tabStringLabel.Name = "tabStringLabel";
			this.tabStringLabel.Size = new System.Drawing.Size(135,23);
			this.tabStringLabel.Text = "Tab String";
			//
			// tabStringBox
			//
			this.tabStringBox = new System.Windows.Forms.TextBox();
			this.tabStringBox.Name = "tabStringBox";
			this.tabStringBox.Location = new System.Drawing.Point(150,102);
			this.tabStringBox.Size = new System.Drawing.Size(100,24);
			this.tabStringBox.TabIndex = 8;
			this.tabStringBox.TabStop = true;
        	// 
        	// OptionsForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(477, 163);
        	this.Controls.Add(this.closeAtX);
        	this.Controls.Add(this.hideAtX);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.saveButton);
        	this.Controls.Add(this.browseSnippets);
        	this.Controls.Add(this.snippetsLocation);
        	this.Controls.Add(this.udfBrowse);
        	this.Controls.Add(this.udfLocation);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.label1);
			this.Controls.Add(this.tabStringBox);
			this.Controls.Add(this.tabStringLabel);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = new System.Drawing.Icon(ClippyLib.Extensions.GetLocalFile("clippy2.ico"));
        	this.MaximizeBox = false;
        	this.MaximumSize = new System.Drawing.Size(483, 347);
        	this.MinimizeBox = false;
        	this.MinimumSize = new System.Drawing.Size(483, 147);
        	this.Name = "OptionsForm";
        	this.Text = "Options";
        	this.Load += new System.EventHandler(this.OptionsForm_Load);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.RadioButton closeAtX;
        private System.Windows.Forms.RadioButton hideAtX;
        private System.Windows.Forms.Label label3;

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox udfLocation;
        private System.Windows.Forms.Button udfBrowse;
        private System.Windows.Forms.Button browseSnippets;
        private System.Windows.Forms.TextBox snippetsLocation;
        private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Label tabStringLabel;
		private System.Windows.Forms.TextBox tabStringBox;
    }
}