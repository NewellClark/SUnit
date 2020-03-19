using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.NewAssertions
{
    public interface IDecimalExpression : IValueExpression<decimal?, IDecimalExpression, DecimalTest> { }
    

    public interface IDecimalIsExpression : IIsExpression<decimal?, IDecimalIsExpression, DecimalTest>
    {
        public DecimalTest Zero => EqualTo(0m);

        public DecimalTest Positive => this.GreaterThan(0m);

        public DecimalTest Negative => this.LessThan(0m);
    }


    internal class DecimalExpression : ValueExpression<decimal?, IDecimalExpression, DecimalTest>, IDecimalExpression
    {
        internal DecimalExpression(decimal? actual, ConstraintModifier<decimal?> modifier)
            : base(actual, modifier) { }

        private protected override DecimalTest ApplyConstraint(decimal? actual, IConstraint<decimal?> constraint)
        {
            return new DecimalTest(actual, constraint);
        }

        private protected override IDecimalExpression ApplyModifier(decimal? actual, ConstraintModifier<decimal?> modifier)
        {
            return new DecimalExpression(actual, modifier);
        }
    }


    internal class DecimalIsExpression : IDecimalIsExpression
    {
        private readonly IDecimalExpression expression;

        internal DecimalIsExpression(IDecimalExpression expression)
        {
            Debug.Assert(expression != null);

            this.expression = expression;
        }

        public IDecimalIsExpression ApplyModifier(ConstraintModifier<decimal?> modifier)
        {
            return new DecimalIsExpression(expression.ApplyModifier(modifier));
        }

        public DecimalTest ApplyConstraint(IConstraint<decimal?> constraint)
        {
            return expression.ApplyConstraint(constraint);
        }
    }


    public class DecimalThat : That<decimal?>
    {
        internal DecimalThat(decimal? actual) 
            : this(new DecimalExpression(actual, c => c)) { }

        internal DecimalThat(IDecimalExpression expression)
            : base(expression) { }

        protected private new IDecimalExpression Expression => (IDecimalExpression)base.Expression;

        public new IDecimalIsExpression Is => new DecimalIsExpression(Expression);
    }


    public class DecimalTest : ValueTest<decimal?, DecimalThat>
    {
        internal DecimalTest(decimal? actual, IConstraint<decimal?> constraint)
            : base(actual, constraint) { }

        private protected override That<decimal?> ApplyModifier(decimal? actual, ConstraintModifier<decimal?> modifier)
        {
            return new DecimalThat(new DecimalExpression(actual, modifier));
        }
    }
}
