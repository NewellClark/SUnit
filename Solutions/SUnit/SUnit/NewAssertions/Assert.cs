using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.NewAssertions
{
    public abstract class BaseAssert
    {
        protected private BaseAssert() { }

        public static That<T> That<T>(T actual) => new That<T>(actual);
    }

    public class Assert : BaseAssert
    {
        private Assert() : base() { }

        public static DoubleThat That(double? actual) => new DoubleThat(actual);
    }
}
