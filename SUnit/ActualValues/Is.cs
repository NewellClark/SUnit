using SUnit.ActualValues;
using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    public interface Is<T, TIs, TTest> : IThing<T, TIs, TTest>
        where TIs : IThing<T, TIs, TTest>
        where TTest : IThingTest<T, TIs, TTest>
    {
        public virtual TTest EqualTo(T expected)
        {
            return ApplyConstraint(new EqualToConstraint<T>(expected));
        }

        public virtual TTest Null
        {
            get => ApplyConstraint(new NullConstraint<T>());
        }
    }
    public interface Is<T> : Is<T, Is<T>, IsTest<T>> { }

    internal class IsThing<T> : ThingBase<T, Is<T>, IsTest<T>>, Is<T>
    {
        internal IsThing(T actual, ConstraintModifier<T> constraintModifier)
            : base(actual, constraintModifier) { }

        internal IsThing(T actual) : this(actual, c => c) { }

        private protected override IsTest<T> CreateThingTest(T actual, IConstraint<T> constraint)
        {
            return new IsTest<T>(actual, constraint);
        }

        private protected override Is<T> CreateNewThing(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsThing<T>(actual, constraintModifier);
        }
    }

    public class IsTest<T> : ThingTestBase<T, Is<T>, IsTest<T>>
    {
        internal IsTest(T actual, IConstraint<T> constraint) : base(actual, constraint) { }

        private protected override Is<T> CreateThing(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsThing<T>(actual, constraintModifier);
        }
    }
}
