using System;
using System.Collections.Generic;

namespace ECS
{
    public class ECS_ObjectPool
    {
	    private static ECS_ObjectPool instance;

	    public static ECS_ObjectPool Instance
	    {
		    get
		    {
			    return instance ?? new ECS_ObjectPool();
		    }
	    }

        private readonly Dictionary<Type, ECS_Queue<ECS_Disposer>> dictionary = new Dictionary<Type, ECS_Queue<ECS_Disposer>>();

        private ECS_ObjectPool()
        {
        }

	    public static void Close()
	    {
		    instance = null;
	    }

        public ECS_Disposer Fetch(Type type)
        {
	        ECS_Queue<ECS_Disposer> queue;
            if (!this.dictionary.TryGetValue(type, out queue))
            {
                queue = new ECS_Queue<ECS_Disposer>();
                this.dictionary.Add(type, queue);
            }
	        ECS_Disposer obj;
			if (queue.Count > 0)
            {
				obj = queue.Dequeue();
	            obj.Id = IdGenerater.GenerateId();
	            return obj;
            }
	        obj = (ECS_Disposer)Activator.CreateInstance(type);
            return obj;
        }

        public T Fetch<T>() where T: ECS_Disposer
		{
            return (T) this.Fetch(typeof(T));
        }
        
        public void Recycle(ECS_Disposer obj)
        {
            Type type = obj.GetType();
	        ECS_Queue<ECS_Disposer> queue;
            if (!this.dictionary.TryGetValue(type, out queue))
            {
                queue = new ECS_Queue<ECS_Disposer>();
				this.dictionary.Add(type, queue);
            }
            queue.Enqueue(obj);
        }
    }
}