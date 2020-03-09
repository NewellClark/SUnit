using SUnit.ActualValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// Specifies the actual value that was returned from whatever it is you are testing.
    /// Allows you to write code like <code>Assert.That(actual).Is.EqualTo(expected);</code>.
    /// </summary>
    /// <typeparam name="TActual">The type of the actual value.</typeparam>
    public class That<TActual>
    {
        private readonly TActual actual;

        internal That(TActual actual)
        {
            this.actual = actual;
        }

        /// <summary>
        /// Contains methods and properties for applying contstraints to the actual value specified in the 
        /// <see cref="Assert.That{TActual}(TActual)"/> method.
        /// </summary>
        public IIsThing<TActual> Is => new IsThing<TActual>(actual);
    }
}
