using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    //  The only purpose of this class is to provide the type of the Is property 
    //  on the That class, as in Assert.That(actual).Is.EqualTo(expected);
    //  Since the constructor is internal, and there are no useful static methods on this class, 
    //  the fact that Visual Basic programmers can't speak the name of this class should
    //  not be an issue.
    //  Sure, NUnit had to make a class called "Iz", but the way they have things set up, you actually have 
    //  to say "Is" directly, rather than just saying "Is" as a property. 
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Is<TValue>
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        internal Is(TValue actual)
        {
            this.Actual = actual;
        }

        internal TValue Actual { get; }

        public Test EqualTo(TValue expected) => new EqualityTest<TValue>(expected, Actual);

        public Test Null => new EqualityTest<object>(null, Actual);
    }

    public class IsBool : Is<bool>
    {
        internal IsBool(bool actual) : base(actual) { }

        public Test True => new EqualityTest<bool>(true, Actual);

        public Test False => new EqualityTest<bool>(false, Actual);
    }
}
