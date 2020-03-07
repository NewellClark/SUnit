using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    public abstract class Test
    {
        public abstract bool Passed { get; }

        private sealed class NotTest : Test
        {
            private readonly Test inner;
            public NotTest(Test inner) => this.inner = inner;

            public override bool Passed => !inner.Passed;
        }
        
        public static Test operator !(Test operand)
        {
            if (operand is null) throw new ArgumentNullException(nameof(operand));

            return new NotTest(operand);
        }
    }
}
