using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUnit;

namespace ActualLegitUnitTests
{
    public class EnumerableAssertions
    {
        private static T[] Iter<T>(params T[] items) => items;
        private static readonly IEnumerable<int> @null = null;

        public class SetEqualTo
        {
            public Test AnEmptyArray_IsEmpty() => Assert.That(Array.Empty<object>()).Is.Empty;
            
            public Test NullSequence_IsNotEmpty()
            {
                return Assert.That((int[])null).Is.Not.Empty;
            }

            public Test EmptySequences_AreSetEqual()
            {
                return Assert.That(Enumerable.Empty<object>()).Is.SetEqualTo(Array.Empty<object>());
            }

            public Test OrderDoesNotMatter_ForSetEquality()
            {
                var actual = new string[] { "Hello", "World", "Everyone" };
                var expected = new string[] { "Everyone", "Hello", "World" };

                return Assert.That(actual).Is.SetEqualTo(expected);
            }

            public Test DuplicatesDoNotMatter_ForSetEquality()
            {
                var actual = new string[] { "baby", "baby", "goats", "carrots" };
                var expected = new string[] { "carrots", "carrots", "carrots", "baby", "goats", "goats", "goats" };

                return Assert.That(actual).Is.SetEqualTo(expected);
            }

            public Test NonEqualSets_AreNotEqual()
            {
                var actual = new string[] { "C#", "Java", "C++" };
                var expected = new string[] { "C#", "C++" };

                return Assert.That(actual).Is.Not.SetEqualTo(expected);
            }

            public Test NullIsSetEqualToNull()
            {
                IEnumerable<string> @null = null;

                return Assert.That(@null).Is.SetEqualTo(@null);
            }
        }

        public class SequenceEqualTo
        {
            public Test Null_Equals_Null() => Assert.That(@null).Is.SequenceEqualTo(@null);

            public Test Empty_Equals_Empty() => Assert.That(Enumerable.Empty<object>())
                .Is.SequenceEqualTo(Array.Empty<object>());

            public Test Null_IsNotEqualToEmpty() => Assert.That(@null).Is.Not.SequenceEqualTo(Array.Empty<int>());

            public Test Empty_IsNotEqualToNull() => Assert.That(Array.Empty<int>()).Is.Not.SequenceEqualTo(@null);

            public Test SameElements_DifferentOrder_AreUnequal()
            {
                return Assert.That(Iter(1, 2, 3))
                    .Is.Not.SequenceEqualTo(Iter(2, 3, 1));
            }

            public Test ActualSequenceShorter()
            {
                return Assert.That(Iter(1, 2, 3))
                    .Is.Not.SequenceEqualTo(Iter(1, 2, 3, 4));
            }

            public Test ExpectedSequenceShorter()
            {
                return Assert.That(Iter(1, 2, 3, 4))
                    .Is.Not.SequenceEqualTo(Iter(1, 2, 3));
            }

            public Test Empty_NotEqualTo_NotEmpty()
            {
                return Assert.That(Array.Empty<int>()).Is.Not.SequenceEqualTo(Iter(69));
            }

            public Test NotEmpty_NotEqualTo_Empty()
            {
                return Assert.That(Iter(61)).Is.Not.SequenceEqualTo(Iter<int>());
            }
        }
        
        public class EquivalentTo
        {
            public Test Empty_IsEquivalentTo_Empty()
            {
                return Assert.That(Iter<int>()).Is.EquivalentTo(Iter<int>());
            }

            public Test Null_IsEquivalentTo_Null()
            {
                return Assert.That(@null).Is.EquivalentTo(@null);
            }

            public Test Empty_IsNotEquivalentTo_Null()
            {
                return Assert.That(Iter<int>()).Is.Not.EquivalentTo(@null) &
                    Assert.That(@null).Is.Not.EquivalentTo(Iter<int>());
            }

            public Test OrderDoesNotMatter()
            {
                return Assert.That(Iter(4, 69, -1000))
                    .Is.EquivalentTo(Iter(69, -1000, 4));
            }

            public Test DuplicatesDoMatter()
            {
                return Assert.That(Iter(1, 3, 7, 8, 3))
                    .Is.Not.EquivalentTo(Iter(7, 7, 8, 3, 1, 1));
            }

            public Test Empty_IsNotEquivalentToNonEmpty()
            {
                return Assert.That(Iter<int>()).Is.Not.EquivalentTo(Iter(71)) &
                    Assert.That(Iter(14, -123)).Is.Not.EquivalentTo(Iter<int>());
            }

            public Test SingleNullItems_InEquivalentSequences()
            {
                return Assert.That(Iter("hello", null, "mass", "hole"))
                    .Is.EquivalentTo(Iter(null, "hole", "hello", "mass"));
            }

            public Test DuplicateNulls_PreventPassingtest()
            {
                return Assert.That(Iter("Hi", null, null, null, "Baby"))
                    .Is.Not.EquivalentTo(Iter("Baby", null, null, "Hi"));
            }
        }
    }
}
