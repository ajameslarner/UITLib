using UITestingFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UITestingFramework.Utilities
{
    /// <summary>
    /// The entity class for calling, creating and handling referenced classes.
    /// </summary>
    public static class Entity
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
        private static MethodInfo GetMethod(object src, string methodName)
        {
            if (src == null) return null;
            return src.GetType()?.GetMethod(methodName, _accessLayer) ?? null;
        }
        private static Type GetClassFromNamespace(Assembly src, string ns, string className)
        {
            if (src == null) return null;
            return src.GetTypes()?.Where(x => x.Namespace == ns)?.Where(y => y.Name == className)?.FirstOrDefault() ?? null;
        }
        private static Type GetReferencedClass(string className)
        {
            return GetClassFromNamespace(_assembly, _namespaces.Where(x => GetClassFromNamespace(_assembly, x, className) != null)?.First(), className) ?? null;
        }
        private static MethodInfo GetMethod(Type src, string methodName)
        {
            if (src == null) return null;
            return src.GetMethod(methodName, _accessLayer) ?? null;
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// The using method to reference class namespaces (Note: Class namespace must be referenced before use)
        /// </summary>
        /// <param name="ns">The namespace of the referenced class</param>
        /// <exception cref="ArgumentException">Thrown if the namespace passed is null or whitespace</exception>
        public static void Using(string ns)
        {
            if (string.IsNullOrWhiteSpace(ns))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(ns));
            }

            if (!_namespaces.Contains(ns))
            {
                _namespaces.Add(ns);
            }

        }
        /// <summary>
        /// The call method to call static referenced class methods
        /// </summary>
        /// <param name="className">The name of the class to be called</param>
        /// <param name="methodName">The name of the method to be called</param>
        /// <returns>A dynamic object type of the methods return type, if exists</returns>
        /// <exception cref="ArgumentException">Thrown if the classname or the method is null or whitespace</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingNamespaceReferenceException">Thrown if the class is missing a referenced namespace</exception>
        /// <exception cref="MissingMethodException">Thrown if the method does not exist on the referenced class</exception>
        public static dynamic Call(string className, string methodName)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(className));
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(methodName));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            Type classname = GetReferencedClass(className);
            if (classname is null)
            {
                throw new MissingNamespaceReferenceException($"No class called {className} has been found in the referenced Namespaces. Try Using() to reference class namespaces.");
            }

            MethodInfo method = GetMethod(_view, methodName);
            if (methodName is null)
            {
                throw new MissingMethodException($"The method {methodName} does not exist in {_assembly}.");
            }

            return method.Invoke(null, new object[method.GetParameters().Length]);
        }
        /// <summary>
        /// The call method to call static referenced class methods with parameters
        /// </summary>
        /// <param name="className">The name of the class to be called</param>
        /// <param name="methodName">The name of the method to be called</param>
        /// <param name="methodParams">The paramaters to be passed into the called method</param>
        /// <returns>A dynamic object type of the methods return type, if exists</returns>
        /// <exception cref="ArgumentException">Thrown if the class name or the method name is null or whitespace</exception>
        /// <exception cref="ArgumentNullException">Thrown if the parameters are null</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingNamespaceReferenceException">Thrown if the class has not been referenced</exception>
        /// <exception cref="MissingMethodException">Thrown if the method does not exist in the referenced class</exception>
        public static dynamic Call(string className, string methodName, params object[] methodParams)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(className));
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(methodName));
            }

            if (methodParams is null)
            {
                throw new ArgumentNullException(nameof(methodParams), "Argument cannot be null.");
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            Type classname = GetReferencedClass(className);
            if (classname is null)
            {
                throw new MissingNamespaceReferenceException($"No class called {className} has been found in the referenced Namespaces. Try Using() to reference class namespaces.");
            }

            MethodInfo method = GetMethod(classname, methodName);
            if (method is null)
            {
                throw new MissingMethodException($"The method {methodName} does not exist in {_assembly}.");
            }

            return method.Invoke(null, methodParams);
        }
        /// <summary>
        /// The create method for instatiating referenced classes without parameters
        /// </summary>
        /// <param name="className">The name of the referenced class to be instatiated</param>
        /// <returns>An instance of the class</returns>
        /// <exception cref="ArgumentException">Thrown if the class name called is null or whitespace</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingNamespaceReferenceException">Thrown if the class has not been referenced</exception>
        public static dynamic Create(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(className));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            Type classType = GetReferencedClass(className);
            if (classType is null)
            {
                throw new MissingNamespaceReferenceException($"No class called {className} has been found in the referenced Namespaces. Try SequenceTest.Using() to reference class namespaces.");
            }

            try
            {
                return Activator.CreateInstance(classType) ?? null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// The create method for instatiating referenced classes with parameters
        /// </summary>
        /// <param name="className">The name of the referenced class to be instatiated</param>
        /// <param name="parameters">The parameters for the referenced class to be instatiated</param>
        /// <returns>An instance of the class</returns>
        /// <exception cref="ArgumentException">Thrown if the class called is null or whitespace</exception>
        /// <exception cref="ArgumentNullException">Thrown if the parameters are null</exception>
        /// <exception cref="NullReferenceException">Thrown if the main testing form is null</exception>
        /// <exception cref="MissingNamespaceReferenceException">Thrown if the class has not been referenced</exception>
        public static dynamic Create(string className, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Argument cannot be null or whitespace.", nameof(className));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (_view is null)
            {
                throw new NullReferenceException(nameof(_view));
            }

            Type classType = GetReferencedClass(className);
            if (classType is null)
            {
                throw new MissingNamespaceReferenceException($"No class called {className} has been found in the referenced Namespaces. Try SequenceTest.Using() to reference class namespaces.");
            }

            try
            {
                return Activator.CreateInstance(classType, parameters) ?? null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}