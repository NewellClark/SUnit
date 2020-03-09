using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    public interface IIsExpression<T, TIs, TTest> : IActualValueExpression<T, TIs, TTest>
        where TIs : IActualValueExpression<T, TIs, TTest>
        where TTest : ActualValueTest<T, TIs, TTest>
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

    public interface IIsExpression<T> : IIsExpression<T, IIsExpression<T>, IsTest<T>> { }

    internal class IsExpression<T> : ActualValueExpressionBase<T, IIsExpression<T>, IsTest<T>>, IIsExpression<T>
    {
        internal IsExpression(T actual, ConstraintModifier<T> constraintModifier) 
            : base(actual, constraintModifier) { }

        internal IsExpression(T actual) : this(actual, c => c) { }

        private protected override IsTest<T> CreateTest(T actual, IConstraint<T> constraint)
        {
            return new IsTest<T>(actual, constraint);
        }

        private protected override IIsExpression<T> CreateExpression(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsExpression<T>(actual, constraintModifier);
        }
    }

    public class IsTest<T> : ActualValueTest<T, IIsExpression<T>, IsTest<T>>
    {
        internal IsTest(T actual, IConstraint<T> constraint) : base(actual, constraint) { } 

        private protected override IIsExpression<T> CreateThing(T actual, ConstraintModifier<T> constraintModifier)
        {
            return new IsExpression<T>(actual, constraintModifier);
        }
    }
}
