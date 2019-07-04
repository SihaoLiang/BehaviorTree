using System.Collections;
using System.Collections.Generic;

namespace ECS
{
    public class ECS_Queue<T> : IEnumerable
    {
        private readonly Queue<T> list = new Queue<T>();

        public void Enqueue(T t)
        {
            this.list.Enqueue(t);
        }

        public T Peek()
        {
            return this.list.Peek();
        }

        public T Dequeue()
        {
            return list.Dequeue();
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public void Clear()
        {
            this.list.Clear();
        }
    }
}