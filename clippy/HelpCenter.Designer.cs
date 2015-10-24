namespace clippy
{
    partial class HelpCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpCenter));
            this.functionList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.helpBox = new System.Windows.Forms.TextBox();
            this.choicerBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // functionList
            // 
            this.functionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.functionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.functionList.FormattingEnabled = true;
            this.functionList.Location = new System.Drawing.Point(12, 26);
            this.functionList.Name = "functionList";
            this.functionList.Size = new System.Drawing.Size(301, 21);
            this.functionList.TabIndex = 0;
            this.functionList.SelectedIndexChanged += new System.EventHandler(this.FunctionListSelectedIndexChanged);
            this.functionList.Leave += new System.EventHandler(this.FunctionListLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose a function for help";
            // 
            // helpBox
            // 
            this.helpBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.helpBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpBox.Location = new System.Drawing.Point(12, 54);
            this.helpBox.Multiline = true;
            this.helpBox.Name = "helpBox";
            this.helpBox.ReadOnly = true;
            this.helpBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.helpBox.Size = new System.Drawing.Size(301, 209);
            this.helpBox.TabIndex = 2;
            // 
            // choicerBtn
            // 
            this.choicerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.choicerBtn.Location = new System.Drawing.Point(188, 269);
            this.choicerBtn.Name = "choicerBtn";
            this.choicerBtn.Size = new System.Drawing.Size(125, 23);
            this.choicerBtn.TabIndex = 3;
            this.choicerBtn.Text = "That\'s the one I want!";
            this.choicerBtn.UseVisualStyleBackColor = true;
            this.choicerBtn.Click += new System.EventHandler(this.choicerBtn_Click);
            // 
            // HelpCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 292);
            this.Controls.Add(this.choicerBtn);
            this.Controls.Add(this.helpBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.functionList);
			this.Icon = new System.Drawing.Icon(ClippyLib.Extensions.GetLocalFile("clippy2.ico"));
            this.Name = "HelpCenter";
            this.Text = "Function Descriptions";
            this.Load += new System.EventHandler(this.HelpCenterLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox functionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox helpBox;
        private System.Windows.Forms.Button choicerBtn;
    }
}