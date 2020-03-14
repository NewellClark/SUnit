
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
        /// Contains methods and properties for applying contstraints to the actual value specified.
        /// </summary>
        public IIsExpression<TActual> Is => new IsExpression<TActual>(Actual);
    }

    /// <summary>
    /// The type that allows you to say "Is" and apply constraints to sequences.
    /// </summary>
    /// <typeparam name="T">The type of element in the actual value sequence.</typeparam>
    public class ThatEnumerable<T> : That<IEnumerable<T>>
    {
        internal ThatEnumerable(IEnumerable<T> actual) : base(actual) { }

        /// <summary>
        /// Allows you to apply constraints to sequences.
        /// </summary>
        public new IIsExpressionEnumerable<T> Is => new IsExpressionEnumerable<T>(Actual);
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
