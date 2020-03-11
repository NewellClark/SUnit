using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;
using static SUnit.Helpers;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Assertions
{
    [TestFixture]
    public class FloatingPointEquality
    {
        private readonly double? @null = null;

        private void FloatAssert(double expected, double actual)
        {
            var result = Assert.That(actual).Is.EqualTo(expected);
            double errorFraction = Abs(expected - actual) / Max(expected, actual);
            string message = $"Expected {expected}\nbut was {actual}\n{errorFraction}";
            nAssert.That(result.Passed, Is.True, message);
        }

        private double Repeat(long iterations, double start, Func<double, double> operation)
        {
            double result = start;
            for (long count = 0; count < iterations; count++)
                result = operation(result);

            return result;
        }

        [Test]
        [TestCase(600)]
        [TestCase(6_000)]
        [TestCase(60_000)]
        public void SinPiOverSix_Iterations(int iterations)
        {
            double angle = Repeat(iterations, PI / 6.0, r => r += PI / 6.0);
            double actual = 1_000.0 * Sin(angle);
            double expected = 500.0;

            FloatAssert(expected, actual);
        }

        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        public void SquareRootSquared_PI_E(int iterations)
        {
            double squareRooted = Repeat(iterations, PI * E, r => Sqrt(r));
            double squaredBack = Repeat(iterations, squareRooted, r => Pow(r, 2));

            FloatAssert(PI * E, squaredBack);
        }

        [Test]
        [TestCase(600)]
        [TestCase(6_000)]
        [TestCase(60_000)]
        public void Negatives_SinThingAgain(int iterations)
        {
            double angle = Repeat(iterations, PI / 6.0, r => r += PI / 6.0);
            double actual = -1_000_000.0 * Sin(angle);
            double expected = -500_000.0;

            FloatAssert(expected, actual);
        }

        [Test]
        public void NullEqualsNull()
        {
            AssertPassed(Assert.That(@null).Is.EqualTo(@null));
        }

        [Test]
        public void Null_NotEqualTo_NonNull()
        {
            AssertPassed(Assert.That(@null).Is.Not.EqualTo(498.6));
        }

        [Test]
        public void NonNull_NotEqualTo_Null()
        {
            AssertPassed(Assert.That(-123.456).Is.Not.EqualTo(@null));
        }

        [Test]
        public void Nan_NotEqualTo_Null()
        {
            AssertPassed(Assert.That(double.NaN).Is.Not.EqualTo(@null));
        }

        [Test]
        public void Null_NotEqualTo_NaN()
        {
            AssertPassed(Assert.That(@null).Is.Not.EqualTo(double.NaN));
        }
    }
}
