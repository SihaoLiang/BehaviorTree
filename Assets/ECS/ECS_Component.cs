namespace ECS
{
    public abstract class ECS_Component : ECS_Disposer
    {
        public Entity Entity { get; set; }

        public T GetEntity<T>() where T : Entity
        {
            return this.Entity as T;
        }

        protected ECS_Component()
        {
            this.Id = 1;
        }

        public T GetComponent<T>() where T : ECS_Component
        {
            return this.Entity.GetComponent<T>();
        }

        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();

            if (this.Entity != null)
                this.Entity.RemoveComponent(this.GetType());
        }

        public virtual void OnNotiyUpdate(int evt, params object[] param) { }
    }
}