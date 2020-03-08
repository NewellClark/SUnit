using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static NUnit.Framework.Assert;

namespace SUnit.Discovery.FinderTests
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
        }

        [Test]
        public void TestReturningInstanceMethod_IsValid()
        {
            MethodInfo info = typeof(Mock).GetMethod(nameof(Mock.InstanceMethod));

            That(Finder.IsValidTestMethod(info), Is.True);
        }

        [Test]
        public void TestReturningStaticMethod_IsNotValid()
        {
            var info = typeof(Mock).GetMethod(nameof(Mock.StaticMethod));

            That(Finder.IsValidTestMethod(info), Is.False);
        }

        [Test]
        public void VoidInstanceMethod_IsNotValid()
        {
            var info = typeof(Mock).GetMethod(nameof(Mock.VoidMethod));

            That(Finder.IsValidTestMethod(info), Is.False);
        }

        [Test]
        public void NonPublicMethod_IsNotValid()
        {
            var info = typeof(Mock).GetMethod(
                nameof(Mock.InternalMethod),
                BindingFlags.Instance | BindingFlags.NonPublic);

            That(Finder.IsValidTestMethod(info), Is.False);
        }

        [Test]
        public void MethodWithArguments_IsNotValid()
        {
            var info = typeof(Mock).GetMethod(nameof(Mock.HasArguments));

            That(Finder.IsValidTestMethod(info), Is.False);
        }

        [Test]
        public void GenericMethod_IsNotValid()
        {
            var info = typeof(Mock).GetMethod(nameof(Mock.GenericMethod));

            That(Finder.IsValidTestMethod(info), Is.False);
        }

        [Test]
        public void ConstructedGenericMethod_IsValid()
        {
            var info = typeof(Mock).GetMethod(nameof(Mock.GenericMethod))
                .MakeGenericMethod(typeof(int));

            That(Finder.IsValidTestMethod(info), Is.True);
        }

        [Test]
        public void ReturnsTestSubclass_IsValid()
        {
            var info = typeof(Mock).GetMethod(nameof(Mock.ReturnsTestSubclass));

            That(Finder.IsValidTestMethod(info), Is.True);
        }
    }
}
