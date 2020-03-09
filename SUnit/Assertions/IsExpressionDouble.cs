using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    public interface IIsExpressionDouble : IIsExpression<double, IIsExpressionDouble, IsTestDouble>
    {
        public IsTestDouble Zero => ApplyConstraint(num => num == 0.0);
        public IsTestDouble Negative => ApplyConstraint(num => double.IsNegative(num));
        public IsTestDouble Positive => ApplyConstraint(num => num >= double.Epsilon);
        public IsTestDouble NaN => ApplyConstraint(Constraint.Create<double>(num => double.IsNaN(num)));
    }

    internal class IsExpressionDouble : ActualValueExpression<double, IIsExpressionDouble, IsTestDouble>, IIsExpressionDouble
    {
        internal IsExpressionDouble(double actual, ConstraintModifier<double> constraintModifier) 
            : base(actual, constraintModifier) { }
        internal IsExpressionDouble(double actual) 
            : base(actual, c => c) { }

        private protected override IsTestDouble CreateTest(double actual, IConstraint<double> constraint)
        {
            return new IsTestDouble(actual, constraint);
        }

        private protected override IIsExpressionDouble CreateExpression(double actual, ConstraintModifier<double> constraintModifier)
        {
            return new IsExpressionDouble(actual, constraintModifier);
        }
    }
}
