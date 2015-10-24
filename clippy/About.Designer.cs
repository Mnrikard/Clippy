﻿/*
 * Created by SharpDevelop.
 * User: Matthew
 * Date: 6/13/2013
 * Time: 12:00 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace clippy
{
	partial class About
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.aboutme = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// aboutme
			// 
			this.aboutme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.aboutme.Location = new System.Drawing.Point(2, 12);
			this.aboutme.Multiline = true;
			this.aboutme.Name = "aboutme";
			this.aboutme.ReadOnly = true;
			this.aboutme.Size = new System.Drawing.Size(387, 334);
			this.aboutme.TabIndex = 0;
			// 
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(391, 348);
			this.Controls.Add(this.aboutme);
			this.Name = "About";
			this.Text = "About Clippy";
			this.ResumeLayout(false);
			this.PerformLayout();
			this.Icon = new System.Drawing.Icon(ClippyLib.Extensions.GetLocalFile("clippy2.ico"));
		}
		private System.Windows.Forms.TextBox aboutme;
	}
}
