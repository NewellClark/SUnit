using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nAssert = NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    [TestFixture]
    public class TraitPairTests
    {
        [DatapointSource]
        protected IEnumerable<(string name, string value)> Pairs
        {
            get
            {
                return new (string, string)[]
                {
                    ("Squishy", "Squidward"),
                    ("32,7:Squishy", "31,51:Squidward"),
                    ("   hello world", "everyone\r\nsquidward"),
                    ("\n\n       \r\n  hell", "sanetudsacndSND ||||RC:::::;;;;;;,,,,,,......10987417109"), 
                    ("Alice and Bob", "Spongebob Squarepants"),
                    ("Elictivere", "kinda sucks")
                };
            }
        }

        [TestFixture]
        public class SinglePair : TraitPairTests
        {
            [Theory]
            public void IsRoundTripSerializable((string name, string value) data)
            {
                var pair = new TraitPair(data.name, data.value);
                TraitPair roundTripped = TraitPair.Parse(pair.ToString());

                nAssert.That(pair, Is.EqualTo(roundTripped));
            }
        }

        [TestFixture]
        public class FlatPairList : TraitPairTests
        {
            private readonly IEnumerable<TraitPair> pairs;

            public FlatPairList()
            {
                pairs = Pairs.Select(t => new TraitPair(t.name, t.value)).ToList();
            }

            [Test]
            public void CanBeRoundTripped()
            {
                string serialized = pairs.Aggregate(
                    new StringBuilder(),
                    (sb, pair) => { 
                        sb.Append(pair.ToString()); 
                        return sb; 
                    })
                    .ToString();

                var roundTripped = TraitPair.ParseAll(serialized);

                CollectionAssert.AreEqual(pairs, roundTripped);
            }
        }

        [TestFixture]
        public class NestedPairs
        {
            private readonly TraitPair inner;
            private readonly TraitPair outer;
            private readonly TraitPair roundTrippedOuter;
            public NestedPairs()
            {
                inner = new TraitPair("AbsoluteValue", "This is the value of the absolute property");
                outer = new TraitPair("Serialized Inner Trait Pair", inner.ToString());
                roundTrippedOuter = TraitPair.Parse(outer.ToString());
            }

            [Test]
            public void OuterPair_IsRoundTripSerializeable()
            {
                nAssert.That(roundTrippedOuter, Is.EqualTo(outer));
            }

            [Test]
            public void InnerPair_IsRoundTripSerializableFromOuterValue()
            {
                var roundTrippedInner = TraitPair.Parse(roundTrippedOuter.Value);

                nAssert.That(roundTrippedInner, Is.EqualTo(inner));
            }
        }
    }
}
