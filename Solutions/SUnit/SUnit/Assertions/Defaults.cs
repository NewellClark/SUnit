﻿using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SUnit.Assertions
{
    /// <summary>
    /// The return value of <see cref="That{T}.Is"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value that is under test.</typeparam>
    public interface IIsExpression<T> : IIsExpression<T, IIsExpression<T>, ValueTest<T>> { }

    /// <summary>
    /// The type that lets you say <see cref="Is"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value under test.</typeparam>
    public class That<T>
    {
        internal That(T actual) 
            : this(new BasicValueExpression<T>(actual, c => c)) { }

        internal That(IValueExpression<T> expression)
        {
            Debug.Assert(expression != null);

            this.Expression = expression;
        }

        protected private IValueExpression<T> Expression { get; }

        /// <summary>
        /// Contains methods for performing assertions on the value under test.
        /// </summary>
        public IIsExpression<T> Is => new BasicIsExpression<T>(Expression);
    }


    internal class BasicValueTest<T> : ValueTest<T>
    {
        internal BasicValueTest(T actual, IConstraint<T> constraint)
            : base(actual, constraint) { }

        private protected override That<T> ApplyModifier(T actual, ConstraintModifier<T> modifier)
        {
            var expression = new BasicValueExpression<T>(actual, modifier);

            return new That<T>(expression);
        }
    }


    internal class BasicValueExpression<T> 
        : ValueExpression<T, IValueExpression<T>, ValueTest<T>>
    {
        internal BasicValueExpression(T actual, ConstraintModifier<T> modifier) 
            : base(actual, modifier) { }

        protected private override ValueTest<T> ApplyConstraint(T actual, IConstraint<T> constraint)
        {
            return new BasicValueTest<T>(actual, constraint);
        }

        protected private override IValueExpression<T> ApplyModifier(T actual, ConstraintModifier<T> modifier)
        {
            return new BasicValueExpression<T>(actual, modifier);
        }
    }

    internal class BasicIsExpression<T> : IIsExpression<T>
    {
        private readonly IValueExpression<T> expression;

        internal BasicIsExpression(IValueExpression<T> expression)
        {
            Debug.Assert(expression != null);

            this.expression = expression;
        }

        public IIsExpression<T> ApplyModifier(ConstraintModifier<T> modifier)
        {
            return new BasicIsExpression<T>(expression.ApplyModifier(modifier));
        }

        public ValueTest<T> ApplyConstraint(IConstraint<T> constraint)
        {
            return expression.ApplyConstraint(constraint);
        }
    }

}
