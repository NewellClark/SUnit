using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("SUnitTests")]

namespace SUnit.Discovery
{
    /// <summary>
    /// Contains methods for finding tests.
    /// </summary>
    public static class Finder
    {
        /// <summary>
        /// Indicates whether the specified <see cref="MethodInfo"/> is a valid test method.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo"/> to inspect.</param>
        /// <returns>True if the specified <see cref="MethodInfo"/> is valid.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static bool IsValidTestMethod(MethodInfo method)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));

            bool hasCorrectReturnType = typeof(Test).IsAssignableFrom(method.ReturnType);

            if (!hasCorrectReturnType)
                return false;
            if (!method.IsPublic)
                return false;
            if (method.IsStatic)
                return false;
            if (method.GetParameters().Length > 0)
                return false;
            if (method.IsGenericMethodDefinition)
                return false;

            return true;
        }

        /// <summary>
        /// Finds all the methods on the specified type that are valid test methods.
        /// </summary>
        /// <param name="type">The type to search.</param>
        /// <returns>All methods defined on the type that are valid test methods.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        public static IEnumerable<MethodInfo> FindAllValidTestMethods(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return type.GetRuntimeMethods()
                .Where(IsValidTestMethod);
        }

        internal static IEnumerable<FixtureFactory> FindAllFactories(Type type)
        {
            var namedCtors = FindAllNamedConstructors(type)
                .Select(method => FixtureFactory.FromNamedConstructor(method));
            var ctor = GetDefaultConstructor(type);

            return ctor != null ?
                namedCtors.Prepend(FixtureFactory.FromDefaultConstructor(ctor)) :
                namedCtors;
        }

        private static ConstructorInfo GetDefaultConstructor(Type type)
        {
            return type.GetConstructors()
                .Where(ctor => ctor.GetParameters().Length == 0)
                .Where(ctor => !ctor.IsStatic)
                .SingleOrDefault();
        }

        private static IEnumerable<MethodInfo> FindAllNamedConstructors(Type type)
        {
            Debug.Assert(type != null);

            bool predicate(MethodInfo method)
            {
                return method.IsStatic &&
                    method.IsPublic &&
                    type.IsAssignableFrom(method.ReturnType) &&
                    method.GetParameters().Length == 0;
            }

            return type.GetRuntimeMethods()
                .Where(predicate);
        }
        
        public static IEnumerable<TestCase> FindAllTestCases(IEnumerable<Type> types)
        {
            var validTypes = types
                .Where(type => type.IsPublic)
                .Where(type => !type.ContainsGenericParameters);

            (Type type, IEnumerable<MethodInfo> methods, IEnumerable<FixtureFactory> factories) findEverything(Type type)
            {
                return (type, FindAllValidTestMethods(type), FindAllFactories(type));
            }

            var cases = validTypes.Select(type => findEverything(type))
                .SelectMany(t => t.methods.SelectMany(m => t.factories.Select(f => new TestCase(m, f, t.type))));

            return cases;
        }
    }
}
