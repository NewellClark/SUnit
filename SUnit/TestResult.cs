using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit
{
    public enum TestResult
    {
        /// <summary>
        /// The test did not run successfully.
        /// </summary>
        Error,
        /// <summary>
        /// The test ran successfully, and the result is a FAIL. 
        /// </summary>
        Fail,
        /// <summary>
        /// The test ran successfully, and the result is a PASS.
        /// </summary>
        Pass
    }
}
