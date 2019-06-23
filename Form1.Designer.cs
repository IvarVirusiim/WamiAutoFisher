namespace AutoFisher
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
            this.scanButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(12, 43);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(131, 48);
            this.scanButton.TabIndex = 0;
            this.scanButton.Text = "Scan";
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(55, 13);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Starting....";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(149, 43);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(105, 48);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "STOP";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 103);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.scanButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button scanButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button stopButton;
    }
}

