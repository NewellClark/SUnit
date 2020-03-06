using SUnit;
using System;

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
    }
}
