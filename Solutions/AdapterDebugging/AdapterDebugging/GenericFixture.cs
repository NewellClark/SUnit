using SUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterDebugging
{
    public class GenericFixture<T> 
    {
        private readonly T expected;
        private readonly T actual;

        public GenericFixture(T expected, T actual)
        {
            this.expected = expected;
            this.actual = actual;
        }

        public static GenericFixture<int> EqualIntegers() => new GenericFixture<int>(17, 17);

        public static GenericFixture<int> UnequalIntegers() => new GenericFixture<int>(9, 14);

        public static GenericFixture<string> NullNonNullStrings() => new GenericFixture<string>(null, "hello");

        public static GenericFixture<string> ValueEqualStrings()
        {
            DateTime current = DateTime.Now;
            string left = current.ToString();
            string right = current.ToString();

            return new GenericFixture<string>(left, right);
        }

        public static GenericFixture<string> ReferenceEqualStrings()
        {
            string text = "Reference Equal";
            return new GenericFixture<string>(text, text);
        }

        public Test EqualTo() => Assert.That(actual).Is.EqualTo(expected);

        public Test NotEqualTo() => Assert.That(actual).Is.Not.EqualTo(expected);
    }
}
