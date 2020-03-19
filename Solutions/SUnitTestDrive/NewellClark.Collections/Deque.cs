using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NewellClark.Collections
{
    public class Deque<T> : IEnumerable<T>
    {
        private T[] items;
        private int start = 0;

        public Deque()
        {
            items = new T[4];
        }

        public void PushFront(T item)
        {
            ResizeIfFull();

            start = DecrementIndex(start);
            items[FrontHook] = item;
            Count++;
        }

        public void PushBack(T item)
        {
            ResizeIfFull();

            items[BackHook] = item;
            Count++;
        }

        public T PopFront()
        {
            if (Count == 0)
                throw new InvalidOperationException("Can't front-pop an empty Deque.");

            T result = items[FrontHook];
            start = IncrementIndex(start);
            Count--;

            return result;
        }

        public T PopBack()
        {
            if (Count == 0)
                throw new InvalidOperationException("Can't back-pop an empty Deque.");

            Count--;
            T result = items[BackHook];

            return result;
        }

        public int Count { get; private set; }

        public int Capacity => items.Length;

        public IEnumerator<T> GetEnumerator()
        {
            for (int index = start; index < start + Count; index++)
                yield return items[index % Capacity];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// The index of the first item.
        /// </summary>
        private int FrontHook => start;

        /// <summary>
        /// The first free spot at the back.
        /// </summary>
        private int BackHook => (start + Count) % Capacity;

        private int IncrementIndex(int index)
        {
            return (index + 1) % Capacity;
        }

        private int DecrementIndex(int index)
        {
            if (index == 0)
                return Capacity - 1;

            return (index - 1) % Capacity;
        }

        private void ResizeIfFull()
        {
            if (Count == Capacity - 1)
            {
                T[] newItems = new T[2 * Capacity];

                int index = 0;
                foreach (T item in this)
                {
                    newItems[index] = item;
                    index++;
                }

                items = newItems;
                start = 0;
            }
        }


    }
}
