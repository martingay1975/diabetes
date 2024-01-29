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
			this.button1 = new Button();
			this.openFileDialog1 = new OpenFileDialog();
			this.txtBefore = new TextBox();
			this.txtAfter = new TextBox();
			this.label1 = new Label();
			this.label2 = new Label();
			this.chkOldData = new CheckBox();
			this.progressBar1 = new ProgressBar();
			this.folderBrowserDialog1 = new FolderBrowserDialog();
			SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new Point(178, 9);
			this.button1.Margin = new Padding(3, 2, 3, 2);
			this.button1.Name = "button1";
			this.button1.Size = new Size(124, 70);
			this.button1.TabIndex = 0;
			this.button1.Text = "Go....";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += button1_Click;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "Open Diasend CSV file";
			// 
			// txtBefore
			// 
			this.txtBefore.Location = new Point(10, 96);
			this.txtBefore.Margin = new Padding(3, 2, 3, 2);
			this.txtBefore.Multiline = true;
			this.txtBefore.Name = "txtBefore";
			this.txtBefore.Size = new Size(293, 90);
			this.txtBefore.TabIndex = 1;
			// 
			// txtAfter
			// 
			this.txtAfter.Location = new Point(10, 203);
			this.txtAfter.Margin = new Padding(3, 2, 3, 2);
			this.txtAfter.Multiline = true;
			this.txtAfter.Name = "txtAfter";
			this.txtAfter.Size = new Size(293, 90);
			this.txtAfter.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new Point(10, 188);
			this.label1.Name = "label1";
			this.label1.Size = new Size(36, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "After:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new Point(10, 82);
			this.label2.Name = "label2";
			this.label2.Size = new Size(44, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "Before:";
			// 
			// chkOldData
			// 
			this.chkOldData.AutoSize = true;
			this.chkOldData.Location = new Point(14, 15);
			this.chkOldData.Margin = new Padding(3, 2, 3, 2);
			this.chkOldData.Name = "chkOldData";
			this.chkOldData.Size = new Size(142, 19);
			this.chkOldData.TabIndex = 5;
			this.chkOldData.Text = "Ignore errors, old data";
			this.chkOldData.UseVisualStyleBackColor = true;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new Point(14, 297);
			this.progressBar1.Margin = new Padding(3, 2, 3, 2);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new Size(289, 38);
			this.progressBar1.TabIndex = 6;
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.InitialDirectory = "C:\\Users\\mgay\\Downloads\\export_Martin Gay";
			this.folderBrowserDialog1.SelectedPath = "C:\\Users\\mgay\\Downloads\\export_Martin Gay";
			this.folderBrowserDialog1.ShowNewFolderButton = false;
			this.folderBrowserDialog1.UseDescriptionForTitle = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(361, 405);
			Controls.Add(this.progressBar1);
			Controls.Add(this.chkOldData);
			Controls.Add(this.label2);
			Controls.Add(this.label1);
			Controls.Add(this.txtAfter);
			Controls.Add(this.txtBefore);
			Controls.Add(this.button1);
			Margin = new Padding(3, 2, 3, 2);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button button1;
		private OpenFileDialog openFileDialog1;
		private TextBox txtBefore;
		private TextBox txtAfter;
		private Label label1;
		private Label label2;
		private CheckBox chkOldData;
		private ProgressBar progressBar1;
		private FolderBrowserDialog folderBrowserDialog1;
	}
}
