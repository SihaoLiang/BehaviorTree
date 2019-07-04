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
#if XLUA
            if (NewFunc == null)
                NewFunc = XLuaEngine.Get<Func<string, AgentLuaProxy, ILuaAgentProxy>>("XLuaBehaviorManager.NewLuaNodeProxy");

            if (NewFunc != null)
                LuaArentProxy = NewFunc(classType, this);         
#endif
            this.Events = OnGetEvents();
        }

        public override void OnStart()
        {
            LuaArentProxy?.OnStart();
        }

        public override void OnAwake()
        {
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
            LuaArentProxy?.OnDestroy();
        }

        public override void OnUpdate(float deltaTime)
        {
            LuaArentProxy?.OnUpdate(deltaTime);
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            LuaArentProxy?.OnFixedUpdate(deltaTime);
        }

        public override string[] OnGetEvents()
        {
           return LuaArentProxy?.OnGetEvents();
        }

        public override void OnNotify(string evt, params object[] args)
        {
            LuaArentProxy?.OnNotify(evt, args);
        }
    }
}