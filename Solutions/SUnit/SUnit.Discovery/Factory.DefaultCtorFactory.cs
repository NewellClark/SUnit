﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SUnit.Discovery
{
    partial class Factory
    {
        private sealed class DefaultCtorFactory : Factory
        {
            private readonly ConstructorInfo ctor;

            public DefaultCtorFactory(Fixture fixture)
                : base(fixture)
            {
                Debug.Assert(fixture != null);

                this.ctor = fixture.Type.GetConstructor(Type.EmptyTypes);
            }

            //  Used by reflection.
#pragma warning disable IDE0060 // Remove unused parameter
            private static DefaultCtorFactory Load(Fixture fixture, string subclassData)
#pragma warning restore IDE0060 // Remove unused parameter
            {
                return new DefaultCtorFactory(fixture);
            }

            protected private override string SaveSubclassData()
            {
                var ourTypePair = new TraitPair(nameof(Type), GetType().FullName);
                var fixturePair = new TraitPair(nameof(Fixture), Fixture.Save());

                return TraitPair.SaveAll(ourTypePair, fixturePair);
            }

            public override object Build()
            {
                return ctor.Invoke(Array.Empty<object>());
            }

            public override string Name => "ctor";
            public override bool IsDefaultConstructor => true;
            public override bool IsNamedConstructor => false;
        }
    }
}
