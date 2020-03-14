using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;

namespace SUnit.Discovery
{
    [TestFixture]
    public class FinderTests
    {
        [TestFixture]
        public class IsValidTestMethod
        {
            private class TestSubclass : Test
            {
                public override bool Passed => false;
            }

            private class Mock
            {
                public Test InstanceMethod() => throw new NotSupportedException();
                public static Test StaticMethod() => throw new NotSupportedException();
                public void VoidMethod() { }
                internal Test InternalMethod() => throw new NotSupportedException();
                public Test HasArguments(string argument) => throw new NotSupportedException(argument);
                public Test GenericMethod<T>() => throw new NotSupportedException();
                public TestSubclass ReturnsTestSubclass() => throw new NotSupportedException();
                public IEnumerable<Test> EnumerableOfTest() => throw new NotSupportedException();

                public IEnumerable<TTest> EnumerableOfConstrainedTypeParameter<TTest>() where TTest 
                    : Test => throw new NotSupportedException();
                public List<Test> ListOfTest() => throw new NotSupportedException();
                public Test[] ArrayOfTest() => throw new NotSupportedException();
                public List<TestSubclass> ListOfTestSubclass() => throw new NotSupportedException();
                public Task<Test> TaskOfTest() => throw new NotSupportedException();
            }

            [Test]
            public void TestReturningInstanceMethod_IsValid()
            {
                MethodInfo info = typeof(Mock).GetMethod(nameof(Mock.InstanceMethod));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void TestReturningStaticMethod_IsNotValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.StaticMethod));

                That(Rules.IsValidTestMethod(info), Is.False);
            }

            [Test]
            public void VoidInstanceMethod_IsNotValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.VoidMethod));

                That(Rules.IsValidTestMethod(info), Is.False);
            }

            [Test]
            public void NonPublicMethod_IsNotValid()
            {
                var info = typeof(Mock).GetMethod(
                    nameof(Mock.InternalMethod),
                    BindingFlags.Instance | BindingFlags.NonPublic);

                That(Rules.IsValidTestMethod(info), Is.False);
            }

            [Test]
            public void MethodWithArguments_IsNotValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.HasArguments));

                That(Rules.IsValidTestMethod(info), Is.False);
            }

            [Test]
            public void GenericMethod_IsNotValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.GenericMethod));

                That(Rules.IsValidTestMethod(info), Is.False);
            }

            [Test]
            public void ConstructedGenericMethod_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.GenericMethod))
                    .MakeGenericMethod(typeof(int));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void ReturnsTestSubclass_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.ReturnsTestSubclass));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void EnumerableOfTest_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.EnumerableOfTest));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void ListOfTest_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.ListOfTest));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void ArrayOfTest_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.ArrayOfTest));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void ListOfTestSubclass_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.ListOfTestSubclass));

                That(Rules.IsValidTestMethod(info), Is.True);
            }

            [Test]
            public void TaskOfTest_IsValid()
            {
                var info = typeof(Mock).GetMethod(nameof(Mock.TaskOfTest));

                That(Rules.IsValidTestMethod(info), Is.True);
            }
        }
    }
}
