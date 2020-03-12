using System;
using System.Collections.Generic;
using System.Text;

/*  Everything here must be:
 *  -   Not concerned with actually RUNNING tests.
 *  -   Easily serializable to text.
 *  
 *  A Fixture can:
 *  -   Tell you its Assembly.
 *  -   Tell you its Type.
 *  -   Tell you its TestMethods.
 *  -   Tell you all its Factories.
 *  -   Be round-trip serialized to text.
 *  -   Use value-equality semantics.
 *  
 *  A Factory can:
 *  -   Tell you its' Fixture's type (not its fixture!).
 *  -   Instantiate an instance of the Fixture type (not an instance of the Fixture class!). 
 *  -   Be round-trip serialized to text.
 *  -   Use value-equality semantics.
 *  
 *  A TestMethod can:
 *  -   Tell you its' Fixture's type. 
 *  -   Tell you what kind of test it is (once we add multi-tests and async tests). 
 *  -   Give you the MethodInfo for the test method it encapsulates.
 *  -   Be round-trip serialized to text.
 *  -   Use value-equality semantics.
 * */
namespace SUnit.Discovery
{
    internal class Fixture
    {
    }

    internal abstract class Factory
    {

    }

    internal class TestMethod
    {

    }
}
