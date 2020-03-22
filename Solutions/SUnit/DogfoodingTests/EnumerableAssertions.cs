using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUnit;

namespace DogfoodingTests
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

        public class Contains
        {
            public class ArrayContainingNull
            {
                private string[] items = new string[] { "TeamS", null, "Squishy", "Squidwards" };

                public IEnumerable<Test> Empty_ContainsNothing()
                {
                    return items.Select(t => !Assert.That(Enumerable.Empty<string>()).Contains(t));
                }

                public Test ContainsNull() 
                {
                    return Assert.That(items).Contains(null);
                }

                public Test DoesNotContainMissingNonNull()
                {
                    return !Assert.That(items).Contains("TeamU");
                }

                public Test ContainsIncludedNonNull()
                {
                    return Assert.That(items).Contains("Squidwards");
                }
            }


            public class ArrayLackingNull
            {
                private string[] items = new string[] { "Squishy", "Squidward", "TeamS" };

                public Test ContainsNonNull()
                {
                    return Assert.That(items).Contains("Squishy");
                }

                public Test DoesNotContainMissingNonNull()
                {
                    return !Assert.That(items).Contains("TeamT");
                }

                public Test DoesNotContainNull()
                {
                    return !Assert.That(items).Contains(null);
                }
            }
        }

        public class SetOperations
        {
            private static IEnumerable<IEnumerable<string>> SplitAll(IEnumerable<string> strings)
            {
                return strings.Select(s => s.Split(" "));
            }


            public class AnySequence
            {
                private IEnumerable<IEnumerable<string>> Data
                {
                    get
                    {
                        string[] lines =
                        {
                            "I love C# more than java",
                            "",
                            "Hello World",
                            "Hi everyone! Keep coding!",
                            "A B C D E F G G H I J J J",
                            "hi"
                        };

                        return SplitAll(lines);
                    }
                }

                public IEnumerable<Test> IsSupersetOfEmptySet()
                {
                    return Data.Select(actual => Assert.That(actual).Is.SupersetOf(Enumerable.Empty<string>()));
                }

                public IEnumerable<Test> IsSupersetOfItself()
                {
                    return Data.Select(actual => Assert.That(actual).Is.SupersetOf(actual));
                }

                public IEnumerable<Test> IsSubsetOfItself()
                {
                    return Data.Select(actual => Assert.That(actual).Is.SubsetOf(actual));
                }

                public IEnumerable<Test> IsNotProperSupersetOfItself()
                {
                    return Data.Select(actual => Assert.That(actual).Is.Not.ProperSupersetOf(actual));
                }

                public IEnumerable<Test> IsNotProperSubsetOfItself()
                {
                    return Data.Select(actual => Assert.That(actual).Is.Not.ProperSubsetOf(actual));
                }

                public IEnumerable<Test> IsSupersetOfItselfWithItemsRemoved()
                {
                    foreach (var items in Data)
                    {
                        yield return Assert.That(items).Is.SupersetOf(items.Skip(1));
                        yield return Assert.That(items).Is.SupersetOf(items.SkipLast(1));
                    }
                }

                public IEnumerable<Test> IsProperSupersetOfItselfWithItemsRemoved()
                {
                    foreach (var items in Data.Where(data => data.Any()))
                    {
                        yield return Assert.That(items).Is.ProperSupersetOf(items.Skip(1));
                        yield return Assert.That(items).Is.ProperSupersetOf(items.SkipLast(1));
                    }
                }

                public IEnumerable<Test> EmptySetIsNotSupersetOfNonEmpty()
                {
                    return Data.Where(t => t.Any())
                        .Select(actual => Assert.That(Enumerable.Empty<string>()).Is.Not.SupersetOf(actual));
                }

                public IEnumerable<Test> EmptySetIsSubsetOfAnySet()
                {
                    return Data.Select(actual => Assert.That(Enumerable.Empty<string>()).Is.SubsetOf(actual));
                }
            }

            public class SequenceWithDuplicates
            {
                private readonly string[] actual = "the quick the fox brown".Split(" ");
                
                public Test IsSupersetOfUniquifiedSelf()
                {
                    string[] expected = "the quick fox brown".Split(" ");

                    return Assert.That(actual).Is.SupersetOf(expected);
                }

                public Test IsProperSupersetOfUniquifiedSelf()
                {
                    string[] expected = "the quick fox brown".Split(" ");

                    return Assert.That(actual).Is.ProperSupersetOf(expected);
                }

                public Test IsNotSubsetOfUniquifiedSelf()
                {
                    string[] expected = "brown fox quick the".Split(" ");

                    return Assert.That(actual).Is.Not.SubsetOf(expected);
                }

                public Test IsNotProperSubsetOfUniquifiedSelf()
                {
                    string[] expected = "brown fox quick the".Split(" ");

                    return Assert.That(actual).Is.Not.ProperSubsetOf(expected);
                }

                public Test IsSupersetOfSelf() => Assert.That(actual).Is.SupersetOf(actual);

                public Test IsNotProperSupersetOfSelf() => Assert.That(actual).Is.Not.ProperSupersetOf(actual);

                public Test IsSubsetOfSelf() => Assert.That(actual).Is.SubsetOf(actual);

                public Test IsNotProperSubsetOfSelf() => Assert.That(actual).Is.Not.ProperSubsetOf(actual);
            }
        }
    }
}
