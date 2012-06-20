namespace Installer
{
    partial class Form1
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
            this.button3 = new System.Windows.Forms.Button();
            this.saveToPath = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.udfFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.installFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(245, 124);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(125, 26);
            this.button3.TabIndex = 15;
            this.button3.Text = "Install";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // saveToPath
            // 
            this.saveToPath.AutoSize = true;
            this.saveToPath.Location = new System.Drawing.Point(6, 93);
            this.saveToPath.Name = "saveToPath";
            this.saveToPath.Size = new System.Drawing.Size(227, 17);
            this.saveToPath.TabIndex = 14;
            this.saveToPath.Text = "Include clippy in path environment variable";
            this.saveToPath.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(295, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Functions file and Snippets file location";
            // 
            // udfFolder
            // 
            this.udfFolder.Location = new System.Drawing.Point(6, 66);
            this.udfFolder.Name = "udfFolder";
            this.udfFolder.Size = new System.Drawing.Size(285, 20);
            this.udfFolder.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // installFolder
            // 
            this.installFolder.Location = new System.Drawing.Point(6, 26);
            this.installFolder.Name = "installFolder";
            this.installFolder.Size = new System.Drawing.Size(285, 20);
            this.installFolder.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Where to save clippy:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 165);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.saveToPath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.udfFolder);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.installFolder);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Install Clippy";
            this.Load += new System.EventHandler(this.Screen1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox saveToPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox udfFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox installFolder;
        private System.Windows.Forms.Label label1;

    }
}

