using SUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterDebugging
{
    public class MultiTests
    {
        public IEnumerable<Test> IsNotDivisibleByFive()
        {
            return Enumerable.Range(0, 7)
                .Select(n => Assert.That(n % 5).Is.Not.Zero);
        }

        public IEnumerable<Test> IsNotNegative()
        {
            return Enumerable.Range(-3, 7)
                .Select(n => Assert.That(n).Is.Not.Negative);
        }

        public IEnumerable<Test> IsPositiveOrNegative()
        {
            return Enumerable.Range(-2, 5)
                .Select(n => Assert.That(n).Is.Positive.Or.Negative);
        }

        public IEnumerable<Test> IsNotPositiveAndNotNegative()
        {
            return Enumerable.Range(-2, 5)
                .Select(n => Assert.That(n).Is.Not.Positive.And.Not.Negative);
        }

        public IEnumerable<Test> ThrowsAfterThree()
        {
            yield return Assert.That(7).Is.EqualTo(4);
            yield return Assert.That(2).Is.Not.Zero;
            yield return Assert.That(-1).Is.Not.Positive.Or.Not.Zero;

            throw new InvalidOperationException($"This exception was intentionally thrown.");
        }
    }

    public class AsyncTests
    {
        public async Task<Test> KeepYouWaitingThenThrow()
        {
            await Task.Yield();
            throw new InvalidOperationException("This exception was thrown intentionally.");
        }

        public async Task<Test> DelayThenPass()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(5));
            return Test.Pass;
        }
    }
}
