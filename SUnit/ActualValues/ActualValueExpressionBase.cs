using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    internal abstract class ActualValueExpressionBase<T, TExpression, TTest> : IActualValueExpression<T, TExpression, TTest>
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : ActualValueTest<T, TExpression, TTest>
    {
        private readonly T actual;
        private readonly ConstraintModifier<T> constraintModifier;

        protected private ActualValueExpressionBase(T actual, ConstraintModifier<T> constraintModifier)
        {
            Debug.Assert(constraintModifier != null);

            this.actual = actual;
            this.constraintModifier = constraintModifier;
        }

        protected private abstract TTest CreateTest(T actual, IConstraint<T> constraint);
        protected private abstract TExpression CreateExpression(T actual, ConstraintModifier<T> constraintModifier);

        public TTest ApplyConstraint(IConstraint<T> constraint)
        {
            if (constraint is null) throw new ArgumentNullException(nameof(constraint));

            return CreateTest(actual, constraintModifier(constraint));
        }
        public TExpression ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            if (constraintModifier is null) throw new ArgumentNullException(nameof(constraintModifier));

            IConstraint<T> combinedConstraintModifier(IConstraint<T> constraint)
            {
                var existing = this.constraintModifier;
                var @new = constraintModifier;

                return existing(@new(constraint));
            }

            return CreateExpression(actual, combinedConstraintModifier);
        }
    }
}
