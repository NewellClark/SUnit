
using SUnit.Assertions;
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
        protected private TActual Actual { get; }

        internal That(TActual actual)
        {
            this.Actual = actual;
        }

        /// <summary>
        /// Contains methods and properties for applying contstraints to the actual value specified in the 
        /// <see cref="Assert.That{TActual}(TActual)"/> method.
        /// </summary>
        public IIsExpression<TActual> Is => new IsExpression<TActual>(Actual);
    }

    public class ThatDouble : That<double?>
    {
        internal ThatDouble(double? actual) : base(actual) { }
        public new IIsExpressionDouble Is => new IsExpressionDouble(Actual);
    }

    public class ThatDecimal : That<decimal?>
    {
        internal ThatDecimal(decimal? actual) : base(actual) { }

        public new IIsExpressionDecimal Is => new IsExpressionDecimal(Actual);
    }

    public class ThatLong : That<long?>
    {
        internal ThatLong(long? actual) : base(actual) { }

        public new IIsExpressionLong Is => new IsExpressionLong(Actual);
    }
}
