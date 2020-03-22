using SUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleConsumer
{
    public class FailingTests
    {
        Test pass = Assert.That(0).Is.Zero;
        Test fail = Assert.That(0).Is.Not.Zero;

        public Test PositiveNumber_IsNegative() => Assert.That(17).Is.Negative;

        public Test PassAndFail() => Assert.That(-1).Is.Negative.And.GreaterThan(0);

        public Test LessAndMore()
        {
            return
                Assert.That(5).Is.LessThan(6) &
                Assert.That(9).Is.GreaterThan(17) &
                Assert.That(true).Is.True;
        }

        public Test NoIdeaWhetherThisWillPass()
        {
            return pass | (fail & pass);
        }

        public Test Hmmmm() => pass & (fail | pass & fail) | (pass & fail);

        public Test NonInclusiveLessThan() => Assert.That(7).Is.LessThan(7);

        public Test InclusiveLessThan() => Assert.That(7).Is.LessThanOrEqualTo(7);

        public Test IsNullTest() => Assert.That((object)null).Is.Not.Null;
    }

    public class MultiTests
    {
        public IEnumerable<Test> AllPass() => Enumerable.Repeat(Test.Pass, 5);

        public IEnumerable<Test> AllFail() => Enumerable.Repeat(Test.Fail, 4);

        public IEnumerable<Test> EveryThirdFails()
        {
            return Enumerable.Range(1, 9)
                .Select(n => Assert.That(n % 3).Is.Not.EqualTo(0));
        }
    }
}
