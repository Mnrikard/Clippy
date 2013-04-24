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
            this.installButton = new System.Windows.Forms.Button();
            this.saveToPath = new System.Windows.Forms.CheckBox();
            this.browseUdfBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.udfFolder = new System.Windows.Forms.TextBox();
            this.browseSaveBtn = new System.Windows.Forms.Button();
            this.installFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.runAtStartup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // installButton
            // 
            this.installButton.Location = new System.Drawing.Point(245, 124);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(125, 26);
            this.installButton.TabIndex = 15;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.InstallClick);
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
            // browseUdfBtn
            // 
            this.browseUdfBtn.Location = new System.Drawing.Point(295, 66);
            this.browseUdfBtn.Name = "browseUdfBtn";
            this.browseUdfBtn.Size = new System.Drawing.Size(75, 23);
            this.browseUdfBtn.TabIndex = 13;
            this.browseUdfBtn.Text = "Browse";
            this.browseUdfBtn.UseVisualStyleBackColor = true;
            this.browseUdfBtn.Click += new System.EventHandler(this.BrowseForUdfLocation);
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
            // browseSaveBtn
            // 
            this.browseSaveBtn.Location = new System.Drawing.Point(295, 25);
            this.browseSaveBtn.Name = "browseSaveBtn";
            this.browseSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.browseSaveBtn.TabIndex = 10;
            this.browseSaveBtn.Text = "Browse";
            this.browseSaveBtn.UseVisualStyleBackColor = true;
            this.browseSaveBtn.Click += new System.EventHandler(this.BrowseForSaveLocation);
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
            // runAtStartup
            // 
            this.runAtStartup.AutoSize = true;
            this.runAtStartup.Location = new System.Drawing.Point(6, 117);
            this.runAtStartup.Name = "runAtStartup";
            this.runAtStartup.Size = new System.Drawing.Size(95, 17);
            this.runAtStartup.TabIndex = 16;
            this.runAtStartup.Text = "Run at Startup";
            this.runAtStartup.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 165);
            this.Controls.Add(this.runAtStartup);
            this.Controls.Add(this.installButton);
            this.Controls.Add(this.saveToPath);
            this.Controls.Add(this.browseUdfBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.udfFolder);
            this.Controls.Add(this.browseSaveBtn);
            this.Controls.Add(this.installFolder);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Install Clippy";
            this.Load += new System.EventHandler(this.ScreenLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button installButton;
        private System.Windows.Forms.CheckBox saveToPath;
        private System.Windows.Forms.Button browseUdfBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox udfFolder;
        private System.Windows.Forms.Button browseSaveBtn;
        private System.Windows.Forms.TextBox installFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox runAtStartup;

    }
}

