using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.NewAssertions
{
    public interface IBoolExpression : IValueExpression<bool?, IBoolExpression, BoolTest> { }


    public interface IBoolIsExpression : IIsExpression<bool?, IBoolIsExpression, BoolTest>
    {
        public BoolTest True => EqualTo(true);

        public BoolTest False => EqualTo(false);
    }


    public class BoolThat : That<bool?>
    {
        internal BoolThat(bool? actual) 
            : this(new BoolExpression(actual, c => c)) { }

        internal BoolThat(IBoolExpression expression)
            : base(expression) { }

        protected private new IBoolExpression Expression => (IBoolExpression)base.Expression;

        public new IBoolIsExpression Is => new BoolIsExpression(Expression);
    }


    public class BoolTest : ValueTest<bool?, BoolThat>
    {
        internal BoolTest(bool? actual, IConstraint<bool?> constraint)
            : base(actual, constraint) { }

        private protected override That<bool?> ApplyModifier(bool? actual, ConstraintModifier<bool?> modifier)
        {
            return new BoolThat(new BoolExpression(actual, modifier));
        }
    }


    internal class BoolExpression : ValueExpression<bool?, IBoolExpression, BoolTest>, IBoolExpression
    {
        internal BoolExpression(bool? actual, ConstraintModifier<bool?> modifier)
            : base(actual, modifier) { }

        private protected override BoolTest ApplyConstraint(bool? actual, IConstraint<bool?> constraint)
        {
            return new BoolTest(actual, constraint);
        }

        private protected override IBoolExpression ApplyModifier(bool? actual, ConstraintModifier<bool?> modifier)
        {
            return new BoolExpression(actual, modifier);
        }
    }


    internal class BoolIsExpression : IBoolIsExpression
    {
        private readonly IBoolExpression expression;

        internal BoolIsExpression(IBoolExpression expression)
        {
            Debug.Assert(expression != null);

            this.expression = expression;
        }

        public IBoolIsExpression ApplyModifier(ConstraintModifier<bool?> modifier)
        {
            return new BoolIsExpression(expression.ApplyModifier(modifier));
        }

        public BoolTest ApplyConstraint(IConstraint<bool?> constraint)
        {
            return expression.ApplyConstraint(constraint);
        }
    }
}
