using System.Collections.Generic;

namespace SUnit.Assertions
{
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
}
