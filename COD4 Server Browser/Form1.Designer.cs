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
            btnSetPath = new Button();
            ofd = new OpenFileDialog();
            btnLaunch = new Button();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(6, 352);
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
            exListBox1.Location = new Point(6, 12);
            exListBox1.Name = "exListBox1";
            exListBox1.Size = new Size(573, 334);
            exListBox1.TabIndex = 2;
            // 
            // btnSetPath
            // 
            btnSetPath.Location = new Point(369, 352);
            btnSetPath.Name = "btnSetPath";
            btnSetPath.Size = new Size(102, 30);
            btnSetPath.TabIndex = 3;
            btnSetPath.Text = "COD4 EXE";
            btnSetPath.UseVisualStyleBackColor = true;
            btnSetPath.Click += btnSetPath_Click;
            // 
            // ofd
            // 
            ofd.FileName = "openFileDialog1";
            ofd.Filter = "COD4 EXE|iw3mp.exe";
            // 
            // btnLaunch
            // 
            btnLaunch.Location = new Point(477, 352);
            btnLaunch.Name = "btnLaunch";
            btnLaunch.Size = new Size(102, 30);
            btnLaunch.TabIndex = 4;
            btnLaunch.Text = "Launch";
            btnLaunch.UseVisualStyleBackColor = true;
            btnLaunch.Click += btnLaunch_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(87, 367);
            progressBar1.MarqueeAnimationSpeed = 20;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 11);
            progressBar1.Step = 5;
            progressBar1.TabIndex = 5;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(586, 390);
            Controls.Add(progressBar1);
            Controls.Add(btnLaunch);
            Controls.Add(btnSetPath);
            Controls.Add(exListBox1);
            Controls.Add(btnRefresh);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "COD4 Server Browser";
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnRefresh;
        private exListBox exListBox1;
        private Button btnSetPath;
        private OpenFileDialog ofd;
        private Button btnLaunch;
        private ProgressBar progressBar1;
    }
}