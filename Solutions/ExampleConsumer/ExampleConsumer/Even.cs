using SUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleConsumer
{
    public class Fact
    {
        private readonly bool isTrue;

        public Fact(bool isTrue) => this.isTrue = isTrue;

        public static Fact operator &(Fact left, Fact right) => new Fact(left.isTrue & right.isTrue);
        public static Fact operator |(Fact left, Fact right) => new Fact(left.isTrue | right.isTrue);
        public static Fact operator !(Fact operand) => new Fact(!operand.isTrue);
        public static bool operator true(Fact operand) => operand.isTrue;
        public static bool operator false(Fact operand) => !operand.isTrue;
    }

    public class SideEffect<T>
    {
        private readonly Func<T> supplier;

        public SideEffect(Func<T> supplier) => this.supplier = supplier;
        public SideEffect(T value) : this(() => value) { }

        public bool Happened { get; private set; }

        public T Value
        {
            get
            {
                Happened = true;
                return supplier();
            }
        }
    }

    public class FactTests
    {
        private SideEffect<Fact> True { get; } = new SideEffect<Fact>(new Fact(true));
        private SideEffect<Fact> False { get; } = new SideEffect<Fact>(new Fact(false));

        public Test EagerOr_SideEffects_Happen()
        {
            var _ = True.Value | False.Value;

            return Assert.That(False.Happened).Is.True;
        }

        public Test EagerAnd_SideEffects_Happen()
        {
            var _ = False.Value & True.Value;

            return Assert.That(True.Happened).Is.True;
        }

        public Test LazyOr_SideEffects_DoNotHappen()
        {
            var _ = True.Value || False.Value;

            return Assert.That(False.Happened).Is.False;
        }

        public Test LazyAnd_SideEffects_DoNotHappen()
        {
            var _ = False.Value && True.Value;

            return Assert.That(True.Happened).Is.False;
        }
    }
}
