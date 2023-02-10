using UITestingFramework.Models;
using UITestingFramework.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework
{
    public partial class UITester : Form, IView
    {
        public UITester()
        {
            InitializeComponent();
        }

        public void LoadTestData(List<TreeNode> tests)
        {
            tree_TestData.Nodes.AddRange(tests.ToArray());
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);

                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
            }

            base.WndProc(ref m);
        }
    }
}
