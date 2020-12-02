using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Constraints
{
#pragma warning disable CA2225
    /// <summary>
    /// A constraint that can be applied to a value. 
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

        /// <summary>
        /// Creates a new <see cref="IConstraint{T}"/> that passes if its operand fails.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns>A <see cref="IConstraint{T}"/> that passes if the operand fails.</returns>
        public static IConstraint<T> operator !(IConstraint<T> operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return new NotConstraint<T>(operand);
        }

        /// <summary>
        /// Creates a new <see cref="IConstraint{T}"/> that passes when the current <see cref="IConstraint{T}"/> fails.
        /// </summary>
        public IConstraint<T> Inverted => !this;

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

            return new AndConstraint<T>(left, right);
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

            return new OrConstraint<T>(left, right);
        }

        /// <summary>
        /// Creates a <see cref="IConstraint{T}"/> that passes if exactly one operand passes, but fails if both 
        /// operands pass or if both operands fail.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A new <see cref="IConstraint{T}"/> that passes if exactly one operand passes.</returns>
        public static IConstraint<T> operator ^(IConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new XorConstraint<T>(left, right);
        }
    }
#pragma warning restore CA2225
}
