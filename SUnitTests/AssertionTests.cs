using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using assert = NUnit.Framework.Assert;

namespace SUnit
{
    [TestFixture]
    public class AssertionTests
    {
        [Test]
        public void TwoPlusTwo_EqualTo_Four()
        {
            assert.That(Assert.That(2 + 2).Is.EqualTo(4).Passed, Is.True);
        }

        [Test]
        public void TwoPlusTwo_Not_EqualTo_Four_Fails()
        {
            assert.That(Assert.That(2 + 2).Is.Not.EqualTo(4).Passed, Is.False);
        }

        [Test]
        public void AndFails_IfFirstFails()
        {
            assert.That(Assert.That(5).Is.EqualTo(4).And.EqualTo(5).Passed, Is.False);
        }

        [Test]
        public void AndPasses_IfBothPass()
        {
            assert.That(Assert.That(5).Is.EqualTo(5).And.Not.EqualTo(4).Passed, Is.True);
        }

        [Test]
        public void OrPasses_IfEitherPasses()
        {
            assert.That(Assert.That(5).Is.EqualTo(4).Or.Not.Null.Passed, Is.True);
        }

        [Test]
        public void OrFails_IfBothFail()
        {
            assert.That(Assert.That(5).Is.EqualTo(9).Or.EqualTo(4).Passed, Is.False);
        }
        [Test]
        public void EqualDoubles_Equal_Passes()
        {
            assert.That(Assert.That(0.0).Is.EqualTo(0.0).Passed, Is.True);
        }

        [Test]
        public void UnequalDoubles_Equal_Fails()
        {
            assert.That(Assert.That(0.0).Is.EqualTo(0.1).Passed, Is.False);
        }

        [Test]
        public void EqualDoubles_NotEqual_Fails()
        {
            assert.That(Assert.That(0.0).Is.Not.EqualTo(0.0).Passed, Is.False);
        }

        [Test]
        public void NullReference_IsNull()
        {
            object @null = null;
            assert.That(Assert.That(@null).Is.Null.Passed, Is.True);
        }

        [Test]
        public void NullNullableValue_IsNull()
        {
            double? @null = null;
            assert.That(Assert.That(@null).Is.Null.Passed, Is.True);
        }

        [Test]
        public void NonNullableValue_IsNotNull()
        {
            assert.That(Assert.That(17).Is.Null.Passed, Is.False);
        }

        [Test]
        public void NonNullReference_IsNotNull()
        {
            assert.That(Assert.That(new object()).Is.Null.Passed, Is.False);
        }

        //[Test]
        //public void True_IsTrue()
        //{
        //    assert.That(Assert.That(true).Is.True.Passed, Is.True);
        //}

        //[Test]
        //public void False_IsNotTrue()
        //{
        //    assert.That(Assert.That(false).Is.True.Passed, Is.False);
        //}

        //[Test]
        //public void True_IsNotFalse()
        //{
        //    assert.That(Assert.That(true).Is.False.Passed, Is.False);
        //}

        //[Test]
        //public void False_IsFalse()
        //{
        //    assert.That(Assert.That(false).Is.False.Passed, Is.True);
        //}

        [Test]
        public void LessThan_LesserValue_Pass()
        {
            assert.That(Assert.That(5).Is.LessThan(6).Passed, Is.True);
        }

        [Test]
        public void LessThan_EqualValues_Fails()
        {
            assert.That(Assert.That(5).Is.LessThan(5).Passed, Is.False);
        }

        [Test]
        public void LessThanOrEqualTo_EqualValues_Passes()
        {
            assert.That(Assert.That(5).Is.LessThanOrEqualTo(5).Passed, Is.True);
        }

        [Test]
        public void LessThanOrEqualTo_GreaterValue_Fails()
        {
            assert.That(Assert.That(5).Is.LessThanOrEqualTo(4).Passed, Is.False);
        }

        [Test]
        public void GreaterThan_GreaterValue_Pass()
        {
            assert.That(Assert.That(-1.4).Is.GreaterThan(-1.5).Passed, Is.True);
        }

        [Test]
        public void GreaterThan_EqualValue_Fails()
        {
            assert.That(Assert.That(16.3).Is.GreaterThan(19.001).Passed, Is.False);
        }

        [Test]
        public void GreaterThanOrEqualTo_EqualValue_Pass()
        {
            assert.That(Assert.That(5).Is.GreaterThanOrEqualTo(5).Passed, Is.True);
        }

        [Test]
        public void GreaterThanOrEqualTo_LesserValue_Fails()
        {
            assert.That(Assert.That(-123).Is.GreaterThanOrEqualTo(11).Passed, Is.False);
        }
    }
}
