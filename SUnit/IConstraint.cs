using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
#pragma warning disable CA2225
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

            public override string ToString() => $"NOT {inner}";
        }

        /// <summary>
        /// Creates a new <see cref="IConstraint{T}"/> that passes if its operand fails.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns>A <see cref="IConstraint{T}"/> that passes if the operand fails.</returns>
        public static IConstraint<T> operator !(IConstraint<T> operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return new NotConstraint(operand);
        }

        /// <summary>
        /// Creates a new <see cref="IConstraint{T}"/> that passes when the current <see cref="IConstraint{T}"/> fails.
        /// </summary>
        public IConstraint<T> Inverted => !this;

        private abstract class BinaryConstraint : IConstraint<T>
        {
            protected IConstraint<T> Left { get; }
            protected IConstraint<T> Right { get; }

            public BinaryConstraint(IConstraint<T> left, IConstraint<T> right)
            {
                this.Left = left;
                this.Right = right;
            }

            protected abstract bool ApplyToBoth(T value, IConstraint<T> left, IConstraint<T> right);

            public bool Apply(T value) => ApplyToBoth(value, Left, Right);
        }

        private sealed class AndConstraint : BinaryConstraint
        {
            public AndConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }
            protected override bool ApplyToBoth(T value, IConstraint<T> left, IConstraint<T> right)
            {
                return left.Apply(value) & right.Apply(value);
            }

            public override string ToString() => $"{Left} AND {Right}";
        }
        
        /// <summary>
        /// Creates a <see cref="IConstraint{T}"/> that passes if both operands pass.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A new <see cref="IConstraint{T}"/> that passes if both operands pass.</returns>
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

            public override string ToString() => $"{Left} OR {Right}";
        }

        /// <summary>
        /// Creates a <see cref="IConstraint{T}"/> that passes if either or both operands passes.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A new <see cref="IConstraint{T}"/> that passes if either or both operands pass.</returns>
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

            public override string ToString() => $"{Left} XOR {Right}";
        }

        /// <summary>
        /// Creates a <see cref="IConstraint{T}"/> that passes if exactly one operand passes, but fails if both 
        /// operands pass or if both operands fail.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A new <see cref="IConstraint{T}"/> that passes if exactly one operand passes.</returns>
        public static IConstraint<T> operator ^ (IConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new XorConstraint(left, right);
        }
    }
#pragma warning restore CA2225
}
