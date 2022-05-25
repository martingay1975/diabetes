namespace UploadCarbsAndInsulin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtBefore = new System.Windows.Forms.TextBox();
            this.txtAfter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkOldData = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(204, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 94);
            this.button1.TabIndex = 0;
            this.button1.Text = "Go....";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Open Diasend CSV file";
            // 
            // txtBefore
            // 
            this.txtBefore.Location = new System.Drawing.Point(12, 128);
            this.txtBefore.Multiline = true;
            this.txtBefore.Name = "txtBefore";
            this.txtBefore.Size = new System.Drawing.Size(334, 119);
            this.txtBefore.TabIndex = 1;
            // 
            // txtAfter
            // 
            this.txtAfter.Location = new System.Drawing.Point(12, 271);
            this.txtAfter.Multiline = true;
            this.txtAfter.Name = "txtAfter";
            this.txtAfter.Size = new System.Drawing.Size(334, 119);
            this.txtAfter.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "After:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Before:";
            // 
            // chkOldData
            // 
            this.chkOldData.AutoSize = true;
            this.chkOldData.Location = new System.Drawing.Point(16, 20);
            this.chkOldData.Name = "chkOldData";
            this.chkOldData.Size = new System.Drawing.Size(179, 24);
            this.chkOldData.TabIndex = 5;
            this.chkOldData.Text = "Ignore errors, old data";
            this.chkOldData.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 414);
            this.Controls.Add(this.chkOldData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAfter);
            this.Controls.Add(this.txtBefore);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private Button button1;
		private OpenFileDialog openFileDialog1;
		private TextBox txtBefore;
		private TextBox txtAfter;
		private Label label1;
		private Label label2;
		private CheckBox chkOldData;
	}
}
