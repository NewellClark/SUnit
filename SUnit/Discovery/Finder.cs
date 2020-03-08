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
        /// <returns></returns>
        public static IEnumerable<MethodInfo> FindAllNamedConstructors(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return type.ContainsGenericParameters ?
                FindGenericNamedConstructors(type) :
                FindNonGenericConstructors(type);
        }

        private static IEnumerable<MethodInfo> FindNonGenericConstructors(Type type)
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

        private static IEnumerable<MethodInfo> FindGenericNamedConstructors(Type type)
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
                .Select(t => t.constructed.GetMethod(t.method.Name));
        }
    }
}
