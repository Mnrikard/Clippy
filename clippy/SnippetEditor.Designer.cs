namespace clippy
{
    partial class SnippetEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnippetEditor));
            this.snippetContent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.snippetDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.snippetList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.deleter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // snippetContent
            // 
            this.snippetContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.snippetContent.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.snippetContent.Location = new System.Drawing.Point(8, 115);
            this.snippetContent.Multiline = true;
            this.snippetContent.Name = "snippetContent";
            this.snippetContent.Size = new System.Drawing.Size(400, 152);
            this.snippetContent.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Description";
            // 
            // snippetDescription
            // 
            this.snippetDescription.Location = new System.Drawing.Point(8, 69);
            this.snippetDescription.Name = "snippetDescription";
            this.snippetDescription.Size = new System.Drawing.Size(232, 20);
            this.snippetDescription.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Snippet Name";
            // 
            // snippetList
            // 
            this.snippetList.FormattingEnabled = true;
            this.snippetList.Location = new System.Drawing.Point(8, 29);
            this.snippetList.Name = "snippetList";
            this.snippetList.Size = new System.Drawing.Size(232, 21);
            this.snippetList.TabIndex = 5;
            this.snippetList.SelectedIndexChanged += new System.EventHandler(this.snippetList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Snippet";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(325, 274);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // deleter
            // 
            this.deleter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleter.BackColor = System.Drawing.Color.Firebrick;
            this.deleter.Location = new System.Drawing.Point(10, 273);
            this.deleter.Name = "deleter";
            this.deleter.Size = new System.Drawing.Size(75, 23);
            this.deleter.TabIndex = 12;
            this.deleter.Text = "Delete";
            this.deleter.UseVisualStyleBackColor = false;
            this.deleter.Click += new System.EventHandler(this.deleter_Click);
            // 
            // SnippetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 297);
            this.Controls.Add(this.deleter);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.snippetContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.snippetDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.snippetList);
			this.Icon = new System.Drawing.Icon(ClippyLib.Extensions.GetLocalFile("clippy2.ico"));
            this.Name = "SnippetEditor";
            this.Text = "SnippetEditor";
            this.Load += new System.EventHandler(this.SnippetEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox snippetContent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox snippetDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox snippetList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button deleter;

    }
}