using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUnit;

namespace NewellClark.Collections.Tests
{
    //  Any public class that contains unit tests is a "Test Fixture".
    public class MultidictionaryTests
    {
        public class NewMultidictionary
        {
            private readonly Multidictionary<string, string> dictionary = new Multidictionary<string, string>();
            private readonly string[] keys = new[] 
            { 
                "hello", "world", "squishy",  
                "squidward", "is", "number", "one"
            };

            public Test HasCountZero() => Assert.That(dictionary.Count).Is.Zero;

            public Test HasNoItems() => Assert.That(dictionary).Is.Empty;

            public IEnumerable<Test> Indexer_ReturnsEmptySequence()
            {
                return keys
                    .Where(key => key != null)
                    .Select(key => Assert.That(dictionary[key]).Is.Empty);
            }
        }

        public class AfterSingleAdd
        {
            private readonly Multidictionary<string, string> dictionary = new Multidictionary<string, string>();
            private readonly string key = "Hello";
            private readonly string value = "World";
            public AfterSingleAdd()
            {
                dictionary.Add(key, value);
            }

            public Test HasOneItem() => Assert.That(dictionary.Count).Is.EqualTo(1);

            public Test HasSingleElement()
            {
                return Assert.That(dictionary.Single().Key).Is.EqualTo(key) &&
                    Assert.That(dictionary.Single().Value).Is.SequenceEqualTo(value);
            }

            public Test IndexerFetchesElement()
            {
                return Assert.That(dictionary[key]).Is.SequenceEqualTo(value);
            }
        }
    }
}
