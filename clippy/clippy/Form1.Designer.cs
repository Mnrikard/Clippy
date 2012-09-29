namespace clippy
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.clipNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functions = new System.Windows.Forms.ComboBox();
            this.parametersGrid = new System.Windows.Forms.DataGridView();
            this.executeButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openUserFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSnippetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentCommandsImage = new System.Windows.Forms.PictureBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.notifyMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recentCommandsImage)).BeginInit();
            this.SuspendLayout();
            // 
            // clipNotify
            // 
            this.clipNotify.ContextMenuStrip = this.notifyMenu;
            this.clipNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("clipNotify.Icon")));
            this.clipNotify.Text = "Clippy";
            this.clipNotify.Visible = true;
            this.clipNotify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.clipNotify_MouseClick);
            // 
            // notifyMenu
            // 
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.notifyMenu.Name = "notifyMenu";
            this.notifyMenu.Size = new System.Drawing.Size(124, 48);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.restoreToolStripMenuItem.Text = "&Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // functions
            // 
            this.functions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.functions.FormattingEnabled = true;
            this.functions.Location = new System.Drawing.Point(13, 25);
            this.functions.Name = "functions";
            this.functions.Size = new System.Drawing.Size(316, 21);
            this.functions.TabIndex = 1;
            this.functions.Leave += new System.EventHandler(this.FunctionOnLeave);
            // 
            // parametersGrid
            // 
            this.parametersGrid.AllowUserToAddRows = false;
            this.parametersGrid.AllowUserToDeleteRows = false;
            this.parametersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.parametersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersGrid.Location = new System.Drawing.Point(13, 53);
            this.parametersGrid.Name = "parametersGrid";
            this.parametersGrid.Size = new System.Drawing.Size(344, 113);
            this.parametersGrid.TabIndex = 2;
            // 
            // executeButton
            // 
            this.executeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.executeButton.Location = new System.Drawing.Point(282, 175);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 23);
            this.executeButton.TabIndex = 3;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(369, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.openUserFunctionsToolStripMenuItem,
            this.openSnippetsToolStripMenuItem,
            this.recentCommandsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // openUserFunctionsToolStripMenuItem
            // 
            this.openUserFunctionsToolStripMenuItem.Name = "openUserFunctionsToolStripMenuItem";
            this.openUserFunctionsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openUserFunctionsToolStripMenuItem.Text = "Open &User Functions";
            this.openUserFunctionsToolStripMenuItem.Click += new System.EventHandler(this.openUserFunctionsToolStripMenuItem_Click);
            // 
            // openSnippetsToolStripMenuItem
            // 
            this.openSnippetsToolStripMenuItem.Name = "openSnippetsToolStripMenuItem";
            this.openSnippetsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openSnippetsToolStripMenuItem.Text = "Open &Snippets";
            this.openSnippetsToolStripMenuItem.Click += new System.EventHandler(this.openSnippetsToolStripMenuItem_Click);
            // 
            // recentCommandsToolStripMenuItem
            // 
            this.recentCommandsToolStripMenuItem.Name = "recentCommandsToolStripMenuItem";
            this.recentCommandsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.recentCommandsToolStripMenuItem.Text = "&Recent Commands";
            this.recentCommandsToolStripMenuItem.Click += new System.EventHandler(this.recentCommandsToolStripMenuItem_Click);
            // 
            // recentCommandsImage
            // 
            this.recentCommandsImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recentCommandsImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.recentCommandsImage.Image = ((System.Drawing.Image)(resources.GetObject("recentCommandsImage.Image")));
            this.recentCommandsImage.Location = new System.Drawing.Point(336, 28);
            this.recentCommandsImage.Name = "recentCommandsImage";
            this.recentCommandsImage.Size = new System.Drawing.Size(21, 19);
            this.recentCommandsImage.TabIndex = 5;
            this.recentCommandsImage.TabStop = false;
            this.recentCommandsImage.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(13, 184);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 13);
            this.errorLabel.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 203);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.recentCommandsImage);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.parametersGrid);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.functions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Clippy";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.notifyMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.parametersGrid)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recentCommandsImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon clipNotify;
        private System.Windows.Forms.ContextMenuStrip notifyMenu;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ComboBox functions;
        private System.Windows.Forms.DataGridView parametersGrid;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openUserFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSnippetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentCommandsToolStripMenuItem;
        private System.Windows.Forms.PictureBox recentCommandsImage;
        private System.Windows.Forms.Label errorLabel;
    }
}

