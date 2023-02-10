using UITestingFramework.Controllers;
using UITestingFramework.Models;
using UITestingFramework.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace UITestingFramework.Core
{
    /// <summary>
    /// The abstract base class for running sequence tests.
    /// </summary>
    public class UITestEngine
    {
        #region Properties
        internal Encapsulation AccessLayer { get; set; }
        internal object UIView { get; set; }
        internal Assembly UIAssembly { get; set; }
        #endregion

        #region Public Methods
        public UITestEngine(Assembly assembly, object form)
        {
            UIAssembly = assembly;
            UIView = form;
            AccessLayer = Encapsulation.All;
        }

        public void RunTestConsole(ConcurrentDictionary<ITester, List<object>> tests)
        {
            if (tests.Count <= 0)
                return;

            UI.Instance = this;
            ToolKit.Instance = this;
            Entity.Instance = this;

            UITestConsole.InitializeTestingConsole(tests);
        }

        public void RunTraceConsole(IMessageFilter tracer)
        {
            UITestConsole.InitializeTracingConsole(tracer);
        }

        public void RunTestUI(List<object> tests)
        {
            new Thread(()=>
            {
                var view = new UITester();
                view.ShowDialog();
                var controller = new UITesterController(view);
                var testList = new List<TreeNode>();
                int index = 0;
                foreach (var item in tests)
                {
                    testList.Add(new UITest(index++) { Name = "New Test" });

                }
                controller.Load(testList);
                
            }).Start();
        }
        #endregion
    }
}