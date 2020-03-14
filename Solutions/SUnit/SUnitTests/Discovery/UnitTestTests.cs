using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using nAssert = NUnit.Framework.Assert;
using System.Linq;
using System.Reflection;
using SUnit.Discovery.Results;

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
            internal Data(UnitTest unitTest, ResultKind expectedResult)
            {
                this.UnitTest = unitTest;
                this.Expected = expectedResult;
            }

            internal UnitTest UnitTest { get; }
            internal ResultKind Expected { get; }

            public override string ToString() => UnitTest.ToString();
        }

        protected private abstract Type FixtureType { get; }

        [DatapointSource]
        protected private IEnumerable<Data> Tests
        {
            get
            {
                static ResultKind fromBool(bool value) => value ? ResultKind.Pass : ResultKind.Fail;

                var fixture = new Fixture(FixtureType);
                var outcomes = fixture.Tests
                    .ToDictionary(
                        method => method.Name,
                        method => method.GetCustomAttribute<OutcomeAttribute>()?.Pass);

                var unitTests = fixture.Factories
                    .SelectMany(factory => factory.CreateTests())
                    .Where(test => outcomes[test.Name] != null)
                    .Select(test => new Data(test, fromBool(outcomes[test.Name].Value)));

                return unitTests;
            }
        }

        [Theory]
        public void YieldsExpectedResult(Data data)
        {
            nAssert.That(TestRunner.RunTest(data.UnitTest).Kind, Is.EqualTo(data.Expected));
        }

        [Theory]
        public void WorksAfterRoundTripSerialization(Data data)
        {
            string serialized = data.UnitTest.Save();
            UnitTest roundTripped = UnitTest.Load(serialized);

            nAssert.That(TestRunner.RunTest(roundTripped).Kind, Is.EqualTo(data.Expected));
        }
    }
}
