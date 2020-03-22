using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SUnit.Discovery;
using SUnit.Discovery.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SUnit.TestAdapter
{
    internal static class Extensions
    {
        private static readonly TestProperty SerializedUnitTestProperty =
            TestProperty.Register(
                nameof(SerializedUnitTestProperty),
                nameof(SerializedUnitTestProperty),
                typeof(string), typeof(TestCase));

        public static TestCase ToTestCase(this UnitTest @this)
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            var @case = new TestCase();
            @case.Source = @this.Factory.Fixture.Assembly.Location;
            @case.DisplayName = @this.Name;
            @case.FullyQualifiedName = $"{@this.Factory.Fixture.Type.FullName}+{@this.Factory.Name}.{@this.Name}";
            @case.ExecutorUri = new Uri(SUnitTestExecutor.ExecutorUri);
            @case.SetPropertyValue(SerializedUnitTestProperty, @this.Save());

            return @case;
        }

        public static UnitTest ToUnitTest(this TestCase @this)
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));

            string serialized = (string)@this.GetPropertyValue(SerializedUnitTestProperty);

            return UnitTest.Load(serialized);
        }

        public static TestOutcome ToTestOutcome(this ResultKind @this)
        {
            return @this switch
            {
                ResultKind.Pass => TestOutcome.Passed,
                ResultKind.Fail => TestOutcome.Failed,
                ResultKind.Error => TestOutcome.Failed,
                _ => TestOutcome.None
            };
        }
    }
}
