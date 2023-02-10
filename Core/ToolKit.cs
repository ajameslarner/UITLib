using UITestingFramework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;

namespace UITestingFramework.Utilities
{
    /// <summary>
    /// The toolkit class for tweaking and peaking variables
    /// </summary>
    public static class ToolKit
    {
        #region Fields
        private static BindingFlags _accessLayer;
        private static Assembly _assembly;
        private static object _view;
        private static readonly List<string> _namespaces = new List<string>();
        #endregion

        internal static UITestEngine Instance
        {
            set
            {
                _accessLayer = (BindingFlags)value.AccessLayer;
                _assembly = value.UIAssembly;
                _view = value.UIView;
            }
        }

        #region Private Methods
        private static object GetPropertyValue(object src, string propName)
        {
            if (src == null) return null;
            return src.GetType()?.GetProperty(propName, _accessLayer)?.GetValue(src, null) ?? null;
        }
        private static void SetPropertyValue(object src, string propName, object newValue)
        {
            if (src == null) return;
            src.GetType()?.GetProperty(propName, _accessLayer)?.SetValue(src, newValue, null);
        }
        private static object GetFieldValue(object src, string fieldName)
        {
            if (src == null) return null;
            return src.GetType()?.GetField(fieldName, _accessLayer)?.GetValue(src) ?? null;
        }
        private static void SetFieldValue(object src, string fieldName, object newValue)
        {
            if (src == null) return;
            src.GetType()?.GetField(fieldName, _accessLayer)?.SetValue(src, newValue);
        }
        private static FieldInfo GetField(object src, string fieldName)
        {
            if (src == null) return null;
            return src.GetType()?.GetField(fieldName, _accessLayer) ?? null;
        }
        private static PropertyInfo GetProperty(object src, string propertyName)
        {
            if (src == null) return null;
            return src.GetType()?.GetProperty(propertyName, _accessLayer) ?? null;
        }
        private static dynamic GetControl(object src, string controlName)
        {
            if (src == null) return null;
            return GetField(src, controlName)?.GetValue(src) ?? null;
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// The tweak control method for modifying control properties
        /// </summary>
        /// <param name="controlName">The name of the control to be tweaked</param>
        /// <param name="propertyName">The name of the property to be tweaked</param>
        /// <param name="newValue">The new value of the property on the control to be set</param>
        /// <exception cref="ArgumentException">Thrown if the control name or property name is null or whitespace</exception>
        /// <exception cref="ArgumentNullException">Thrown if the new value is null</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingPropertyException">Thrown if the property does not exist on the control</exception>
        /// <exception cref="InvalidOperationException">Thrown if the main testing object is not typeof form</exception>
        public static void TweakControl(string controlName, string propertyName, object newValue)
        {
            if (string.IsNullOrWhiteSpace(controlName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(controlName));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(propertyName));
            }

            if (newValue is null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            dynamic ctrl = GetControl(_view, controlName);

            if (ctrl is null)
            {
                throw new MissingPropertyException($"The control {controlName} does not exist on {_view?.GetType()?.Name}.");
            }

            if (_view.GetType() != typeof(Form))
            {
                throw new InvalidOperationException(nameof(_view));
            }

            if (!(_view as Form).InvokeRequired)
            {
                SetPropertyValue(ctrl, propertyName, newValue);
            }
            else
            {
                (_view as Form).Invoke(new Action(() =>
                {
                    SetPropertyValue(ctrl, propertyName, newValue);
                }));
            }
        }
        /// <summary>
        /// The tweak property method for modifying property values
        /// </summary>
        /// <param name="propertyName">The name of the property to be tweaked</param>
        /// <param name="newValue">The new value of the property to be set</param>
        /// <exception cref="ArgumentException">Thrown if the property name is null or whitespace</exception>
        /// <exception cref="ArgumentNullException">Thrown if the new value is null</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingPropertyException">Thrown if the property does not exist on the main form</exception>
        /// <exception cref="InvalidOperationException">Thrown if the main testing object is not typeof form</exception>
        public static void TweakProperty(string propertyName, object newValue)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(propertyName));
            }

