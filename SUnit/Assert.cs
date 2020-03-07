using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    public static class Assert
    {
        public static That<TActual> That<TActual>(TActual actual) => new That<TActual>(actual);
    }

    public class That<TActual>
    {
        private readonly TActual actual;

        internal That(TActual actual)
        {
            this.actual = actual;
        }

        public Is<TActual> Is => new Is<TActual>(actual);
    }
}
