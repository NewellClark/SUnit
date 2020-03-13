using NUnit.Framework;
using SUnit.Discovery;
using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.TestAdapter
{
    internal class Mock
    {
        public Mock() => ConstructedBy = DefaultCtorName;
        public static Mock Alpha()
        {
            Mock result = new Mock();
            result.ConstructedBy = nameof(Alpha);
            return result;
        }

        public Test WasConstructedBy_Default()
        {
            return Assert.That(ConstructedBy).Is.EqualTo(DefaultCtorName);
        }

        public Test WasConstructedBy_Alpha()
        {
            return Assert.That(ConstructedBy).Is.EqualTo(nameof(Alpha));
        }

        public string ConstructedBy { get; private set; }

        public static string DefaultCtorName { get; } = "DefaultCtor";
    }


    //[TestFixture]
    //public class LiteTestTests
    //{
    //    private readonly UnitTest defaultCtorTest;
    //    private readonly UnitTest namedCtorTest;

    //    public LiteTestTests()
    //    {
    //        var fixture = new Fixture(typeof(Mock));

    //        defaultCtorTest = fixture.Factories
    //            .Where(fact => fact.IsDefaultConstructor)
    //            .Single()
    //            .CreateTests()
    //            .Where(test => test.Name == nameof(Mock.WasConstructedBy_Default))
    //            .Single();

    //        namedCtorTest = fixture.Factories
    //            .Where(fact => fact.IsNamedConstructor)
    //            .Single()
    //            .CreateTests()
    //            .Where(test => test.Name == nameof(Mock.WasConstructedBy_Alpha))
    //            .Single();
    //    }

    //    [Test]
    //    public void CanRunDefaultCtor()
    //    {
    //        var lite = new LiteTest(defaultCtorTest);
    //        var result = lite.Run();

    //        nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
    //    }

    //    [Test]
    //    public void CanRunNamedCtor()
    //    {
    //        var lite = new LiteTest(namedCtorTest);
    //        var result = lite.Run();

    //        nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
    //    }

    //    [Test]
    //    public void CanRunDefaultCtor_AfterRoundTrip()
    //    {
    //        var original = new LiteTest(defaultCtorTest);
    //        var lines = original.ToLines();
    //        var roundTripped = new LiteTest(lines);

    //        var result = roundTripped.Run();

    //        nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
    //    }

    //    [Test]
    //    public void CanRunNamedCtor_AfterRoundTrip()
    //    {
    //        var original = new LiteTest(namedCtorTest);
    //        var lines = original.ToLines();
    //        var roundTripped = new LiteTest(lines);

    //        var result = roundTripped.Run();

    //        nAssert.That(result.Kind, Is.EqualTo(ResultKind.Pass));
    //    }
    //}
}
