using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SUnit.Constraints
{
#pragma warning disable CA2225
    /// <summary>
    /// A constraint that can be applied to a value. Evaluating the constraint may be asynchronous.
    /// </summary>
    /// <typeparam name="T">The type of value the constraint can be applied to.</typeparam>
    public interface IAsyncConstraint<T>
    {
        /// <summary>
        /// Applies the constraint to the specified value.
        /// </summary>
        /// <param name="value">The value to apply the constraint to.</param>
        /// <returns>A task who's value indicates whether the specified value satisfies the constraint.</returns>
        public Task<bool> ApplyAsync(T value);
        

        public static IAsyncConstraint<T> operator !(IAsyncConstraint<T> operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return new AsyncNotConstraint(operand);
        }

        public static IAsyncConstraint<T> operator &(IAsyncConstraint<T> left, IAsyncConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            static async Task<bool> applyOperandsAsync(T value, IAsyncConstraint<T> left, IAsyncConstraint<T> right)
            {
                var leftTask = left.ApplyAsync(value);
                var rightTask = right.ApplyAsync(value);

                return await leftTask.ConfigureAwait(false) & await rightTask.ConfigureAwait(false);
            }

            return CreateBinary(left, right, "AND", applyOperandsAsync);
        }

        public static IAsyncConstraint<T> operator &(IAsyncConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return left & FromConstraint(right);
        }

        public static IAsyncConstraint<T> operator &(IConstraint<T> left, IAsyncConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return FromConstraint(left) & right;
        }

        public static IAsyncConstraint<T> operator |(IAsyncConstraint<T> left, IAsyncConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            static async Task<bool> applyOperandsAsync(T value, IAsyncConstraint<T> left, IAsyncConstraint<T> right)
            {
                var leftTask = left.ApplyAsync(value);
                var rightTask = right.ApplyAsync(value);

                return await leftTask.ConfigureAwait(false) | await rightTask.ConfigureAwait(false);
            }

            return CreateBinary(left, right, "OR", applyOperandsAsync);
        }

        public static IAsyncConstraint<T> operator |(IAsyncConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return left | FromConstraint(right);
        }

        public static IAsyncConstraint<T> operator |(IConstraint<T> left, IAsyncConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return FromConstraint(left) | right;
        }

        public static IAsyncConstraint<T> operator ^(IAsyncConstraint<T> left, IAsyncConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            static async Task<bool> applyOperandsAsync(T value, IAsyncConstraint<T> left, IAsyncConstraint<T> right)
            {
                var leftTask = left.ApplyAsync(value);
                var rightTask = right.ApplyAsync(value);

                return await leftTask.ConfigureAwait(false) ^ await rightTask.ConfigureAwait(false);
            }

            return CreateBinary(left, right, "XOR", applyOperandsAsync);
        }

        public static IAsyncConstraint<T> operator ^(IAsyncConstraint<T> left, IConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return left ^ FromConstraint(right);
        }

        public static IAsyncConstraint<T> operator ^(IConstraint<T> left, IAsyncConstraint<T> right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return FromConstraint(left) ^ right;
        }


        private class AsyncNotConstraint : IAsyncConstraint<T>
        {
            private readonly IAsyncConstraint<T> inner;

            public AsyncNotConstraint(IAsyncConstraint<T> inner)
            {
                Debug.Assert(inner != null);

                this.inner = inner;
            }

            public async Task<bool> ApplyAsync(T value) => !(await inner.ApplyAsync(value).ConfigureAwait(false));

            public override string ToString() => $"NOT {inner}";
        }

        private sealed class AsyncConstraintWrapper : IAsyncConstraint<T>
        {
            private readonly IConstraint<T> inner;

            public AsyncConstraintWrapper(IConstraint<T> inner)
            {
                Debug.Assert(inner != null);

                this.inner = inner;
            }

            public Task<bool> ApplyAsync(T value) => Task.FromResult(inner.Apply(value));

            public override string ToString() => inner.ToString();
        }

        private abstract class AsyncBinaryOperatorConstraint : IAsyncConstraint<T>
        {
            private readonly IAsyncConstraint<T> left;
            private readonly IAsyncConstraint<T> right;

            public AsyncBinaryOperatorConstraint(IAsyncConstraint<T> left, IAsyncConstraint<T> right)
            {
                this.left = left;
                this.right = right;
            }

            protected abstract string OperatorName { get; }

            protected abstract Task<bool> ApplyOperandsAsync(T value, IAsyncConstraint<T> left, IAsyncConstraint<T> right);

            public Task<bool> ApplyAsync(T value) => ApplyOperandsAsync(value, left, right);

            public override string ToString() => $"{left} {OperatorName} {right}";
        }

        private sealed class AnonymousAsyncBinaryOperatorConstraint : AsyncBinaryOperatorConstraint
        {
            private readonly Func<T, IAsyncConstraint<T>, IAsyncConstraint<T>, Task<bool>> _ApplyOperands;

            public AnonymousAsyncBinaryOperatorConstraint(
                IAsyncConstraint<T> left, IAsyncConstraint<T> right, 
                string operatorName, Func<T, IAsyncConstraint<T>, IAsyncConstraint<T>, Task<bool>> applyOperandsAsync)
                : base(left, right)
            {
                Debug.Assert(applyOperandsAsync != null);

                this.OperatorName = operatorName;
                _ApplyOperands = applyOperandsAsync;
            }

            protected override string OperatorName { get; }
            protected override Task<bool> ApplyOperandsAsync(T value, IAsyncConstraint<T> left, IAsyncConstraint<T> right) => _ApplyOperands(value, left, right);
        }

        private static IAsyncConstraint<T> CreateBinary(
            IAsyncConstraint<T> left, IAsyncConstraint<T> right, string operatorName, 
            Func<T, IAsyncConstraint<T>, IAsyncConstraint<T>, Task<bool>> applyOperandsAsync)
        {
            return new AnonymousAsyncBinaryOperatorConstraint(left, right, operatorName, applyOperandsAsync);
        }

        private static IAsyncConstraint<T> FromConstraint(IConstraint<T> constraint) => new AsyncConstraintWrapper(constraint);
    }
#pragma warning restore CA2225
}
