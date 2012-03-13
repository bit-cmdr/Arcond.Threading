namespace PoolProof
{
	partial class Proof
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
			if (disposing && (components != null)) {
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
			this.lblMax = new System.Windows.Forms.Label();
			this.txtMax = new System.Windows.Forms.TextBox();
			this.grpThreads = new System.Windows.Forms.GroupBox();
			this.lblAvailThreads = new System.Windows.Forms.Label();
			this.lblRunningThreads = new System.Windows.Forms.Label();
			this.lblAvail = new System.Windows.Forms.Label();
			this.lblRunning = new System.Windows.Forms.Label();
			this.lblSpin = new System.Windows.Forms.Label();
			this.txtSpin = new System.Windows.Forms.TextBox();
			this.btnThread = new System.Windows.Forms.Button();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.grpThreads.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMax
			// 
			this.lblMax.AutoSize = true;
			this.lblMax.Location = new System.Drawing.Point(16, 9);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(69, 13);
			this.lblMax.TabIndex = 2;
			this.lblMax.Text = "Max Threads";
			// 
			// txtMax
			// 
			this.txtMax.Location = new System.Drawing.Point(19, 25);
			this.txtMax.Name = "txtMax";
			this.txtMax.Size = new System.Drawing.Size(100, 20);
			this.txtMax.TabIndex = 3;
			// 
			// grpThreads
			// 
			this.grpThreads.Controls.Add(this.lblAvailThreads);
			this.grpThreads.Controls.Add(this.lblRunningThreads);
			this.grpThreads.Controls.Add(this.lblAvail);
			this.grpThreads.Controls.Add(this.lblRunning);
			this.grpThreads.Location = new System.Drawing.Point(19, 100);
			this.grpThreads.Name = "grpThreads";
			this.grpThreads.Size = new System.Drawing.Size(126, 60);
			this.grpThreads.TabIndex = 4;
			this.grpThreads.TabStop = false;
			this.grpThreads.Text = "Threads";
			// 
			// lblAvailThreads
			// 
			this.lblAvailThreads.AutoSize = true;
			this.lblAvailThreads.Location = new System.Drawing.Point(64, 37);
			this.lblAvailThreads.Name = "lblAvailThreads";
			this.lblAvailThreads.Size = new System.Drawing.Size(13, 13);
			this.lblAvailThreads.TabIndex = 3;
			this.lblAvailThreads.Text = "0";
			// 
			// lblRunningThreads
			// 
			this.lblRunningThreads.AutoSize = true;
			this.lblRunningThreads.Location = new System.Drawing.Point(10, 37);
			this.lblRunningThreads.Name = "lblRunningThreads";
			this.lblRunningThreads.Size = new System.Drawing.Size(13, 13);
			this.lblRunningThreads.TabIndex = 2;
			this.lblRunningThreads.Text = "0";
			// 
			// lblAvail
			// 
			this.lblAvail.AutoSize = true;
			this.lblAvail.Location = new System.Drawing.Point(61, 20);
			this.lblAvail.Name = "lblAvail";
			this.lblAvail.Size = new System.Drawing.Size(50, 13);
			this.lblAvail.TabIndex = 1;
			this.lblAvail.Text = "Available";
			// 
			// lblRunning
			// 
			this.lblRunning.AutoSize = true;
			this.lblRunning.Location = new System.Drawing.Point(7, 20);
			this.lblRunning.Name = "lblRunning";
			this.lblRunning.Size = new System.Drawing.Size(47, 13);
			this.lblRunning.TabIndex = 0;
			this.lblRunning.Text = "Running";
			// 
			// lblSpin
			// 
			this.lblSpin.AutoSize = true;
			this.lblSpin.Location = new System.Drawing.Point(16, 58);
			this.lblSpin.Name = "lblSpin";
			this.lblSpin.Size = new System.Drawing.Size(82, 13);
			this.lblSpin.TabIndex = 5;
			this.lblSpin.Text = "Threads to Spin";
			// 
			// txtSpin
			// 
			this.txtSpin.Location = new System.Drawing.Point(19, 74);
			this.txtSpin.Name = "txtSpin";
			this.txtSpin.Size = new System.Drawing.Size(100, 20);
			this.txtSpin.TabIndex = 6;
			// 
			// btnThread
			// 
			this.btnThread.Location = new System.Drawing.Point(126, 9);
			this.btnThread.Name = "btnThread";
			this.btnThread.Size = new System.Drawing.Size(100, 91);
			this.btnThread.TabIndex = 7;
			this.btnThread.Text = "Fire ALL the Threads";
			this.btnThread.UseVisualStyleBackColor = true;
			this.btnThread.Click += new System.EventHandler(this.btnThread_Click);
			// 
			// txtOutput
			// 
			this.txtOutput.Location = new System.Drawing.Point(19, 176);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOutput.Size = new System.Drawing.Size(210, 229);
			this.txtOutput.TabIndex = 8;
			// 
			// Proof
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(241, 417);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.btnThread);
			this.Controls.Add(this.txtSpin);
			this.Controls.Add(this.lblSpin);
			this.Controls.Add(this.grpThreads);
			this.Controls.Add(this.txtMax);
			this.Controls.Add(this.lblMax);
			this.Name = "Proof";
			this.Text = "Pool Proof";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Proof_FormClosing);
			this.Load += new System.EventHandler(this.Proof_Load);
			this.grpThreads.ResumeLayout(false);
			this.grpThreads.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.TextBox txtMax;
		private System.Windows.Forms.GroupBox grpThreads;
		private System.Windows.Forms.Label lblAvail;
		private System.Windows.Forms.Label lblAvailThreads;
		private System.Windows.Forms.Label lblRunning;
		private System.Windows.Forms.Label lblRunningThreads;
		private System.Windows.Forms.Label lblSpin;
		private System.Windows.Forms.TextBox txtSpin;
		private System.Windows.Forms.Button btnThread;
		private System.Windows.Forms.TextBox txtOutput;
	}
}