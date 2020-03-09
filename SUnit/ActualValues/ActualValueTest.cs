using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    public abstract class ActualValueTest<T, TExpression, TTest> : Test
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : ActualValueTest<T, TExpression, TTest>
    {
        private readonly T actual;
        private readonly IConstraint<T> constraint;

        public sealed override bool Passed => constraint.Apply(actual);

        protected private ActualValueTest(T actual, IConstraint<T> constraint)
        {
            Debug.Assert(constraint != null);

            this.actual = actual;
            this.constraint = constraint;
        }

        protected private abstract TExpression CreateThing(T actual, ConstraintModifier<T> constraintModifier);

        public TExpression ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            return CreateThing(actual, constraintModifier);
        }

        public TExpression And => ApplyModifier(c => constraint & c);
        public TExpression Or => ApplyModifier(c => constraint | c);
        public new TExpression Xor => ApplyModifier(c => constraint ^ c);
    }
}
