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
        public class EmptyMultidictionary
        {
            private readonly Multidictionary<string, string> dictionary = new Multidictionary<string, string>();
            private readonly string[] keys = new[] 
            { 
                "hello", "world", "squishy",  
                "squidward", "is", "number", "one"
            };

            public Test HasEntryCountZero() => Assert.That(dictionary.Count).Is.Zero;

            public Test HasKeyCountZero() => Assert.That(dictionary.Keys.Count).Is.Zero;

            public Test HasNoEntries() => Assert.That(dictionary).Is.Empty;

            public Test HasNoKeys() => Assert.That(dictionary.Keys).Is.Empty;

            public IEnumerable<Test> Indexer_ReturnsEmptySequence()
            {
                return keys
                    .Where(key => key != null)
                    .Select(key => Assert.That(dictionary[key]).Is.Empty);
            }

            public IEnumerable<Test> DoesNotContainAnyKeys()
            {
                foreach (string key in keys)
                    yield return Assert.That(dictionary.ContainsKey(key)).Is.False;
            }

            public IEnumerable<Test> DoesNotRemoveAnyKeys()
            {
                foreach (string key in keys)
                    yield return Assert.That(dictionary.Remove(key)).Is.False;
            }

            public Test Add_ThrowsWhenKeyNull()
            {
                return Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, "whatever"));
            }

            public Test AddRange_ThrowsWhenKeyNull()
            {
                return Assert.Throws<ArgumentNullException>(() => dictionary.AddRange(null, Enumerable.Empty<string>()));
            }

            public Test AddRange_ThrowsWhenCollectionNull()
            {
                return Assert.Throws<ArgumentNullException>(() => dictionary.AddRange("Not allowed", null));
            }

            public Test Remove_key_ThrowsWhenKeyNull()
            {
                return Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null));
            }

            public Test Remove_key_value_ThrowsWhenKeyNull()
            {
                return Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null, "not null"));
            }
        }


        public class SingletonMultidictionary
        {
            private readonly Multidictionary<string, string> dictionary = new Multidictionary<string, string>();
            private readonly string key = "Hello";
            private readonly string value = "World";
            public SingletonMultidictionary()
            {
                dictionary.Add(key, value);
            }

            public Test HasEntryCountOne() => Assert.That(dictionary.Count).Is.EqualTo(1);

            public Test HasKeyCountOne() => Assert.That(dictionary.Keys.Count).Is.EqualTo(1);

            public Test HasAddedEntry()
            {
                var entry = dictionary.Single();

                return Assert.That(entry.Key).Is.EqualTo(key) &&
                    Assert.That(entry).Is.SequenceEqualTo(value);
            }

            public Test ContainsSingleKey()
            {
                return Assert.That(dictionary.Keys)
                    .Is.EquivalentTo(key);
            }
            
            public Test HasAddedValue()
            {
                return Assert.That(dictionary.Single())
                    .Is.SequenceEqualTo(value);
            }

            public Test IndexerReturnsSingleValue()
            {
                return Assert.That(dictionary[key]).Is.SequenceEqualTo(value);
            }

            public Test RemoveExistingKey_LowersCount()
            {
                dictionary.Remove(key);

                return Assert.That(dictionary.Count).Is.Zero;
            }

            public Test RemoveExistingKey_Succeeds()
            {
                return Assert.That(dictionary.Remove(key)).Is.True;
            }
        }


        public class AddingDistinctItemsToSingleKey
        {
            private readonly Multidictionary<string, string> dictionary;

            public AddingDistinctItemsToSingleKey()
            {
                dictionary = new Multidictionary<string, string>
                {
                    { "teamS", "#1" },
                    { "teamS", "rules" },
                    { "teamS", "cargo" }
                };
            }

            public Test ContainsSingleKey()
            {
                return Assert.That(dictionary.Keys).Is.EquivalentTo("teamS");
            }

            public Test DoesNotIncreaseEntryCount() => Assert.That(dictionary.Count).Is.EqualTo(1);

            public Test DoesNotIncreaseKeyCollectionCount()
            {
                return Assert.That(dictionary.Keys.Count).Is.EqualTo(1);
            }

            public Test GroupsValuesWithKey()
            {
                return Assert.That(dictionary["teamS"])
                    .Is.EquivalentTo("rules", "#1", "cargo");
            }

            public Test IncreasesGroupSize()
            {
                return Assert.That(dictionary["teamS"].Count).Is.EqualTo(3);
            }

            public Test Remove_ExistingValue_ReturnsTrue()
            {
                return Assert.That(dictionary.Remove("teamS", "cargo")).Is.True;
            }

            public Test Remove_ExistingValue_RemovesValueFromGroup()
            {
                dictionary.Remove("teamS", "cargo");

                return Assert.That(dictionary["teamS"]).Is.EquivalentTo("#1", "rules");
            }
        }

        public class MultidictionaryWithItemsRemoved
        {
            private readonly Multidictionary<string, int> dictionary;

            public MultidictionaryWithItemsRemoved()
            {
                dictionary = new Multidictionary<string, int>();
                dictionary.AddRange("SUnit", new int[] { 50, 100, 150 });
                dictionary.AddRange("hello", new int[] { 7 });
                dictionary.AddRange("TeamS", new int[] { 420, 69 });

                dictionary.Remove("hello", 7);

            }

            public IEnumerable<Test> HasNoEmptyGroupsWhenEnumerated()
            {
                return dictionary
                    .Select(group => Assert.That(group).Is.Not.Empty);
            }

            public Test HasEntryCountUpdatedToReflectRemovedGroups()
            {
                return Assert.That(dictionary.Count).Is.EqualTo(2);
            }
        }
    }
}
