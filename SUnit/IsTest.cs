using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit
{
    ///// <summary>
    ///// A test produced from applying a constraint to an Is value.
    ///// </summary>
    ///// <typeparam name="TActual">The type of the actual value under test.</typeparam>
    //public sealed class IsTest<TActual> : Test
    //{
    //    private readonly TActual actual;
    //    private readonly IConstraint<TActual> constraint;

    //    internal IsTest(TActual actual, IConstraint<TActual> constraint)
    //    {
    //        Debug.Assert(constraint != null);

    //        this.actual = actual;
    //        this.constraint = constraint;
    //    }

    //    /// <summary>
    //    /// Indicates whether the test passed.
    //    /// </summary>
    //    public override bool Passed => constraint.Apply(actual);

    //    /// <summary>
    //    /// Overridden to output the constraint, and the actual value.
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString() => $"Expected {constraint}\nWas {PrintActualValue(actual)}";

    //    private static string PrintActualValue(TActual actual)
    //    {
    //        return actual == null ? "<null>" : actual.ToString();
    //    }

    //    /// <summary>
    //    /// Allows multiple constraints to be chained to the same value using boolean AND.
    //    /// </summary>
    //    public Is<TActual> And => new Is<TActual>(actual, c => constraint & c);

    //    /// <summary>
    //    /// Allows chaining multiple constraints to the same value using boolean OR.
    //    /// </summary>
    //    public Is<TActual> Or => new Is<TActual>(actual, c => constraint | c);

    //    /// <summary>
    //    /// Allows chaining multiple constraints to the same value using boolean XOR.
    //    /// </summary>
    //    public new Is<TActual> Xor => new Is<TActual>(actual, c => constraint ^ c);
    //}
}
