using SUnit.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    /// <summary>
    /// We need these methods, but we're not sure where they belong. 
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Displays an actual value that can be null.
        /// </summary>
        /// <param name="value">The value to display. Can be null.</param>
        /// <returns>The string representation for the value to display to the user.</returns>
        public static string DisplayValue(object value)
        {
            return value is null ?
                "[null]" :
                value.ToString();
        }
    }
}
