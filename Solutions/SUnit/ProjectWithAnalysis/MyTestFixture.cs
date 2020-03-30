using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUnit;

namespace ProjectWithAnalysis
{
    //  Any public class that contains unit tests is a Test Fixture.
    public class MyTestFixture
    {
        //  Any public instance method that returns a Test is a Unit Test.
        public Test MyUnitTest()
        {
            //Assert.That(2 + 2).Is.EqualTo(4);

            //  The Assert class contains properties and methods that return Test objects.
            return Assert.That("Hello, World!").Is.Not.Null;
        }

        //  You can return multiple tests from one test method. 
        public IEnumerable<Test> TestAllMyData()
        {
            foreach (int n in Enumerable.Range(1, 7))
                yield return Assert.That(n).Is.Positive.And.Is.Not.Zero;
        }

        //  Async tests are also supported.
        public async Task<Test> TestTheLaggyDatabase()
        {
            await Task.Yield();

            return Assert.That("Hello, World!").Is.EquivalentTo("World, Hello!");
        }
    }
}
