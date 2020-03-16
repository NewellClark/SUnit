using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUnit;

namespace NewellClark.Collections.Tests
{
    //  Any public class that contains unit tests is a "Test Fixture".
    public class MultidictionaryTests
    {
        public class NewMultidictionary
        {
            private readonly Multidictionary<string, string> dictionary = new Multidictionary<string, string>();
            private readonly string[] keys = new[] 
            { 
                "hello", "world", "squishy",  
                "squidward", "is", "number", "one",
                "", null
            };

            public Test HasCountZero() => Assert.That(dictionary.Count).Is.Zero;

            public Test HasNoItems() => Assert.That(dictionary).Is.Empty;
        }
    }
}
