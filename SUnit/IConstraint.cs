using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// A constraint that can be applied to value. 
    /// </summary>
    /// <typeparam name="T">The type of value the constraint can be applied to.</typeparam>
    public interface IConstraint<T>
    {
        /// <summary>
        /// Applies the constraint to the specified value.
        /// </summary>
        /// <param name="value">The value to apply the constraint to.</param>
        /// <returns>Whether the value satisfies the constraint.</returns>
        public bool Apply(T value);


        private sealed class NotConstraint : IConstraint<T>
        {
            private readonly IConstraint<T> inner;
            public NotConstraint(IConstraint<T> inner)
            {
                this.inner = inner;
            }

            public bool Apply(T value) => !inner.Apply(value);
        }

        public static IConstraint<T> operator !(IConstraint<T> operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return new NotConstraint(operand);
        }

        private abstract class BinaryConstraint : IConstraint<T>
        {
            private readonly IConstraint<T> left;
            private readonly IConstraint<T> right;

            public BinaryConstraint(IConstraint<T> left, IConstraint<T> right)
            {
                this.left = left;
                this.right = right;
            }

            protected abstract bool ApplyToBoth(T value, IConstraint<T> left, IConstraint<T> right);

            public bool Apply(T value) => ApplyToBoth(value, left, right);
        }

        private sealed class AndConstraint : BinaryConstraint
        {
            public AndConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }
            protected override bool ApplyToBoth(T value, IConstraint<T> left, IConstraint<T> right)
            {
                return left.Apply(value) & right.Apply(value);
            }
        }
        
        public static IConstraint<T> operator &(IConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new AndConstraint(left, right);
        }

        private sealed class OrConstraint : BinaryConstraint
        {
            public OrConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }
            protected override bool ApplyToBoth(T value, IConstraint<T> left, IConstraint<T> right)
            {
                return left.Apply(value) | right.Apply(value);
            }
        }

        public static IConstraint<T> operator |(IConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new OrConstraint(left, right);
        }

        private sealed class XorConstraint : BinaryConstraint
        {
            public XorConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }

            protected override bool ApplyToBoth(T value, IConstraint<T> left, IConstraint<T> right)
            {
                return left.Apply(value) ^ right.Apply(value);
            }
        }

        public static IConstraint<T> operator ^ (IConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new XorConstraint(left, right);
        }
    }
}
