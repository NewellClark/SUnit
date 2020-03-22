using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal class FloatingPointEqualToConstraint : IConstraint<double?>
    {
        private readonly double? expected;

        /// <summary>
        /// The maximum allowed "normalized" error; in other words, the error divided by
        /// the largest argument.
        /// </summary>
        private readonly double maxAllowedNormalizedError;
        
        public FloatingPointEqualToConstraint(double? expected) 
            : this(expected, 1e-7) { }
        public FloatingPointEqualToConstraint(double? expected, double maxAllowedNormalizedError)
        {
            this.expected = expected;
            this.maxAllowedNormalizedError = maxAllowedNormalizedError;
        }

        public bool Apply(double? actual)
        {
            if (actual is null)
                return expected is null;

            if (expected is null)
                return false;

            double x = expected.Value;
            double y = actual.Value;

            if (x == y)
                return true;

            double normalizedError = Math.Abs(x - y) / Math.Max(Math.Abs(x), Math.Abs(y));

            return normalizedError < maxAllowedNormalizedError;
        }

        public override string ToString() => $"~={Utilities.DisplayValue(expected)}";
    }
}
