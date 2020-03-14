using System;
using System.Collections.Generic;
using System.Text;
using SUnit;

namespace DogfoodingTests
{
    public class ComparableAssertions<T> where T : IComparable<T>
    {
        private readonly T low;
        private readonly T medium;
        private readonly T high;

        public ComparableAssertions(T low, T medium, T high)
        {
            this.low = low;
            this.medium = medium;
            this.high = high;
        }

        public static ComparableAssertions<int> Integers() => new ComparableAssertions<int>(-5, 0, 5);

        public static ComparableAssertions<double> Doubles() => new ComparableAssertions<double>(-11.4, 17.91, 4.24e27);

        public static ComparableAssertions<TimeSpan> TimeSpans()
        {
            return new ComparableAssertions<TimeSpan>(
                TimeSpan.FromMilliseconds(45),
                TimeSpan.FromSeconds(2.11),
                TimeSpan.FromMinutes(0.821));
        }

        public Test NotLessThanSelf()
        {
            return Assert.That(medium).Is.Not.LessThan(medium);
        }

        public Test LowLessThanHigh()
        {
            return Assert.That(low).Is.LessThan(high);
        }

        public Test HighNotLessThanLow() => Assert.That(high).Is.Not.LessThan(low);

        public Test LessThanOrEqualToSelf() => Assert.That(high).Is.LessThanOrEqualTo(high);

        public Test LowLessThanOrEqualToHigh() => Assert.That(low).Is.LessThanOrEqualTo(high);

        public Test HighNotLessThanOrEqualToLow() => Assert.That(high).Is.Not.LessThanOrEqualTo(low);

        public Test NotGreaterThanSelf() => Assert.That(medium).Is.Not.GreaterThan(medium);

        public Test HighGreaterThanLow() => Assert.That(high).Is.GreaterThan(low);

        public Test GreaterThanOrEqualToSelf() => Assert.That(low).Is.GreaterThanOrEqualTo(low);

        public Test LowNotGreaterThanOrEqualToHigh() => Assert.That(low).Is.Not.GreaterThanOrEqualTo(high);

        public Test HighGreaterThanOrEqualToLow() => Assert.That(high).Is.GreaterThanOrEqualTo(low);

    }
}
