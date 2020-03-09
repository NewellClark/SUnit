using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit
{
    public static class Helpers
    {
        public static void AssertPassed(Test test) => assert.That(test.Passed, Is.True);

        public static void AssertFailed(Test test) => assert.That(test.Passed, Is.False);
    }
}
