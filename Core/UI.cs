using UITestingFramework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;

namespace UITestingFramework.Utilities
{
    /// <summary>
    /// The base class for SequenceTests.
    /// </summary>
    public static class UI
    {
        #region Fields
        private static BindingFlags _accessLayer;
        private static object _view;
        #endregion

        internal static UITestEngine Instance
        {
            set
            {
                _accessLayer = (BindingFlags)value.AccessLayer;
                _view = value.UIView;
            }
        }

        #region Private Methods
        private static MethodInfo GetMethod(object src, string methodName)
        {
            if (src == null) return null;
            return src.GetType()?.GetMethod(methodName, _accessLayer) ?? null;
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// The click method for performing one off clicks on buttons
        /// </summary>
        /// <param name="buttonName">The name of the button to be clicked</param>
        /// <exception cref="ArgumentException">Thrown if the button name is null or whitespace</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testin form is null</exception>
        /// <exception cref="MissingMethodException">Thrown if the click event method does not exist on the main testing form</exception>
        /// <exception cref="InvalidOperationException">Thrown if the main testing object is not typeof form</exception>
        public static void Click(string buttonName)
        {
            if (string.IsNullOrWhiteSpace(buttonName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(buttonName));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            MethodInfo method = GetMethod(_view, buttonName + "_Click");
            if (method is null)
            {
                throw new MissingMethodException($"The OnClick event for {buttonName} does not exist on {_view?.GetType()?.Name}.");
            }

            if (!(_view is Form form))
            {
                throw new InvalidOperationException(nameof(_view));
            }

            if (!(_view as Form).InvokeRequired)
            {
                method.Invoke(_view, new object[method.GetParameters().Length]);
            }
            else
            {
                (_view as Form).Invoke(new Action(() =>
                {
                    method.Invoke(_view, new object[method.GetParameters().Length]);
                }));
            }
        }
        /// <summary>
        /// The click more method for performing clicks on a collection of buttons with a defined interval delay
        /// </summary>
        /// <param name="buttonList">The collection of button names</param>
        /// <param name="intervalBetweenClicks">The time delay between sequential clicks</param>
        /// <exception cref="NullReferenceException">Thrown if the button list is null</exception>
        /// <exception cref="ArgumentException">Thrown if the button list is empty</exception>
        /// <exception cref="MissingMethodException">Thrown if the click event method for one of the buttons does not exist</exception>
        /// <exception cref="InvalidOperationException">Thrown if the main testing object is not typeof form</exception>
        public static void ClickMore(List<string> buttonList, int intervalBetweenClicks)
        {
            if (buttonList is null)
            {
                throw new NullReferenceException(nameof(buttonList));
            }

            if (buttonList.Count <= 0)
            {
                throw new ArgumentException("Argument cannot be empty.", nameof(buttonList));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            foreach (var item in buttonList)
            {
                MethodInfo method = GetMethod(_view, item + "_Click");
                if (method is null)
                {
                    throw new MissingMethodException($"The OnClick event for {item} does not exist on {_view?.GetType()?.Name}.");
                }

                if (!(_view is Form form))
                {
                    throw new InvalidOperationException(nameof(_view));
                }

                if (!(_view as Form).InvokeRequired)
                {
                    method.Invoke(_view, new object[method.GetParameters().Length]);
                }
                else
                {
                    (_view as Form).Invoke(new Action(() =>
                    {
                        method.Invoke(_view, new object[method.GetParameters().Length]);
                    }));
                }

                Thread.Sleep(intervalBetweenClicks);
            }
        }
        /// <summary>
        /// Delay the test execution by a given interval
        /// </summary>
        /// <param name="intervalBetweenClicks">The amount of miliseconds to delay the execution for (default: 1000ms)</param>
        public static void Wait(int intervalBetweenClicks = 1000)
        {
            Thread.Sleep(intervalBetweenClicks);
        }
        #endregion
    }
}