using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECS
{
    public interface IObjectEvent
    {
        Type Type();
        void Set(object value);
    }

    public abstract class ObjectEvent<T> : IObjectEvent
    {
        private T value;

        protected T Get()
        {
            return value;
        }

        public void Set(object v)
        {
            this.value = (T)v;
        }

        public Type Type()
        {
            return typeof(T);
        }
    }

    public sealed class ObjectEvents
    {
        private static ObjectEvents instance;

        public static ObjectEvents Instance
        {
            get
            {
                return instance ?? (instance = new ObjectEvents());
            }
        }

        public Assembly HotfixAssembly;

        private readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        private readonly Dictionary<Type, IObjectEvent> disposerEvents = new Dictionary<Type, IObjectEvent>();

        private ECS_Queue<ECS_Disposer> updates = new ECS_Queue<ECS_Disposer>();
        private ECS_Queue<ECS_Disposer> updates2 = new ECS_Queue<ECS_Disposer>();

        private readonly ECS_Queue<ECS_Disposer> starts = new ECS_Queue<ECS_Disposer>();

        private ECS_Queue<ECS_Disposer> lateUpdates = new ECS_Queue<ECS_Disposer>();
        private ECS_Queue<ECS_Disposer> lateUpdates2 = new ECS_Queue<ECS_Disposer>();

        public static void Close()
        {
            instance = null;
        }

        public void Add(string name, Type[] types)
        {
            if (types == null || types.Length == 0)
                return;

            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];

                //if (GameSetup.instance.useILRuntime)
                //{
                //    if (type.BaseType != null && (type.BaseType == typeof(Attribute) || type.BaseType.Name == "Attribute"))
                //        continue;

                //    if (type.Name == "Void")
                //        continue;

                //    if (type.IsValueType)
                //        continue;

                //    if (type == typeof(System.Object))
                //        continue;
                //}

                object[] attrs = type.GetCustomAttributes(typeof(ObjectEventAttribute), false);

                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }

                ObjectEventAttribute objectEventAttribute = attrs[0] as ObjectEventAttribute;

                if (objectEventAttribute == null)
                    continue;

                object obj = Activator.CreateInstance(type);
                IObjectEvent objectEvent = obj as IObjectEvent;
                if (objectEvent == null)
                {
                    UnityEngine.Debug.LogError(string.Format("组件事件没有继承IObjectEvent: {0}", type.Name));
                    continue;
                }
                this.disposerEvents[objectEvent.Type()] = objectEvent;

            }
        }

        public Assembly Get(string name)
        {
            return this.assemblies[name];
        }

        public Assembly[] GetAll()
        {
            return this.assemblies.Values.ToArray();
        }

        public void Add(ECS_Disposer disposer)
        {
            IObjectEvent objectEvent;
            if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
            {
                return;
            }

            if (objectEvent is IUpdate)
            {
                this.updates.Enqueue(disposer);
            }

            if (objectEvent is IStart)
            {
                this.starts.Enqueue(disposer);
            }

            if (objectEvent is ILateUpdate)
            {
                this.lateUpdates.Enqueue(disposer);
            }
        }

        public void Awake(ECS_Disposer disposer)
        {
            IObjectEvent objectEvent;
            if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
            {
                return;
            }
            IAwake iAwake = objectEvent as IAwake;
            if (iAwake == null)
            {
                return;
            }
            objectEvent.Set(disposer);
            iAwake.Awake();
        }

        public void Awake<P1>(ECS_Disposer disposer, P1 p1)
        {
            IObjectEvent objectEvent;
            if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
            {
                return;
            }
            IAwake<P1> iAwake = objectEvent as IAwake<P1>;
            if (iAwake == null)
            {
                return;
            }
            objectEvent.Set(disposer);
            iAwake.Awake(p1);
        }

        public void Awake<P1, P2>(ECS_Disposer disposer, P1 p1, P2 p2)
        {
            IObjectEvent objectEvent;
            if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
            {
                return;
            }
            IAwake<P1, P2> iAwake = objectEvent as IAwake<P1, P2>;
            if (iAwake == null)
            {
                return;
            }
            objectEvent.Set(disposer);
            iAwake.Awake(p1, p2);
        }

        public void Awake<P1, P2, P3>(ECS_Disposer disposer, P1 p1, P2 p2, P3 p3)
        {
            IObjectEvent objectEvent;
            if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
            {
                return;
            }
            IAwake<P1, P2, P3> iAwake = objectEvent as IAwake<P1, P2, P3>;
            if (iAwake == null)
            {
                return;
            }
            objectEvent.Set(disposer);
            iAwake.Awake(p1, p2, p3);
        }

        private void Start()
        {
            while (this.starts.Count > 0)
            {
                ECS_Disposer disposer = this.starts.Dequeue();

                IObjectEvent objectEvent;
                if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
                {
                    continue;
                }
                IStart iStart = objectEvent as IStart;
                if (iStart == null)
                {
                    continue;
                }
                objectEvent.Set(disposer);
                iStart.Start();
            }
        }

        public void Update(float deltaTime)
        {
            this.Start();

            while (this.updates.Count > 0)
            {
                ECS_Disposer disposer = this.updates.Dequeue();
                if (disposer.Id == 0)
                {
                    continue;
                }

                IObjectEvent objectEvent;
                if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
                {
                    continue;
                }

                this.updates2.Enqueue(disposer);

                IUpdate iUpdate = objectEvent as IUpdate;
                if (iUpdate == null)
                {
                    continue;
                }
                objectEvent.Set(disposer);

#if UNITY_EDITOR
                iUpdate.Update(deltaTime);
#else
                try
                {
                    iUpdate.Update(deltaTime);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                }
#endif
            }

            ObjectHelper.Swap(ref this.updates, ref this.updates2);
        }

        public void LateUpdate(float deltaTime)
        {
            while (this.lateUpdates.Count > 0)
            {
                ECS_Disposer disposer = this.lateUpdates.Dequeue();
                if (disposer.Id == 0)
                {
                    continue;
                }

                IObjectEvent objectEvent;
                if (!this.disposerEvents.TryGetValue(disposer.GetType(), out objectEvent))
                {
                    continue;
                }

                this.lateUpdates2.Enqueue(disposer);

                ILateUpdate iLateUpdate = objectEvent as ILateUpdate;
                if (iLateUpdate == null)
                {
                    continue;
                }
                objectEvent.Set(disposer);
#if UNITY_EDITOR
                iLateUpdate.LateUpdate(deltaTime);
#else
                try
                {
                    iLateUpdate.LateUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                }
#endif
            }

            ObjectHelper.Swap(ref this.lateUpdates, ref this.lateUpdates2);
        }
    }
}