using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    public class IsTest<TActual> : Test
    {
        private readonly TActual actual;
        private readonly IConstraint<TActual> constraint;

        internal IsTest(TActual actual, IConstraint<TActual> constraint)
        {
            Debug.Assert(constraint != null);

            this.actual = actual;
            this.constraint = constraint;
        }

        public override bool Passed => constraint.Apply(actual);
        
        public Is<TActual> And => new Is<TActual>(actual, c => constraint & c);
        public Is<TActual> Or => new Is<TActual>(actual, c => constraint | c);
        public Is<TActual> Xor => new Is<TActual>(actual, c => constraint ^ c);
    }
}
