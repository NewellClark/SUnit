﻿using System;
using System.Collections.Generic;
using System.Text;
using SUnit;

namespace SUnitProjectTemplate
{
    public class TestFixture
    {
        public Test WrittenTest()
        {
            return Assert.That(2 + 2).Is.EqualTo(4);
        }
    }
}
