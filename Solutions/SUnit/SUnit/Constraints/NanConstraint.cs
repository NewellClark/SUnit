using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    internal sealed class NanConstraint : IConstraint<double?>
    {
        public bool Apply(double? actual)
        {
            if (!actual.HasValue)
                return false;

            return double.IsNaN(actual.Value);
        }

        public override string ToString() => "NaN";
    }
}
