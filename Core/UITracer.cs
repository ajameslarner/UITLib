using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Core
{
    /// <summary>
    /// Sequence Tracing base class, implements <see cref="IMessageFilter"/>
    /// </summary>
    public class UITracer : IMessageFilter
    {
        private const int WM_KEYUP = 0x101;
        private const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// List of recorded steps to be exported.
        /// </summary>
        public List<string> RecordedSteps { get; set; } = new List<string>();
        /// <summary>
        /// Is the recording session running.
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        /// Catches any event messages before they are processed internal.
        /// </summary>
        /// <param name="m">Windows Message Class</param>
        /// <returns></returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP || (m.Msg == WM_KEYUP && ((int)m.WParam == 32 || (int)m.WParam == 13)))
            {
                var ctl = Control.FromHandle(m.HWnd);
                LogButtonClick(ctl);
            }
            return false;
        }

        private void LogButtonClick(Control ctrl)
        {
            WriteLog($"Click: {ctrl.Name}");
        }

        private void WriteLog(string message)
        {
            if (IsRecording)
            {
                RecordedSteps.Add(message);
            }
            Console.WriteLine(message);
        }
    }
}