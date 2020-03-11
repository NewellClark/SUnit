using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
