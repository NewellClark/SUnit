using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SUnit;

namespace NewellClark.Collections.Tests
{
    public class DequeTests
    {
        private static void FrontPushAll(Deque<string> deque, params string[] items)
        {
            foreach (string item in items)
                deque.PushFront(item);
        }

        private static void BackPushAll(Deque<string> deque, params string[] items)
        {
            foreach (string item in items)
                deque.PushBack(item);
        }

        private static IEnumerable<string> FrontPop(Deque<string> deque, int count)
        {
            for (int c = 0; c < count; c++)
                yield return deque.PopFront();
        }

        private static IEnumerable<string> BackPop(Deque<string> deque, int count)
        {
            for (int c = 0; c < count; c++)
                yield return deque.PopBack();
        }


        public class EmptyDeque
        {
            private readonly Deque<string> empty = new Deque<string>();

            public Test HasZeroCount() => Assert.That(empty.Count).Is.Zero;

            public Test IsEmpty() => Assert.That(empty).Is.Empty;

            public Test ThrowsWhenFrontPopped()
            {
                return Assert.Throws<InvalidOperationException>(() => empty.PopFront());
            }

            public Test ThrowsWhenBackPopped()
            {
                return Assert.Throws<InvalidOperationException>(() => empty.PopBack());
            }

            public Test ContainsBackPushedItemsAtEnd()
            {
                BackPushAll(empty, "hello", "world", "everyone");

                return Assert.That(empty.TakeLast(3)).Is.SequenceEqualTo("hello", "world", "everyone");
            }

            public Test ContainsFrontPushedItemsAtBeginning()
            {
                FrontPushAll(empty, "team", "S", "rocks");

                return Assert.That(empty.Take(3)).Is.SequenceEqualTo("rocks", "S", "team");
            }
        }


        public class SmallDeque
        {
            private readonly Deque<string> small;

            public SmallDeque()
            {
                small = new Deque<string>();

                BackPushAll(small, "try", "catch", "for", "while", "foreach");
            }

            public IEnumerable<Test> PopFrontReturnsFrontItems()
            {
                yield return Assert.That(small.PopFront()).Is.EqualTo("try");
                yield return Assert.That(small.PopFront()).Is.EqualTo("catch");
                yield return Assert.That(small.PopFront()).Is.EqualTo("for");
            }

            public Test PopFrontRemovesPoppedItems()
            {
                small.PopFront();
                small.PopFront();

                return Assert.That(small).Is.SequenceEqualTo("for", "while", "foreach");
            }

            public IEnumerable<Test> PopBackReturnsBackItems()
            {
                yield return Assert.That(small.PopBack()).Is.EqualTo("foreach");
                yield return Assert.That(small.PopBack()).Is.EqualTo("while");
                yield return Assert.That(small.PopBack()).Is.EqualTo("for");
            }

            public Test PopBackRemovesPoppedItems()
            {
                small.PopBack();
                small.PopBack();

                return Assert.That(small).Is.SequenceEqualTo("try", "catch", "for");
            }
        }
        

        public class AnyDeque
        {
            private readonly Deque<string> deque;

            private AnyDeque(Deque<string> deque)
            {
                this.deque = deque;
            }


            public static AnyDeque FrontPushedOnly()
            {
                var deque = new Deque<string>();

                FrontPushAll(deque, "hello", "quick", "brown", "fox", "jumping", "over", "the", "lazy", "dog");

                return new AnyDeque(deque);
            }

            public static AnyDeque BackPushedOnly()
            {
                var deque = new Deque<string>();

                BackPushAll(deque, "hello world string deque params items likes the argument list to be perfect".Split(" "));

                return new AnyDeque(deque);
            }

            public static AnyDeque DoublePushed()
            {
                var deque = new Deque<string>();

                BackPushAll(deque, "I'll do whatever it takes to turn this around".Split(" "));
                FrontPushAll(deque, "I'm getting very lonely in life I should get out more");

                return new AnyDeque(deque);
            }

            public Test FrontPushesItemsAtFront()
            {
                deque.PushFront("extern");
                deque.PushFront("void");
                deque.PushFront("stackalloc");

                return Assert.That(deque.Take(3)).Is.SequenceEqualTo("stackalloc", "void", "extern");
            }

            public Test BackPushesItemsAtBack()
            {
                deque.PushBack("extern");
                deque.PushBack("void");
                deque.PushBack("stackalloc");

                return Assert.That(deque.TakeLast(3)).Is.SequenceEqualTo("extern", "void", "stackalloc");
            }

            public Test FrontPushedItemsGetAdded()
            {
                var initial = deque.ToArray();

                deque.PushFront("blahblah");
                deque.PushFront("C++");

                var expected = initial.Prepend("blahblah").Prepend("C++");

                return Assert.That(deque).Is.SequenceEqualTo(expected);
            }

            public Test FrontDrainingDequeGetsEmptied()
            {
                while (deque.Count > 0)
                    deque.PopFront();

                return Assert.That(deque).Is.Empty &&
                    Assert.That(deque.Count).Is.Zero;
            }

            public Test BackDrainingDequeGetsEmptied()
            {
                while (deque.Count > 0)
                    deque.PopBack();

                return Assert.That(deque).Is.Empty &&
                    Assert.That(deque.Count).Is.Zero;
            }
        }
    }
}
