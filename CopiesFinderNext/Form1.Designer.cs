namespace CopiesFinderNext
{
    partial class CopiesFinderNext
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
            button1 = new Button();
            lDirectory = new Label();
            ProgresBar = new ProgressBar();
            treeView1 = new TreeView();
            label1 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 36);
            button1.Name = "button1";
            button1.Size = new Size(215, 42);
            button1.TabIndex = 1;
            button1.Text = "Select Directory";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // lDirectory
            // 
            lDirectory.AutoSize = true;
            lDirectory.Location = new Point(282, 47);
            lDirectory.Name = "lDirectory";
            lDirectory.Size = new Size(95, 20);
            lDirectory.TabIndex = 2;
            lDirectory.Text = "Not Selected";
            // 
            // ProgresBar
            // 
            ProgresBar.Location = new Point(12, 89);
            ProgresBar.Name = "ProgresBar";
            ProgresBar.Size = new Size(776, 29);
            ProgresBar.TabIndex = 3;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 134);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(776, 281);
            treeView1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 421);
            label1.Name = "label1";
            label1.Size = new Size(267, 20);
            label1.TabIndex = 5;
            label1.Text = "*Click on path to open it in File Exploer";
            // 
            // CopiesFinderNext
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(treeView1);
            Controls.Add(ProgresBar);
            Controls.Add(lDirectory);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "CopiesFinderNext";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CopiesFinderNext";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Label lDirectory;
        private ProgressBar ProgresBar;
        private TreeView treeView1;
        private Label label1;
    }
}