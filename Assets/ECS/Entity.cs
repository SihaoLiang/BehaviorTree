using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace ECS
{
    public class Entity : ECS_Disposer//, ISupportInitialize
    {
        public Entity Parent { get; set; }

        private Dictionary<Type, ECS_Component> componentDict = new Dictionary<Type, ECS_Component>();

        protected Entity()
        {
            this.Id = IdGenerater.GenerateId();
        }

        protected Entity(long id)
        {
            this.Id = id;
        }

        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();

            foreach (ECS_Component component in this.GetComponents())
            {
                try
                {
                    component.Dispose();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.ToString());
                }
            }
        }

        public K AddComponent<K>() where K : ECS_Component, new()
        {
            K component = ComponentFactory.Create<K>(this);

            if (this.componentDict.ContainsKey(component.GetType()))
            {
                throw new Exception(string.Format("AddComponent, component already exist, id: {0}, component: {1}", this.Id, typeof(K).Name));
            }

            this.componentDict.Add(component.GetType(), component);
            return component;
        }

        public K AddComponent<K, P1>(P1 p1) where K : ECS_Component, new()
        {
            K component = ComponentFactory.Create<K, P1>(this, p1);

            if (this.componentDict.ContainsKey(component.GetType()))
            {
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
                throw new Exception(string.Format("AddComponent, component already exist, id: {0}, component: {1}", this.Id, typeof(K).Name));
            }

            this.componentDict.Add(component.GetType(), component);
            return component;
        }

        public K AddComponent<K, P1, P2>(P1 p1, P2 p2) where K : ECS_Component, new()
        {
            K component = ComponentFactory.Create<K, P1, P2>(this, p1, p2);

            if (this.componentDict.ContainsKey(component.GetType()))
            {
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
                throw new Exception(string.Format("AddComponent, component already exist, id: {0}, component: {1}", this.Id, typeof(K).Name));
            }

            this.componentDict.Add(component.GetType(), component);
            return component;
        }

        public K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : ECS_Component, new()
        {
            K component = ComponentFactory.Create<K, P1, P2, P3>(this, p1, p2, p3);

            if (this.componentDict.ContainsKey(component.GetType()))
            {
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
                throw new Exception(string.Format("AddComponent, component already exist, id: {0}, component: {1}", this.Id, typeof(K).Name));
            }

            this.componentDict.Add(component.GetType(), component);
            return component;
        }

        public void RemoveComponent<K>() where K : ECS_Component
        {
            ECS_Component component;
            if (!this.componentDict.TryGetValue(typeof(K), out component))
            {
                return;
            }

            this.componentDict.Remove(typeof(K));
            component.Dispose();
        }

        public void RemoveComponent(Type type)
        {
            ECS_Component component;
            if (!this.componentDict.TryGetValue(type, out component))
            {
                return;
            }

            this.componentDict.Remove(type);
            component.Dispose();
        }

        public void RemoveAllComponents()
        {
            ECS_Component[] components = this.componentDict.Values.ToArray();
            for (int i = 0; i < components.Length; i++)
            {
                ECS_Component component = components[i];
                if (component != null)
                    RemoveComponent(component.GetType());
            }
        }

        public K GetComponent<K>() where K : ECS_Component
        {
            ECS_Component component;
            if (!this.componentDict.TryGetValue(typeof(K), out component))
            {
                return default(K);
            }
            return (K)component;
        }

        public ECS_Component[] GetComponents()
        {
            return this.componentDict.Values.ToArray();
        }

        //public virtual void BeginInit()
        //{
        //    this.componentDict = new Dictionary<Type, ECS_Component>();
        //}

        //public virtual void EndInit()
        //{
        //    ObjectEvents.Instance.Add(this);
        //}

        public void Notify(int evt, params object[] param)
        {
            foreach(ECS_Component component in componentDict.Values)
            {
                if (component != null)
                    component.OnNotiyUpdate(evt, param);
            }
        }
    }
}