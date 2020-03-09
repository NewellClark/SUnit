using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.ActualValues
{
    public delegate IConstraint<T> ConstraintModifier<T>(IConstraint<T> constraint);

    public interface IActualValueExpression<T, TExpression, TTest>
        where TExpression : IActualValueExpression<T, TExpression, TTest>
        where TTest : IActualValueTest<T, TExpression, TTest>
    {
        public abstract TTest ApplyConstraint(IConstraint<T> constraint);
        public abstract TExpression ApplyModifier(ConstraintModifier<T> modifier);

        public sealed TExpression Not => ApplyModifier(constraint => !constraint);
    }
}
