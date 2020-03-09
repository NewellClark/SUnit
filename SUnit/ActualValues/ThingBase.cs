using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.ActualValues
{
    internal abstract class ThingBase<T, TThing, TTest> : IThing<T, TThing, TTest>
        where TThing : IThing<T, TThing, TTest>
        where TTest : IThingTest<T, TThing, TTest>
    {
        private readonly T actual;
        private readonly ConstraintModifier<T> constraintModifier;

        protected private ThingBase(T actual, ConstraintModifier<T> constraintModifier)
        {
            Debug.Assert(constraintModifier != null);

            this.actual = actual;
            this.constraintModifier = constraintModifier;
        }

        protected private abstract TTest CreateThingTest(T actual, IConstraint<T> constraint);
        protected private abstract TThing CreateNewThing(T actual, ConstraintModifier<T> constraintModifier);

        public TTest ApplyConstraint(IConstraint<T> constraint)
        {
            if (constraint is null) throw new ArgumentNullException(nameof(constraint));

            return CreateThingTest(actual, constraintModifier(constraint));
        }
        public TThing ApplyModifier(ConstraintModifier<T> constraintModifier)
        {
            if (constraintModifier is null) throw new ArgumentNullException(nameof(constraintModifier));

            IConstraint<T> combinedConstraintModifier(IConstraint<T> constraint)
            {
                var existing = this.constraintModifier;
                var @new = constraintModifier;

                return existing(@new(constraint));
            }

            return CreateNewThing(actual, combinedConstraintModifier);
        }
    }
}
