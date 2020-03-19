using SUnit.Constraints;
using System.Diagnostics;

namespace SUnit.Assertions
{



    //public interface IThatExpression<T, TExpression, TTest, TThat>
    //    : IValueExpression<T, TExpression, TTest, TThat>
    //    where TExpression : IValueExpression<T, TExpression, TTest, TThat>
    //    where TTest : ValueTest<T, TExpression, TTest, TThat>
    //{

    //}

    //internal class ThatExpression<T, TExpression, TTest, TThat> 
    //    : IThatExpression<T, TExpression, TTest, TThat>
    //    where TExpression : IValueExpression<T, TExpression, TTest, TThat>
    //    where TTest : ValueTest<T, TExpression, TTest, TThat>
    //{
    //    private readonly TExpression expression;

    //    internal ThatExpression(TExpression expression)
    //    {
    //        Debug.Assert(expression != null);

    //        this.expression = expression;
    //    }

    //    public TTest ApplyConstraint(IConstraint<T> constraint) => expression.ApplyConstraint(constraint);

    //    public TExpression ApplyModifier(ConstraintModifier<T> modifier) => expression.ApplyModifier(modifier);
    //}
}
