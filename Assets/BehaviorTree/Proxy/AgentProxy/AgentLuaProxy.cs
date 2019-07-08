using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree {
    public class AgentLuaProxy : AgentProxy {

        public ILuaAgentProxy LuaArentProxy = null;
        static Func<string, AgentLuaProxy, ILuaAgentProxy> NewFunc = null;


        public AgentLuaProxy(string classType)
        {
            if (NewFunc == null)
                NewFunc = XLuaEngine.Get<Func<string, AgentLuaProxy, ILuaAgentProxy>>("XLuaBehaviorManager.NewLuaAgentProxy");

            if (NewFunc != null)
                LuaArentProxy = NewFunc(classType, this);   
            
            this.Events = OnGetEvents();
        }

        public override void OnStart()
        {
            base.OnStart();
            LuaArentProxy?.OnStart();
        }

        public override void OnAwake()
        {
            base.OnAwake();
            LuaArentProxy?.OnAwake();
        }

        public override void OnEnable()
        {
            LuaArentProxy?.OnEnable();
        }

        public override void OnDisable()
        {
            LuaArentProxy?.OnDisable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            LuaArentProxy?.OnDestroy();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            LuaArentProxy?.OnUpdate(deltaTime);
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            base.OnFixedUpdate(deltaTime);
            LuaArentProxy?.OnFixedUpdate(deltaTime);
        }

        public override List<string> OnGetEvents()
        {
            string[] events = LuaArentProxy?.OnGetEvents();
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
            LuaArentProxy?.OnNotify(evt, args);
        }
    }
}