using SUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogfoodingTests
{
    public class MultiTests
    {
        private readonly bool ascending;
        private readonly IEnumerable<int> values;

        private MultiTests(IEnumerable<int> values, bool ascending)
        {
            this.values = values;
            this.ascending = ascending;
        }

        public static MultiTests Descending()
        {
            return new MultiTests(Enumerable.Range(1, 6).Reverse(), false);
        }

        public static MultiTests Ascending()
        {
            return new MultiTests(Enumerable.Range(1, 6), true);
        }

        public IEnumerable<Test> IsSorted()
        {
            using var iter = values.GetEnumerator();
            if (!iter.MoveNext())
                yield break;
            int previous = iter.Current;

            while (iter.MoveNext())
            {
                yield return ascending ?
                    Assert.That(iter.Current).Is.GreaterThan(previous) :
                    Assert.That(iter.Current).Is.LessThan(previous);

                previous = iter.Current;
            }
        }
    }

    public class AsyncTests
    {
        public Task<Test> SyncEquals() => Task.FromResult<Test>(Assert.That(-31).Is.Not.Positive.Or.Is.Zero);

        public async Task<Test> AsyncEquivalentTo()
        {
            await Task.Yield();
            return Assert.That("Hello, World").Is.EquivalentTo("eoo lllHW,dr");
        }

        public async Task<Test> AsyncEqualTo()
        {
            await Task.Delay(400);
            return Assert.That("ABC").Is.EqualTo("ABC");
        }
    }
}
