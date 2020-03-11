using SUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdapterDebugging
{
    public class NamedConstructors
    {
        private readonly string left;
        private readonly string right;

        private NamedConstructors(string left, string right)
        {
            this.left = left;
            this.right = right;
        }

        public static NamedConstructors AlphaBeta() => new NamedConstructors("Alpha", "Beta");
        public static NamedConstructors NullNull() => new NamedConstructors(null, null);
        public static NamedConstructors CharlieNull() => new NamedConstructors("Charlie", null);
        public static NamedConstructors NullBravo() => new NamedConstructors(null, "Bravo");
        public static NamedConstructors BobBob() => new NamedConstructors("Bob", "Bob");

        public Test LeftEqualsRight() => Assert.That(left).Is.EqualTo(right);
        public Test LeftNotEqualToRight() => Assert.That(left).Is.Not.EqualTo(right);
        public Test LeftLengthEqualToRightLength() => Assert.That(left?.Length).Is.EqualTo(right?.Length);
        public Test LeftLengthNotEqualToRightLength() => Assert.That(left?.Length).Is.Not.EqualTo(right?.Length);
        public Test LeftIsNull() => Assert.That(left).Is.Null;
        public Test RightIsNull() => Assert.That(right).Is.Null;
        public Test LeftIsNotNull() => Assert.That(left).Is.Not.Null;
        public Test RightIsNotNull() => Assert.That(right).Is.Not.Null;
    }
}
