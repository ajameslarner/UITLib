
using System;

namespace UITestingFramework.Utilities
{
    public static class Comparisons
    {
        public static bool BeGreaterThan<T>(this A<T> x, T y) where T : IComparable<T>, IConvertible
        {
            return Convert.ToDouble(x.Value) > Convert.ToDouble(y);
        }
        public static bool BeLessThan<T>(this A<T> x, T y) where T : IComparable<T>, IConvertible
        {
            return Convert.ToDouble(x.Value) < Convert.ToDouble(y);
        }
        public static bool BeInRangeOf<T>(this A<T> a, T lower, T upper) where T : IComparable<T>, IConvertible
        {
            return Convert.ToDouble(a.Value) >= Convert.ToDouble(lower) && Convert.ToDouble(a) <= Convert.ToDouble(upper);
        }
    }
}