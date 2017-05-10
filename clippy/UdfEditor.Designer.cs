namespace clippy
{
    partial class UdfEditor
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UdfEditor));
        	this.functionList = new System.Windows.Forms.ComboBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.fxDescription = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.fxCommands = new System.Windows.Forms.TextBox();
        	this.saveButton = new System.Windows.Forms.Button();
        	this.label3 = new System.Windows.Forms.Label();
        	this.loadMruButton = new System.Windows.Forms.Button();
        	this.deleter = new System.Windows.Forms.Button();
        	this.udParms = new System.Windows.Forms.DataGridView();
        	this.ParmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.defval = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        	this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        	((System.ComponentModel.ISupportInitialize)(this.udParms)).BeginInit();
        	this.splitContainer1.Panel1.SuspendLayout();
        	this.splitContainer1.Panel2.SuspendLayout();
        	this.splitContainer1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// functionList
        	// 
        	this.functionList.FormattingEnabled = true;
        	this.functionList.Location = new System.Drawing.Point(13, 23);
        	this.functionList.Name = "functionList";
        	this.functionList.Size = new System.Drawing.Size(232, 21);
        	this.functionList.TabIndex = 0;
        	this.functionList.SelectedIndexChanged += new System.EventHandler(this.functionList_SelectedIndexChanged);
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(13, 4);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(79, 13);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Function Name";
        	// 
        	// fxDescription
        	// 
        	this.fxDescription.Location = new System.Drawing.Point(13, 63);
        	this.fxDescription.Name = "fxDescription";
        	this.fxDescription.Size = new System.Drawing.Size(232, 20);
        	this.fxDescription.TabIndex = 1;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(12, 47);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(60, 13);
        	this.label2.TabIndex = 3;
        	this.label2.Text = "Description";
        	// 
        	// fxCommands
        	// 
        	this.fxCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.fxCommands.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.fxCommands.Location = new System.Drawing.Point(3, 3);
        	this.fxCommands.Multiline = true;
        	this.fxCommands.Name = "fxCommands";
        	this.fxCommands.Size = new System.Drawing.Size(394, 174);
        	this.fxCommands.TabIndex = 2;
        	this.fxCommands.Leave += new System.EventHandler(this.CommandListLeave);
        	// 
        	// saveButton
        	// 
        	this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.saveButton.Location = new System.Drawing.Point(329, 376);
        	this.saveButton.Name = "saveButton";
        	this.saveButton.Size = new System.Drawing.Size(75, 23);
        	this.saveButton.TabIndex = 6;
        	this.saveButton.Text = "Save";
        	this.saveButton.UseVisualStyleBackColor = true;
        	this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(16, 90);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(127, 13);
        	this.label3.TabIndex = 6;
        	this.label3.Text = "Function list (one per line)";
        	// 
        	// loadMruButton
        	// 
        	this.loadMruButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.loadMruButton.Location = new System.Drawing.Point(16, 376);
        	this.loadMruButton.Name = "loadMruButton";
        	this.loadMruButton.Size = new System.Drawing.Size(111, 23);
        	this.loadMruButton.TabIndex = 4;
        	this.loadMruButton.Text = "Load from Recent";
        	this.loadMruButton.UseVisualStyleBackColor = true;
        	this.loadMruButton.Click += new System.EventHandler(this.loadMruButton_Click);
        	// 
        	// deleter
        	// 
        	this.deleter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.deleter.BackColor = System.Drawing.Color.Firebrick;
        	this.deleter.Location = new System.Drawing.Point(248, 376);
        	this.deleter.Name = "deleter";
        	this.deleter.Size = new System.Drawing.Size(75, 23);
        	this.deleter.TabIndex = 5;
        	this.deleter.Text = "Delete";
        	this.deleter.UseVisualStyleBackColor = false;
        	this.deleter.Click += new System.EventHandler(this.deleter_Click);
        	// 
        	// udParms
        	// 
        	this.udParms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.udParms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.udParms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
        	        	        	this.ParmName,
        	        	        	this.defval,
        	        	        	this.Required});
        	this.udParms.Location = new System.Drawing.Point(7, 7);
        	this.udParms.Name = "udParms";
        	this.udParms.Size = new System.Drawing.Size(385, 70);
        	this.udParms.TabIndex = 3;
        	// 
        	// ParmName
        	// 
        	this.ParmName.HeaderText = "Parm Name";
        	this.ParmName.Name = "ParmName";
        	// 
        	// defval
        	// 
        	this.defval.HeaderText = "Default Value";
        	this.defval.Name = "defval";
        	// 
        	// Required
        	// 
        	this.Required.HeaderText = "Required";
        	this.Required.Name = "Required";
        	// 
        	// splitContainer1
        	// 
        	this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.splitContainer1.Location = new System.Drawing.Point(12, 106);
        	this.splitContainer1.Name = "splitContainer1";
        	this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
        	// 
        	// splitContainer1.Panel1
        	// 
        	this.splitContainer1.Panel1.Controls.Add(this.fxCommands);
        	// 
        	// splitContainer1.Panel2
        	// 
        	this.splitContainer1.Panel2.Controls.Add(this.udParms);
        	this.splitContainer1.Size = new System.Drawing.Size(400, 261);
        	this.splitContainer1.SplitterDistance = 180;
        	this.splitContainer1.TabIndex = 10;
        	// 
        	// UdfEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(416, 405);
        	this.Controls.Add(this.splitContainer1);
        	this.Controls.Add(this.deleter);
        	this.Controls.Add(this.loadMruButton);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.saveButton);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.fxDescription);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.functionList);
			this.Icon = new System.Drawing.Icon(ClippyLib.Extensions.GetLocalFile("clippy2.ico"));
        	this.Name = "UdfEditor";
        	this.Text = "Function Editor";
        	this.Load += new System.EventHandler(this.UdfEditor_Load);
        	((System.ComponentModel.ISupportInitialize)(this.udParms)).EndInit();
        	this.splitContainer1.Panel1.ResumeLayout(false);
        	this.splitContainer1.Panel1.PerformLayout();
        	this.splitContainer1.Panel2.ResumeLayout(false);
        	this.splitContainer1.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox functionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fxDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox fxCommands;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadMruButton;
        private System.Windows.Forms.Button deleter;
        private System.Windows.Forms.DataGridView udParms;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn defval;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Required;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}