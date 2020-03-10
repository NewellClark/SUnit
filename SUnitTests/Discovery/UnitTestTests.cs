using NUnit.Framework;
using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    [TestFixture]
    public class UnitTestTests
    {
        private class Mock
        {
            public Mock() { }

            public Test Errors()
            {
                object @null = null;

                return Assert.That(@null.ToString()).Is.EqualTo("Hello World");
            }

            public Test Passes() => Assert.That(2 + 2).Is.EqualTo(4);

            public Test Fails() => Assert.That(2 + 2).Is.EqualTo(5);
        }

        private readonly Dictionary<string, UnitTest> tests;

        public UnitTestTests()
        {
            var fixture = new Fixture(typeof(Mock));
            var factory = fixture.Factories.Single();
            tests = fixture.Tests
                .ToDictionary(
                    method => method.Name,
                    method => new UnitTest(method, factory));
        }

        [Test]
        public void PassingTest_YieldsPassedResult()
        {
            var test = tests[nameof(Mock.Passes)];
            var result = test.Run();

            assert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
        }

        [Test]
        public void FailingTest_YieldsFailingResult()
        {
            var test = tests[nameof(Mock.Fails)];
            var result = test.Run();

            assert.That(result.Kind, Is.EqualTo(ResultKind.Fail));
        }

        [Test]
        public void ThrowingTest_YieldsErrorResult()
        {
            var test = tests[nameof(Mock.Errors)];
            var result = test.Run();

            assert.That(result.Kind, Is.EqualTo(ResultKind.Error));
        }
    }
}
