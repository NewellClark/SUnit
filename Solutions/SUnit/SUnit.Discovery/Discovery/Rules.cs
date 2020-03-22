using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    /// <summary>
    /// Contains methods that define the rules for what is a valid test, what is a valid fixture,
    /// and what is a valid factory.
    /// </summary>
    internal static class Rules
    {
        private static readonly List<ITestKind> testKinds = new List<ITestKind>
        {
            new MultiTestKind(),
            new AsyncTestKind(),
            new SingletonTestKind()
        };

        /// <summary>
        /// Indicates whether the specified type is valid as the return type for a test method.
        /// </summary>
        /// <param name="returnType">The <see cref="Type"/> to test.</param>
        /// <returns>True if the specified type is a legal return type for a unit test method.</returns>
        public static bool IsReturnTypeValidForTestMethod(Type returnType)
        {
            foreach (var testKind in testKinds)
                if (testKind.IsReturnTypeValid(returnType))
                    return true;

            return false;
        }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public static ITestKind? GetTestKind(Type returnType)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            foreach (var testKind in testKinds)
                if (testKind.IsReturnTypeValid(returnType))
                    return testKind;

            return null;
        }

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

            if (!IsReturnTypeValidForTestMethod(method.ReturnType))
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

        /// <summary>
        /// Gets the eligible default constructor for the specified type, if one exists.
        /// </summary>
        /// <param name="type">The type to search.</param>
        /// <returns>The eligible default constructor, if one exists. <see langword="null"/> if none is found.</returns>
        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            if (type.IsAbstract)
                return null;
            if (type.IsGenericTypeDefinition)
                return null;

            return type.GetConstructors()
                .Where(ctor => ctor.GetParameters().Length == 0)
                .Where(ctor => !ctor.IsStatic)
                .SingleOrDefault();
        }

        /// <summary>
        /// Finds all the valid named constructors on the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>All the valid named constructor methods on the specified fixture type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
        public static IEnumerable<MethodInfo> FindNamedConstructors(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return type.ContainsGenericParameters ?
                FindNamedConstructorsOnGenericType(type) :
                FindNamedConstructorsOnNonGenericType(type);
        }

        /// <summary>
        /// Finds all the valid named constructors on a non generic, or constructed generic, fixture type.
        /// </summary>
        /// <param name="type">The type of the fixture. Must be either non-generic or a fully-constructed
        /// generic type. In other words, <see cref="Type.ContainsGenericParameters"/> 
        /// must be <see langword="false"/>.</param>
        /// <returns>All valid named constructor methods found on the fixture type.</returns>
        private static IEnumerable<MethodInfo> FindNamedConstructorsOnNonGenericType(Type type)
        {
            Debug.Assert(!type.ContainsGenericParameters);

            bool predicate(MethodInfo method)
            {
                return
                    method.IsStatic &&
                    method.IsPublic &&
                    type.IsAssignableFrom(method.ReturnType) &&
                    !method.ContainsGenericParameters &&
                    method.GetParameters().Length == 0;
            }

            return type.GetRuntimeMethods()
                .Where(predicate);
        }

        /// <summary>
        /// Finds all the valid named constructors on an unconstructed generic fixture type.
        /// On an unconstructed generic fixture type, a valid named constructor returns a constructed version
        /// of the fixture, or a subclass of a constructed version of the fixture.
        /// </summary>
        /// <param name="type">The type of the fixture. Must be a not-fully-constructed generic type. In other
        /// words, <see cref="Type.ContainsGenericParameters"/> must be <see langword="true"/>.</param>
        /// <returns>All valid named constructor methods found on the fixture type.</returns>
        private static IEnumerable<MethodInfo> FindNamedConstructorsOnGenericType(Type type)
        {
            Debug.Assert(type.ContainsGenericParameters);

            static Type getConstructedGenericBase(Type genericTypeDefinition, Type potentialSubclass)
            {
                Debug.Assert(genericTypeDefinition.IsGenericTypeDefinition);
                Debug.Assert(!potentialSubclass.ContainsGenericParameters);

                static IEnumerable<Type> baseTypes(Type type)
                {
                    if (type is null)
                        yield break;
                    yield return type;
                    foreach (var baseType in baseTypes(type.BaseType))
                        yield return baseType;
                }

                return baseTypes(potentialSubclass)
                    .Where(type => type.IsGenericType)
                    .Where(type => type.GetGenericTypeDefinition() == genericTypeDefinition)
                    .SingleOrDefault();
            }

            (MethodInfo method, Type constructed) selectConstructed(MethodInfo method)
            {
                return (method, getConstructedGenericBase(type.GetGenericTypeDefinition(), method.ReturnType));
            }

            return type.GetRuntimeMethods()
                .Where(method => method.IsStatic)
                .Where(method => method.IsPublic)
                .Where(method => method.GetParameters().Length == 0)
                .Where(method => !method.ReturnType.ContainsGenericParameters)
                .Select(selectConstructed)
                .Select(t => t.constructed.GetMethod(t.method.Name))
                .Where(method => !method.ContainsGenericParameters);
        }
    }
}
