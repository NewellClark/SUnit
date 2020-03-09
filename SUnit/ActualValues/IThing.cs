using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.ActualValues
{
    public delegate IConstraint<T> ConstraintModifier<T>(IConstraint<T> constraint);

    public interface IThing<T, TThing, TTest>
        where TThing : IThing<T, TThing, TTest>
        where TTest : IThingTest<T, TThing, TTest>
    {
        public abstract TTest ApplyConstraint(IConstraint<T> constraint);
        public abstract TThing ApplyModifier(ConstraintModifier<T> modifier);

        public sealed TThing Not => ApplyModifier(constraint => !constraint);
    }


}
