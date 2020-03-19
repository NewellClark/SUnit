using System;
using System.Text;

namespace SUnit.Assertions
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
            Actual = actual;
        }

        /// <summary>
        /// Contains methods and properties for applying contstraints to the actual value specified.
        /// </summary>
        public IIsExpression<TActual> Is => new IsExpression<TActual>(Actual);
    }

    /// <summary>
    /// Specifies the actual value that was returned from whatever it is you are testing.
    /// Allows you to write code like <code>Assert.That(actual).Is.EqualTo(expected);</code>.
    /// </summary>
    public class ThatDouble : That<double?>
    {
        internal ThatDouble(double? actual) : base(actual) { }

        /// <summary>
        /// Contains methods and properties for applying constraints to the actual value specified in
        /// the <see cref="Assert.That(double)"/> method.
        /// </summary>
        public new IIsExpressionDouble Is => new IsExpressionDouble(Actual);
    }

    /// <summary>
    /// Specifies the actual value that was returned from whatever it is you are testing.
    /// Allows you to write code like <code>Assert.That(actual).Is.EqualTo(expected);</code>.
    /// </summary>
    public class ThatDecimal : That<decimal?>
    {
        internal ThatDecimal(decimal? actual) : base(actual) { }

        /// <summary>
        /// Contains methods and properties for applying constraints to the actual value specified
        /// in the <see cref="Assert.That(decimal)"/> method.
        /// </summary>
        public new IIsExpressionDecimal Is => new IsExpressionDecimal(Actual);
    }

    /// <summary>
    /// Specifies the actual value that was returned from whatever it is you are testing.
    /// Allows you to write code like <code>Assert.That(actual).Is.EqualTo(expected);</code>.
    /// </summary>
    public class ThatLong : That<long?>
    {
        internal ThatLong(long? actual) : base(actual) { }

        /// <summary>
        /// Contains methods and properties for applying constraints to the actual value specified
        /// in the <see cref="Assert.That(long)"/> method.
        /// </summary>
        public new IIsExpressionLong Is => new IsExpressionLong(Actual);
    }

    /// <inheritdoc/>
    public class ThatBool : That<bool?>
    {
        internal ThatBool(bool? actual) : base(actual) { }

        /// <inheritdoc/>
        public new IIsExpressionBool Is => new IsExpressionBool(Actual);
    }
}
