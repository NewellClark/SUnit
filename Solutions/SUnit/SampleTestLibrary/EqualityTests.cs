using SUnit;
using System;
using System.Collections.Generic;

namespace SampleTestLibrary
{
    public class EqualityTests
    {
        public EqualityTests() { }

        public Test TwoPlusTwo_IsFour() => Assert.That(2 + 2).Is.EqualTo(4);

        public Test HelloPlusWorld_IsHelloWorld()
        {
            return Assert.That("Hello" + "World").Is.EqualTo("HelloWorld");
        }

        public Test TwoPlusTwo_IsActuallyFive() => Assert.That(2 + 2).Is.EqualTo(5);

        public Test DereferencesNull()
        {
            List<int> @null = null;
            return Assert.That(@null.Count).Is.EqualTo(0);
        }

        public Test LessThanFour_And_GreaterThanSix()
        {
            return Assert.That(5).Is.LessThan(4).And.GreaterThan(6);
        }

        public Test TotallyNotNasty()
        {
            throw new OutOfMemoryException("Not Nasty hehe");
        }

        public Test ToUnderstandRecursion()
        {
            throw new StackOverflowException("You have to understand recursion");
        }

        public Test CompoundTest()
        {
            return Assert.That(4).Is.LessThanOrEqualTo(5) &
                Assert.That(5).Is.GreaterThan(4).And.EqualTo(2 + 3).And.LessThan(3);
        }

        public Test HelloShowsUp() => Assert.That(4).Is.Not.Hello.EqualTo(7);
    }
}
