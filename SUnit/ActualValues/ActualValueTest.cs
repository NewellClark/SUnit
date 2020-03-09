using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.ActualValues
{
    public abstract class ActualValueTest<T, TExpression, TTest> : Test, IActualValueTest<T, TExpression, TTest>
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : IActualValueTest<T, TExpression, TTest>
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

        IConstraint<T> IActualValueTest<T, TExpression, TTest>.Constraint => constraint;

        protected private abstract TExpression CreateExpression(T actual, ConstraintModifier<T> constraintModifier);

        public TExpression ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            Debug.Assert(constraintModifier != null);

            return CreateExpression(actual, constraintModifier);
        }

        private IActualValueTest<T, TExpression, TTest> Casted => this;

        public TExpression And => Casted.And;
        public TExpression Or => Casted.Or;
        public new TExpression Xor => Casted.Xor;
    }
}
