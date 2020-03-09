using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.ActualValues
{
    public interface IThingTest<T, TThing, TTest>
        where TThing : IThing<T, TThing, TTest>
        where TTest : IThingTest<T, TThing, TTest>
    {
        public abstract TThing ApplyModifier(ConstraintModifier<T> modifier);

        protected private abstract IConstraint<T> Constraint { get; }

        public sealed TThing And => ApplyModifier(c => Constraint & c);

        public sealed TThing Or => ApplyModifier(c => Constraint | c);

        public sealed TThing Xor => ApplyModifier(c => Constraint ^ c);
    }
}
