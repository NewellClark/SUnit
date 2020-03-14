using System;
using System.Collections.Generic;
using System.Text;

namespace ActualLegitUnitTests
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


    }
}
