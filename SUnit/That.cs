using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    public class That<TValue>
    {
        internal That(TValue actual)
        {
            this.Actual = actual;
        }
        
        protected private TValue Actual { get; }

        /// <summary>
        /// The class that makes "Is" work.
        /// </summary>
        public Is<TValue> Is => new Is<TValue>(Actual);
    }

    public class ThatBool : That<bool>
    {
        internal ThatBool(bool actual) : base(actual) { }

        public new IsBool Is => new IsBool(Actual);
    }
}
