using UITestingFramework.Models;
using UITestingFramework.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Controllers
{
    internal class UITesterController
    {
        private IView _view;
        private IModel _model;

        public UITesterController(IView view, IModel model)
        {
            _view = view;
            _model = model;
        }

        public UITesterController(IView view)
        {
            _view = view;
        }

        public void Load(List<TreeNode> tests)
        {
            _view.LoadTestData(tests);
        }
    }
}
