using UITestingFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Utilities
{
    internal interface IView
    {
        void LoadTestData(List<TreeNode> tests);
    }
}
