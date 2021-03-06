﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    [TestFixture]
    public class FixtureTests
    {
        private class Mock
        {
            public Mock() : this(nameof(Mock)) { }
            private Mock(string ctor) => CtorName = ctor;

            public static Mock AlphaCtor() => new Mock(nameof(AlphaCtor));
            public static Mock BravoCtor() => new Mock(nameof(BravoCtor));
            public static Mock CharlieCtor() => new Mock(nameof(CharlieCtor));
            public static Mock HasArguments(string trololololol) => new Mock(nameof(HasArguments));
            public Mock InstanceCtor() => new Mock(nameof(InstanceCtor));
            public static Mock IsGeneric<T>() => new Mock(nameof(IsGeneric));
            internal static Mock NonPublicCtor() => new Mock(nameof(NonPublicCtor));

            public string CtorName { get; }
        }

        private readonly Fixture fixture = new Fixture(typeof(Mock));

        [Test]
        public void Factories_IncludesPublicDefaultAndNamedCtors()
        {
            var actualNames = fixture.Factories
                .Select(fact => fact.Build())
                .Cast<Mock>()
                .Select(mock => mock.CtorName);
            var expected = new string[]
            {
                nameof(Mock.AlphaCtor), nameof(Mock.BravoCtor),
                nameof(Mock.CharlieCtor), nameof(Mock)
            };

            assert.That(actualNames, Is.EquivalentTo(expected));
        }

        [Test]
        public void Fixture_IsRoundTripSerializableAsText()
        {
            string text = fixture.Save();
            var roundTripped = Fixture.Load(text);

            assert.That(roundTripped.Type, Is.EqualTo(fixture.Type));
        }
    }
}
