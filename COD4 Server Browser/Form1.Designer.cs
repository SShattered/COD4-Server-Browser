namespace COD4_Server_Browser
{
    partial class MainForm
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
            btnRefresh = new Button();
            exListBox1 = new exListBox();
            SuspendLayout();
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(6, 407);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 30);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // exListBox1
            // 
            exListBox1.DrawMode = DrawMode.OwnerDrawVariable;
            exListBox1.FormattingEnabled = true;
            exListBox1.ItemHeight = 66;
            exListBox1.Location = new Point(6, 67);
            exListBox1.Name = "exListBox1";
            exListBox1.Size = new Size(729, 334);
            exListBox1.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(773, 446);
            Controls.Add(exListBox1);
            Controls.Add(btnRefresh);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Sizable = false;
            Text = "COD4 Server Browser";
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnRefresh;
        private exListBox exListBox1;
    }
}