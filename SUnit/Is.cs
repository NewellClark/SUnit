using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    internal delegate IsTest<TActual> TestBuilder<TActual>(IConstraint<TActual> constraint);

    //  Every constructor is internal. I don't think too many Visual Basic users
    //  will be instantiating this class.
#pragma warning disable CA1716 // Identifiers should not match keywords
    /// <summary>
    /// Contains methods and properties for applying tests to an actual value.
    /// </summary>
    /// <typeparam name="TActual"></typeparam>
    public class Is<TActual>
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        private readonly TestBuilder<TActual> builder;

        internal Is(TActual actual)
        {
            builder = constraint => new IsTest<TActual>(actual, constraint);
        }
        internal Is(TestBuilder<TActual> builder)
        {
            Debug.Assert(builder != null);

            this.builder = builder;
        }
        internal Is(TActual actual, Func<IConstraint<TActual>, IConstraint<TActual>> constraintModifier)
        {
            Debug.Assert(constraintModifier != null);

            this.builder = constraint => new IsTest<TActual>(actual, constraintModifier(constraint));
        }

        internal IsTest<TActual> ApplyConstraint(IConstraint<TActual> constraint)
        {
            Debug.Assert(constraint != null);

            return builder(constraint);
        }

        /// <summary>
        /// Inverts any <see cref="Test"/> that is applied to the current actual value.
        /// </summary>
        public Is<TActual> Not => new Is<TActual>(constraint => ApplyConstraint(!constraint));

        /// <summary>
        /// Tests if the actual value is equal to the expected value.
        /// </summary>
        /// <param name="expected">The value that we expect the actual value to be equal to.</param>
        /// <returns></returns>
        public IsTest<TActual> EqualTo(TActual expected)
        {
            return ApplyConstraint(new EqualToConstraint<TActual>(expected));
        }

        /// <summary>
        /// Tests whether the actual value is <see langword="null"/>.
        /// </summary>
        public IsTest<TActual> Null => ApplyConstraint(new NullConstraint<TActual>());
    }

    
}
