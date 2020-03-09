using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.ActualValues
{
    public interface IIsThing<T, TIs, TTest> : IThing<T, TIs, TTest>
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

    public interface IIsThing<T> : IIsThing<T, IIsThing<T>, IIsThingTest<T>> { }

    internal class IsThing<T> : ThingBase<T, IIsThing<T>, IIsThingTest<T>>, IIsThing<T>
    {
        internal IsThing(T actual, ConstraintModifier<T> constraintModifier) 
            : base(actual, constraintModifier) { }

        internal IsThing(T actual) : this(actual, c => c) { }

        private protected override IIsThingTest<T> CreateThingTest(T actual, IConstraint<T> constraint)
        {
            return new IsThingTest<T>(actual, constraint);
        }

        private protected override IIsThing<T> CreateNewThing(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsThing<T>(actual, constraintModifier);
        }
    }

    public interface IIsThingTest<T> : IThingTest<T, IIsThing<T>, IIsThingTest<T>> { }

    public class IsThingTest<T> : ThingTestBase<T, IIsThing<T>, IIsThingTest<T>>, IIsThingTest<T>
    {
        internal IsThingTest(T actual, IConstraint<T> constraint) : base(actual, constraint) { } 

        private protected override IIsThing<T> CreateThing(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsThing<T>(actual, constraintModifier);
        }
    }
}
