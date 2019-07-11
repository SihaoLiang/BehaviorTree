using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree {
    public class AgentLuaProxy : AgentProxy {

        public ILuaAgentProxy LuaAgentProxy = null;
        static Func<string, AgentLuaProxy, ILuaAgentProxy> NewFunc = null;

        public override void SetAgent(Agent agent, string classType)
        {
            BTAgent = agent;
#if XLUA
            if (NewFunc == null)
                NewFunc = XLuaEngine.Get<Func<string, AgentLuaProxy, ILuaAgentProxy>>("XLuaBehaviorManager.NewLuaAgentProxy");

            if (NewFunc != null)
                LuaAgentProxy = NewFunc(classType, this);
#endif
            this.Events = OnGetEvents();
        }

        public override void OnStart()
        {
            base.OnStart();
            LuaAgentProxy?.OnStart();
        }

        public override void OnAwake()
        {
            base.OnAwake();
            LuaAgentProxy?.OnAwake();
        }

        public override void OnEnable()
        {
            LuaAgentProxy?.OnEnable();
        }

        public override void OnDisable()
        {
            LuaAgentProxy?.OnDisable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            LuaAgentProxy?.OnDestroy();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            LuaAgentProxy?.OnUpdate();
        }

        public override List<string> OnGetEvents()
        {
            string[] events = LuaAgentProxy?.OnGetEvents();
            if (events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    AddEvent(events[i]);
                }
            }

            return Events;
        }

        public override void OnNotify(string evt, params object[] args)
        {
            LuaAgentProxy?.OnNotify(evt, args);
        }
    }
}