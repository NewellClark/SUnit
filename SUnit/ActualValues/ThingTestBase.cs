using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.ActualValues
{
    public abstract class ThingTestBase<T, TThing, TTest> : Test, IThingTest<T, TThing, TTest>
        where TThing : IThing<T, TThing, TTest>
        where TTest : IThingTest<T, TThing, TTest>
    {
        private readonly T actual;
        private readonly IConstraint<T> constraint;

        public sealed override bool Passed => constraint.Apply(actual);

        protected private ThingTestBase(T actual, IConstraint<T> constraint)
        {
            Debug.Assert(constraint != null);

            this.actual = actual;
            this.constraint = constraint;
        }

        IConstraint<T> IThingTest<T, TThing, TTest>.Constraint => constraint;

        protected private abstract TThing CreateThing(T actual, ConstraintModifier<T> constraintModifier);

        public TThing ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            return CreateThing(actual, constraintModifier);
        }

        private IThingTest<T, TThing, TTest> Casted => (IThingTest<T, TThing, TTest>)this;

        public TThing And => Casted.And;
        public TThing Or => Casted.Or;
        public TThing Xor => Casted.Xor;
    }
}
