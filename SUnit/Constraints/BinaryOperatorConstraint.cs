using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
    /// <summary>
    /// A <see cref="IConstraint{T}"/> that combines two <see cref="IConstraint{T}"/>s using a binary operator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class BinaryOperatorConstraint<T> : IConstraint<T>
    {
        private readonly IConstraint<T> left;
        private readonly IConstraint<T> right;

        public BinaryOperatorConstraint(IConstraint<T> left, IConstraint<T> right)
        {
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// When overridden in a derived class, applies both operands to the specified value.
        /// </summary>
        /// <param name="value">The value to apply both operands to.</param>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of applying both operands to the value.</returns>
        protected abstract bool ApplyOperands(T value, IConstraint<T> left, IConstraint<T> right);
        
        /// <inheritdoc/>
        public bool Apply(T value) => ApplyOperands(value, left, right);

        /// <summary>
        /// The name of the operator that is applied to the two sub-constraints.
        /// </summary>
        protected abstract string OperatorName { get; }

        /// <inheritdoc/>
        public sealed override string ToString() => $"{left} {OperatorName} {right}";
    }

    internal sealed class AndConstraint<T> : BinaryOperatorConstraint<T>
    {
        public AndConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }

        /// <inheritdoc/>
        protected override bool ApplyOperands(T value, IConstraint<T> left, IConstraint<T> right)
        {
            return left.Apply(value) & right.Apply(value);
        }

        /// <inheritdoc/>
        protected override string OperatorName => "AND";
    }

    internal sealed class OrConstraint<T> : BinaryOperatorConstraint<T>
    {
        public OrConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }

        /// <inheritdoc/>
        protected override bool ApplyOperands(T value, IConstraint<T> left, IConstraint<T> right)
        {
            return left.Apply(value) | right.Apply(value);
        }

        /// <inheritdoc/>
        protected override string OperatorName => "OR";
    }
    
    internal sealed class XorConstraint<T> : BinaryOperatorConstraint<T>
    {
        public XorConstraint(IConstraint<T> left, IConstraint<T> right) : base(left, right) { }

        /// <inheritdoc/>
        protected override bool ApplyOperands(T value, IConstraint<T> left, IConstraint<T> right)
        {
            return left.Apply(value) ^ right.Apply(value);
        }

        /// <inheritdoc/>
        protected override string OperatorName => "XOR";
    }
}
