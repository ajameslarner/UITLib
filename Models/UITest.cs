using UITestingFramework.Utilities;
using System.Windows.Forms;

namespace UITestingFramework.Models
{
    public class UITest : TreeNode, IModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TestCount { get; set; }
        public int Status { get; set; }
        public string Results { get; set; }
        public UITest(int iD)
        {
            ID = iD;
        }
        public UITest(string result)
        {
            Results = result;
        }
    }
}
