using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    partial class Test
    {
        public static bool operator true(Test operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return operand.Passed;
        }

        public static bool operator false(Test operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return !operand.Passed;
        }

        private sealed class NotTest : Test
        {
            private readonly Test inner;
            public NotTest(Test inner) => this.inner = inner;

            public override bool Passed => !inner.Passed;

            public override string ToString() => $"NOT {inner}";
        }

        /// <summary>
        /// Creates a new test by inverting the current test.
        /// </summary>
        /// <param name="operand">The test to invert.</param>
        /// <returns>A new test that passes when the current test fails.</returns>
        public static Test operator !(Test operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return new NotTest(operand);
        }

        /// <summary>
        /// A <see cref="Test"/> that applies a binary operator to two other <see cref="Test"/>s. 
        /// </summary>
        private abstract class BinaryOperatorTest : Test
        {
            private readonly Test left;
            private readonly Test right;

            protected private BinaryOperatorTest(Test left, Test right)
            {
                this.left = left;
                this.right = right;
            }

            protected private abstract bool BinaryOperator(Test left, Test right);

            protected private abstract string OperatorName { get; }

            public sealed override bool Passed => BinaryOperator(left, right);

            public sealed override string ToString() => $"{left}\n{OperatorName}\n{right}";
        }

        private sealed class AndTest : BinaryOperatorTest
        {
            public AndTest(Test left, Test right) : base(left, right) { }

            protected private override bool BinaryOperator(Test left, Test right) => left.Passed & right.Passed;

            protected private override string OperatorName => "AND";
        }

        /// <summary>
        /// Creates a <see cref="Test"/> that only passes if both operands pass.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A <see cref="Test"/> that only passes if both operands pass.</returns>
        public static Test operator &(Test left, Test right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new AndTest(left, right);
        }

        private sealed class OrTest : BinaryOperatorTest
        {
            public OrTest(Test left, Test right) : base(left, right) { }

            protected private override bool BinaryOperator(Test left, Test right) => left.Passed | right.Passed;

            protected private override string OperatorName => "OR";
        }

        /// <summary>
        /// Creates a <see cref="Test"/> that passes if either or both operands pass.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A <see cref="Test"/> that passes if either operand, or both operands, passes.</returns>
        public static Test operator |(Test left, Test right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new OrTest(left, right);
        }

        private sealed class XorTest : BinaryOperatorTest
        {
            public XorTest(Test left, Test right) : base(left, right) { }

            protected private override bool BinaryOperator(Test left, Test right) => left.Passed ^ right.Passed;

            protected private override string OperatorName => "XOR";
        }

        /// <summary>
        /// Creates a <see cref="Test"/> that passes if exactly one operand passes, but fails if neither operand
        /// passes or if both operands pass.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A <see cref="Test"/> that passes if exactly one operand passes.</returns>
        public static Test operator ^(Test left, Test right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new XorTest(left, right);
        }

        private sealed class PassFailTest : Test
        {
            public PassFailTest(bool passed) => this.Passed = passed;

            public override bool Passed { get; }
        }

        /// <summary>
        /// Gets a test that always passes.
        /// </summary>
        public static Test Pass { get; } = new PassFailTest(true);
        /// <summary>
        /// Gets a test that always fails.
        /// </summary>
        public static Test Fail { get; } = new PassFailTest(false);

        /// <summary>
        /// Creates a new <see cref="Test"/> that passes when the operand fails.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static Test LogicalNot(Test operand) => !operand;

        /// <summary>
        /// Named alias for operator AND.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Test BitwiseAnd(Test left, Test right) => left & right;

        /// <summary>
        /// Named alias for operator |.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Test BitwiseOr(Test left, Test right) => left | right;

        /// <summary>
        /// Named alias for operator XOR.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Test Xor(Test left, Test right) => left ^ right;
    }
}