            if (newValue is null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            PropertyInfo property = GetProperty(_view, propertyName);

            if (property is null)
            {
                throw new MissingPropertyException($"The property {propertyName} does not exist on {_view?.GetType()?.Name}.");
            }

            if (_view.GetType() != typeof(Form))
            {
                throw new InvalidOperationException(nameof(_view));
            }

            if (!(_view as Form).InvokeRequired)
            {
                SetPropertyValue(_view, propertyName, newValue);
            }
            else
            {
                (_view as Form).Invoke(new Action(() =>
                {
                    SetPropertyValue(_view, propertyName, newValue);
                }));
            }
        }
        /// <summary>
        /// The tweak field method for modifying field values
        /// </summary>
        /// <param name="fieldName">The name of the field to be tweaked</param>
        /// <param name="newValue">The new value of the field to be set</param>
        /// <exception cref="ArgumentException">Thrown if the field name is null or whitespace</exception>
        /// <exception cref="ArgumentNullException">Thrown if the new value is null</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingFieldException">Thrown if the field does not exist on the main testing form</exception>
        /// <exception cref="InvalidOperationException">Thrown if the testing form object is not typeof form</exception>
        public static void TweakField(string fieldName, object newValue)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(fieldName));
            }

            if (newValue is null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            if (_view is null)
            { 
                throw new NullReferenceException(nameof(_view));
            }

            FieldInfo field = GetField(_view, fieldName);

            if (field is null)
            {
                throw new MissingFieldException($"The field {fieldName} does not exist on {_view?.GetType()?.Name}.");
            }

            if (_view.GetType() != typeof(Form))
            {
                throw new InvalidOperationException(nameof(_view));
            }

            if ((_view as Form).InvokeRequired)
            {
                SetFieldValue(_view, fieldName, newValue);
            }
            else
            {
                (_view as Form).BeginInvoke(new Action(() =>
                {
                    SetFieldValue(_view, fieldName, newValue);
                }));
            }
        }
        /// <summary>
        /// The peek control method for getting control property values
        /// </summary>
        /// <param name="controlName">The name of the control to be peeked</param>
        /// <param name="propertyName">The name of the property value to get</param>
        /// <returns>A dynamic object type of the property value type</returns>
        /// <exception cref="ArgumentException">Thrown if the control or property name is null or whitespace</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingControlException">Thrown if the control does not exist on the main testing form</exception>
        public static dynamic PeekControl(string controlName, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(controlName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(controlName));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(propertyName));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            dynamic ctrl = GetControl(_view, controlName);

            if (ctrl is null)
            {
                throw new MissingControlException($"The control {controlName} does not exist on {_view?.GetType()?.Name}.");
            }

            return GetPropertyValue(ctrl, propertyName);
        }
        /// <summary>
        /// The peek property method for getting property values
        /// </summary>
        /// <param name="propertyName">The name of the property to be peeked</param>
        /// <returns>A dynamic object type of the property value type</returns>
        /// <exception cref="ArgumentException">Thrown if the property name is null or whitespace</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingPropertyException">Thrown if the property does not exist on the main testing form</exception>
        public static dynamic PeekProperty(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(propertyName));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            PropertyInfo property = GetProperty(_view, propertyName);

            if (property is null)
            {
                throw new MissingPropertyException($"The property {propertyName} does not exist on {_view?.GetType()?.Name}.");
            }

            return GetPropertyValue(_view, propertyName);
        }
        /// <summary>
        /// The peek field method for getting field values
        /// </summary>
        /// <param name="fieldName">The name of the field to be peeked</param>
        /// <returns>A dynamic object type of the field value type</returns>
        /// <exception cref="ArgumentException">Thrown if the field name is null or whitespace</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingFieldException">Thrown if the field does not exist on the main testing form</exception>
        public static dynamic PeekField(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(fieldName));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            FieldInfo field = GetField(_view, fieldName);
            if (field is null)
            {
                throw new MissingFieldException($"The field {fieldName} does not exist on {_view?.GetType()?.Name}.");
            }

            return GetFieldValue(_view, fieldName);
        }
        #endregion
    }
}