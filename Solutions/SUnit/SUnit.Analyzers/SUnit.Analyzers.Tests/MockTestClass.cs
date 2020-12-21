#if false
using SUnit;

namespace MockNamespace
{
    public class MockTestClass
    {
        public void SomeVoidReturningMethod() { }

        public Test MockTestMethod()
        {
            Assert.That(2 + 2).Is.EqualTo(5);

            return Assert.That(2 + 2).Is.EqualTo(4);
        }
    }
}
#endif
