using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// The return type of all unit tests in <see cref="SUnit"/>.
    /// </summary>
    public abstract class Test
    {
        /// <summary>
        /// Indicates whether the test passed.
        /// </summary>
        public abstract bool Passed { get; }

        /// <summary>
        /// Overridden to indicate test status.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Passed ? "PASS" : "FAIL";

        private sealed class NotTest : Test
        {
            private readonly Test inner;
            public NotTest(Test inner) => this.inner = inner;

            public override bool Passed => !inner.Passed;
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

        private abstract class BinaryTest : Test
        {
            protected Test Left { get; }
            protected Test Right { get; }

            protected BinaryTest(Test left, Test right)
            {
                this.Left = left;
                this.Right = right;
            }
        }

        private sealed class AndTest : BinaryTest
        {
            public AndTest(Test left, Test right) : base(left, right) { }

            public override bool Passed => Left.Passed && Right.Passed;
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

        private sealed class OrTest : BinaryTest
        {
            public OrTest(Test left, Test right) : base(left, right) { }

            public override bool Passed => Left.Passed || Right.Passed;
        }
        /// <summary>
        /// Creates a <see cref="Test"/> that passes if either or both operands pass.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A <see cref="Test"/> that passes if either operand, or both operands, passes.</returns>
        public static Test operator | (Test left, Test right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            return new OrTest(left, right);
        }

        private sealed class XorTest : BinaryTest
        {
            public XorTest(Test left, Test right) : base(left, right) { }

            public override bool Passed => Left.Passed ^ Right.Passed;
        }

        /// <summary>
        /// Creates a <see cref="Test"/> that passes if exactly one operand passes, but fails if neither operand
        /// passes or if both operands pass.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>A <see cref="Test"/> that passes if exactly one operand passes.</returns>
        public static Test operator ^ (Test left, Test right)
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
