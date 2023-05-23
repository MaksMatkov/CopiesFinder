using CopiesFinderNext.Service;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CopiesFinderNext
{
    public partial class CopiesFinderNext : Form
    {
        public CopiesFinderNext()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // dataGridView1.AutoGenerateColumns = false;
            // dataGridView1.Columns.Add("FileName", "File Name");
            // dataGridView1.Columns.Add("FilePath", "File Path");

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    lDirectory.Text = folderBrowserDialog.SelectedPath;
                    CopyFinder finder = new CopyFinder(folderBrowserDialog.SelectedPath);

                    //ShowPopup("Loading...");

                    this.Enabled = false;
                    finder.Find(ProgresBar);
                    this.Enabled = true;
                    ProgresBar.Value = 0;

                    //dataGridView1.Rows.Clear();

                    foreach (var group in finder._output.Where(el => el != null).GroupBy(el => el.Hash))
                    {
                        if (group.Count() < 2)
                            continue;

                        var groupNode = new TreeNode(group.First().Name.ToString() + $" = [{group.Count()}]");
                        treeView1.Nodes.Add(groupNode);

                        // Create child nodes for each item in the group
                        foreach (var item in group)
                        {
                            var itemNode = new TreeNode(item.Path);
                            groupNode.Nodes.Add(itemNode);
                        }
                    }
                }
            }
        }

        void ShowPopup(string text)
        {
            var loadingPopup = new Form { Width = 150, Height = 100 };
            var label = new Label { Left = 20, Top = 20, Text = text };
            loadingPopup.Controls.Add(label);
            loadingPopup.StartPosition = FormStartPosition.CenterScreen;
            loadingPopup.ControlBox = false;
            loadingPopup.Show();
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string path = e.Node.Text;

            if (e.Node.Nodes.Count == 0)
                Process.Start("explorer.exe", $"/select,\"{path}\"");
        }
    }
}