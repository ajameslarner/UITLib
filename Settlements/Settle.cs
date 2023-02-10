using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Utilities
{
    /// <summary>
    /// Argue the outcome of a result.
    /// </summary>
    public static class Settle
    {
        public static A<T> Should<T>(this T t)
        {
            return new A<T>(t);
        }
        /// <summary>
        /// Checks if the value is true.
        /// </summary>
        /// <param name="value">Bool value to check.</param>
        /// <returns>Returns true if the value is true; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the value is false.</exception>
        public static bool IsTrue(bool value)
        {
            if (value)
                return true;

            throw new UITestFailedException($"{nameof(value)} is not true.");
        }
        /// <summary>
        /// Checks if the value is false.
        /// </summary>
        /// <param name="value">Bool value to check.</param>
        /// <returns>Returns true if the value is false; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the value is true.</exception>
        public static bool IsFalse(bool value)
        {
            if (!value)
                return true;

            throw new UITestFailedException($"{nameof(value)} is not false.");
        }
        /// <summary>
        /// Checks if the value is false or null.
        /// </summary>
        /// <param name="value">Bool value to check.</param>
        /// <returns>Returns true if the value is false or if the value is null; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the value is true or not null.</exception>
        public static bool IsTrueOrNull(bool? value)
        {
            if (value == null)
                return true;

            if ((bool)value)
                return true;

            throw new UITestFailedException($"{nameof(value)} is not true or null.");
        }
        /// <summary>
        /// Checks if the value is false or null.
        /// </summary>
        /// <param name="value">Bool value to check.</param>
        /// <returns>Returns true if the value is false or if the value is null; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the value is true or not null.</exception>
        public static bool IsFalseOrNull(bool? value)
        {
            if (value == null)
                return true;

            if ((bool)!value)
                return true;

            throw new UITestFailedException($"{nameof(value)} is true and not null.");
        }
        /// <summary>
        /// Checks if the two objects are the same by value.
        /// </summary>
        /// <param name="a">Object A for comparison.</param>
        /// <param name="b">Object B for comparison.</param>
        /// <returns>Returns true if the two objects are the same by value; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the two objects not are the same by value</exception>
        public static bool IsEqual(object a, object b)
        {
            if (a.Equals(b))
                return true;

            throw new UITestFailedException($"{nameof(a)} is not equal to {nameof(b)}.");
        }
        /// <summary>
        /// Checks if the two objects are not the same by value.
        /// </summary>
        /// <param name="a">Object A for comparison.</param>
        /// <param name="b">Object B for comparison.</param>
        /// <returns>Returns true if the two objects are not the same by value; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the instances are the same by reference</exception>
        public static bool IsNotEqual(object a, object b)
        {
            if (!a.Equals(b))
                return true;

            throw new UITestFailedException($"{nameof(a)} is equal to {nameof(b)} by value.");
        }
        /// <summary>
        /// Checks if the two objects are the same by reference.
        /// </summary>
        /// <param name="a">Object A for comparison.</param>
        /// <param name="b">Object B for comparison.</param>
        /// <returns>Returns true if the two objects are the same by reference; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the two objects are not the same by reference.</exception>
        public static bool IsIdentical(object a, object b)
        {
            if (ReferenceEquals(a,b))
                return true;

            throw new UITestFailedException($"{nameof(a)} is not equal to {nameof(b)} by reference.");
        }
        /// <summary>
        /// Checks if the two instances of an object are not the same by reference.
        /// </summary>
        /// <param name="a">Object A for comparison.</param>
        /// <param name="b">Object B for comparison.</param>
        /// <returns>Returns true if the instances are not the same by reference; otherwise, throws SequenceTestFailedException.</returns>
        /// <exception cref="UITestFailedException">Thrown if the instances are the same by reference</exception>
        public static bool IsNotIdentical(object a, object b)
        {
            if (!ReferenceEquals(a, b))
                return true;

            throw new UITestFailedException($"{nameof(a)} is equal to {nameof(b)} by reference.");
        }
    }
}