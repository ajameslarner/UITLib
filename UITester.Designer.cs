namespace UITestingFramework
{
    partial class UITester
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node0");
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.box_UITester = new System.Windows.Forms.GroupBox();
            this.tree_TestData = new System.Windows.Forms.TreeView();
            this.box_UITester.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 415);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(776, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // box_UITester
            // 
            this.box_UITester.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.box_UITester.Controls.Add(this.tree_TestData);
            this.box_UITester.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.box_UITester.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.box_UITester.Location = new System.Drawing.Point(12, 12);
            this.box_UITester.Name = "box_UITester";
            this.box_UITester.Size = new System.Drawing.Size(776, 397);
            this.box_UITester.TabIndex = 1;
            this.box_UITester.TabStop = false;
            this.box_UITester.Text = "UI Tests";
            // 
            // tree_TestData
            // 
            this.tree_TestData.Location = new System.Drawing.Point(6, 20);
            this.tree_TestData.Name = "tree_TestData";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Node0";
            this.tree_TestData.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tree_TestData.Size = new System.Drawing.Size(764, 371);
            this.tree_TestData.TabIndex = 0;
            // 
            // UITester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.box_UITester);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UITester";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UI Testing";
            this.TopMost = true;
            this.box_UITester.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox box_UITester;
        private System.Windows.Forms.TreeView tree_TestData;
    }
}