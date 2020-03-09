using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The return type of <see cref="That{TActual}.Is"/>. Contains methods and properties for
    /// performing assertions on an actual value.
    /// </summary>
    /// <typeparam name="T">The type of the actual value under test.</typeparam>
    /// <typeparam name="TIs">The return type of <see cref="That{TActual}.Is"/>.</typeparam>
    /// <typeparam name="TTest">The type of <see cref="Test"/> that is created when 
    /// constraints are applied to <typeparamref name="TIs"/>.</typeparam>
    public interface IIsExpression<T, TIs, TTest> : IActualValueExpression<T, TIs, TTest>
        where TIs : IActualValueExpression<T, TIs, TTest>
        where TTest : ActualValueTest<T, TIs, TTest>
    {
        /// <summary>
        /// Asserts that the actual value is equal to the specified expected value.
        /// </summary>
        /// <param name="expected">The value that is expected.</param>
        /// <returns>A <see cref="Test"/> that passes if the actual value is equal to <paramref name="expected"/>.</returns>
        public TTest EqualTo(T expected)
        {
            return ApplyConstraint(new EqualToConstraint<T>(expected));
        }

        /// <summary>
        /// Returns a <see cref="Test"/> that passes if the actual value is <see langword="null"/>.
        /// </summary>
        public TTest Null
        {
            get => ApplyConstraint(new NullConstraint<T>());
        }
    }





}
