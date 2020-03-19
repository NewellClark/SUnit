using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    public interface ILongExpression : IValueExpression<long?, ILongExpression, LongTest> { }


    public interface ILongIsExpression : IIsExpression<long?, ILongIsExpression, LongTest>
    {
        public LongTest Zero => EqualTo(0L);

        public LongTest Positive => this.GreaterThan(0L);

        public LongTest Negative => this.LessThan(0L);
    }


    internal class LongIsExpression : ILongIsExpression
    {
        private readonly ILongExpression expression;

        internal LongIsExpression(ILongExpression expression)
        {
            Debug.Assert(expression != null);

            this.expression = expression;
        }

        public ILongIsExpression ApplyModifier(ConstraintModifier<long?> modifier)
        {
            return new LongIsExpression(expression.ApplyModifier(modifier));
        }

        public LongTest ApplyConstraint(IConstraint<long?> constraint)
        {
            return expression.ApplyConstraint(constraint);
        }
    }


    public class LongThat : That<long?>
    {
        internal LongThat(long? actual) 
            : this(new LongExpression(actual, c => c)) { }

        internal LongThat(ILongExpression expression) 
            : base(expression) { }

        protected private new ILongExpression Expression => (ILongExpression)base.Expression;

        public new ILongIsExpression Is => new LongIsExpression(Expression);
    }


    public class LongTest : ValueTest<long?, LongThat>
    {
        internal LongTest(long? actual, IConstraint<long?> constraint) 
            : base(actual, constraint) { }

        private protected override That<long?> ApplyModifier(long? actual, ConstraintModifier<long?> modifier)
        {
            return new LongThat(new LongExpression(actual, modifier));
        }
    }


    internal class LongExpression : ValueExpression<long?, ILongExpression, LongTest>, ILongExpression
    {
        internal LongExpression(long? actual, ConstraintModifier<long?> modifier)
            : base(actual, modifier) { }

        private protected override LongTest ApplyConstraint(long? actual, IConstraint<long?> constraint)
        {
            return new LongTest(actual, constraint);
        }

        private protected override ILongExpression ApplyModifier(long? actual, ConstraintModifier<long?> modifier)
        {
            return new LongExpression(actual, modifier);
        }
    }
}
