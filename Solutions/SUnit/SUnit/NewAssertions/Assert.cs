using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.NewAssertions
{
    public static class Assert
    {
        public static That<T> That<T>(T actual) => new That<T>(actual);

        public static DoubleThat That(double? actual) => new DoubleThat(actual);
    }
}
