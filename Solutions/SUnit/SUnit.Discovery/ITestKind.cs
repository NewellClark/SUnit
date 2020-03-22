using SUnit.Discovery.Results;
using System;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace SUnit.Discovery
{
    internal interface ITestKind
    {
        public bool IsReturnTypeValid(Type returnType);

        public IObservable<TestResult> Run(UnitTest unitTest);
    }
}
