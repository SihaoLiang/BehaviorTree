using System;

namespace ECS
{
    public abstract class ECS_Disposer : ECS_Object
    {
        public long Id { get; set; }

        public bool Active { get; set; }

        protected ECS_Disposer()
        {
            ObjectEvents.Instance.Add(this);
        }

        public virtual void Dispose()
        {
            this.Id = 0;
            ECS_ObjectPool.Instance.Recycle(this);
        }
    }
}