using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.Assertions
{
    /// <inheritdoc/>
    public interface IIsExpressionBool : IIsExpression<bool?, IIsExpressionBool, IsTestBool>
    {

        //  And what else should I call these? They assert that something is TRUE and FALSE, 
        //  they should be called TRUE and FALSE, respectively. 
        //  I believe that THINGS should be called THINGS, not TheThing, not MyThing, not ItsThing,
        //  not ThingObject or ThingValue. Things should be called Things!

#pragma warning disable CA1716 // Identifiers should not match keywords
        /// <summary>
        /// Tests whether the actual value is <see langword="true"/>.
        /// </summary>
        public IsTestBool True => EqualTo(true);

        /// <summary>
        /// Tests whether the actual value is <see langword="false"/>.
        /// </summary>
        public IsTestBool False => EqualTo(false);
#pragma warning restore CA1716 // Identifiers should not match keywords
    }

    internal class IsExpressionBool : ActualValueExpression<bool?, IIsExpressionBool, IsTestBool>, IIsExpressionBool
    {
        internal IsExpressionBool(bool? actual, ConstraintModifier<bool?> constraintModifier)
            : base(actual, constraintModifier) { }
        internal IsExpressionBool(bool? actual) 
            : base(actual, c => c) { }

        protected private override IsTestBool CreateTest(bool? actual, IConstraint<bool?> constraint)
        {
            return new IsTestBool(actual, constraint);
        }

        protected private override IIsExpressionBool CreateExpression(bool? actual, ConstraintModifier<bool?> constraintModifier)
        {
            return new IsExpressionBool(actual, constraintModifier);
        }
    }
}
