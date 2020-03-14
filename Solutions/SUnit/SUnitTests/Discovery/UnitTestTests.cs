using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using nAssert = NUnit.Framework.Assert;
using System.Linq;
using System.Reflection;

namespace SUnit.Discovery
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class OutcomeAttribute : Attribute
    {
        public OutcomeAttribute(bool pass) => this.Pass = pass;

        public bool Pass { get; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class PassAttribute : OutcomeAttribute
    {
        public PassAttribute() : base(true) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FailAttribute : OutcomeAttribute
    {
        public FailAttribute() : base(false) { }
    }

    [TestFixture]
    public abstract partial class UnitTestTests
    {
        public struct Data
        {
            internal Data(UnitTest unitTest, bool expectedResult)
            {
                this.UnitTest = unitTest;
                this.Expected = expectedResult;
            }

            internal UnitTest UnitTest { get; }
            internal bool Expected { get; }
        }

        protected private abstract Type FixtureType { get; }

        [DatapointSource]
        protected private IEnumerable<Data> Tests
        {
            get
            {
                static bool? expectedOutcome(MethodInfo method)
                {
                    return method.GetCustomAttribute<OutcomeAttribute>()?.Pass;
                }

                var fixture = new Fixture(FixtureType);
                var outcomes = fixture.Tests
                    .ToDictionary(
                        method => method.Name,
                        method => method.GetCustomAttribute<OutcomeAttribute>()?.Pass);
                var unitTests = fixture.Factories
                    .SelectMany(factory => factory.CreateTests())
                    .Where(test => outcomes[test.Name] != null)
                    .Select(test => new Data(test, outcomes[test.Name].Value));

                return unitTests;
            }
        }

        [Theory]
        public void YieldsExpectedResult(Data data)
        {
            nAssert.That(data.UnitTest.Execute().Passed, Is.EqualTo(data.Expected));
        }

        [Theory]
        public void WorksAfterRoundTripSerialization(Data data)
        {
            string serialized = data.UnitTest.Save();
            UnitTest roundTripped = UnitTest.Load(serialized);

            nAssert.That(() => roundTripped.Execute().Passed, Is.EqualTo(data.Expected));
        }
    }
}
