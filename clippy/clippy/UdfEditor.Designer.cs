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
            this.fxDescription.TabIndex = 2;
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
            this.fxCommands.Location = new System.Drawing.Point(13, 109);
            this.fxCommands.Multiline = true;
            this.fxCommands.Name = "fxCommands";
            this.fxCommands.Size = new System.Drawing.Size(386, 139);
            this.fxCommands.TabIndex = 4;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(324, 254);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
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
            this.loadMruButton.Location = new System.Drawing.Point(16, 254);
            this.loadMruButton.Name = "loadMruButton";
            this.loadMruButton.Size = new System.Drawing.Size(111, 23);
            this.loadMruButton.TabIndex = 7;
            this.loadMruButton.Text = "Load from Recent";
            this.loadMruButton.UseVisualStyleBackColor = true;
            this.loadMruButton.Click += new System.EventHandler(this.loadMruButton_Click);
            // 
            // UdfEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 283);
            this.Controls.Add(this.loadMruButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.fxCommands);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fxDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.functionList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UdfEditor";
            this.Text = "Function Editor";
            this.Load += new System.EventHandler(this.UdfEditor_Load);
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
    }
}