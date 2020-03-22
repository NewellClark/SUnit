using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using assert = NUnit.Framework.Assert;

namespace SUnit
{
    [TestFixture]
    public class TestTests
    {
        private static void Passed(Test test) => assert.That(test.Passed, Is.True);
        private static void Failed(Test test) => assert.That(test.Passed, Is.False);

        [Test]
        public void NotPass_Fails() => Failed(!Test.Pass);

        [Test]
        public void NotFail_Passes() => Passed(!Test.Fail);

        [Test]
        public void PassAndPass_Passes() => Passed(Test.Pass & Test.Pass);

        [Test]
        public void PassAndfail_Fails() => Failed(Test.Pass & Test.Fail);

        [Test]
        public void FailAndPass_Fails() => Failed(Test.Fail & Test.Pass);

        [Test]
        public void FailAndFail_Fails() => Failed(Test.Fail & Test.Fail);

        [Test]
        public void PassOrPass_Passes() => Passed(Test.Pass | Test.Pass);

        [Test]
        public void PassOrFail_Passes() => Passed(Test.Pass | Test.Fail);

        [Test]
        public void FailOrPass_Passes() => Passed(Test.Fail | Test.Pass);

        [Test]
        public void FailOrFail_Fails() => Failed(Test.Fail | Test.Fail);

        [Test]
        public void PassXorPass_Fails() => Failed(Test.Pass ^ Test.Pass);

        [Test]
        public void PassXorFail_Passes() => Passed(Test.Pass ^ Test.Fail);

        [Test]
        public void FailXorPass_Passes() => Passed(Test.Fail ^ Test.Pass);

        [Test]
        public void FailXorFail_Fails() => Failed(Test.Fail ^ Test.Fail);


        [TestFixture]
        public class LazyOperators
        {
            private class SideEffect
            {
                private readonly Func<Test> supplier;
                public SideEffect(Test value) => supplier = value.With(() => Happened = true);

                public bool Happened { get; private set; }
                public Test Get() => supplier();

            }

            [SetUp]
            public void SetUp()
            {
                Pass = new SideEffect(Test.Pass);
                Fail = new SideEffect(Test.Fail);
            }

            private SideEffect Pass { get; set; } 
            private SideEffect Fail { get; set; }

            

            [Test]
            public void PassAndFail_EvaluatesBoth()
            {
                Test _ = Pass.Get() && Fail.Get();

                assert.That(Pass.Happened && Fail.Happened, Is.True);
            }

            [Test]
            public void FailAndPass_ShortCircuits()
            {
                Test _ = Fail.Get() && Pass.Get();

                assert.That(Pass.Happened, Is.False);
            }

            [Test]
            public void FailOrPass_EvaluatesBoth()
            {
                Test _ = Fail.Get() || Pass.Get();

                assert.That(Pass.Happened, Is.True);
            }

            [Test]
            public void PassOrFail_ShortCircuits()
            {
                Test _ = Pass.Get() || Fail.Get();

                assert.That(Fail.Happened, Is.False);
            }

            [Test]
            public void ShortCircuitAnd_ReturnsLeftOperand()
            {
                Test left = Test.Fail;
                var right = Test.Pass.With(() => Thread.CurrentThread.Abort());

                Test result = left && right();

                assert.That(result, Is.SameAs(left));
            }

            [Test]
            public void ShortCircuitOr_ReturnsLeftOperand()
            {
                Test left = Test.Pass;
                var right = Test.Fail.With(() => Thread.CurrentThread.Abort());

                Test result = left || right();

                assert.That(result, Is.SameAs(left));
            }
        }


    }
}
