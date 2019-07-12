using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTreeData;
using UnityEngine;
#if CLIENT
using XLua;
#endif

namespace BehaviorTree
{
    public class NodeLuaProxy : NodeProxy
    {
       // public LuaTable ProxyLuaTable = null;
        public ILuaNodeProxy LuaNodeProxy = null;
        static Func<string, NodeLuaProxy, ILuaNodeProxy> NewFunc = null;

        public override void SetNode(BaseNode baseNode)
        {
            this.Node = baseNode;
            this.NodeProxyInfo = baseNode.NodeInfo;
#if XLUA
            if (NewFunc == null)
                NewFunc = XLuaEngine.Get<Func<string, NodeLuaProxy, ILuaNodeProxy>>("XLuaBehaviorManager.NewLuaNodeProxy");

            if (NewFunc != null)
                LuaNodeProxy = NewFunc(baseNode.NodeInfo.ClassType, this);
#endif
            this.Events = OnGetEvents();
        }

        public override void OnAwake()
        {
            LuaNodeProxy?.OnAwake();
        }

        public override void OnEnter()
        {
            LuaNodeProxy?.OnEnter();
        }

        public override void OnExit()
        {
            LuaNodeProxy?.OnExit();
        }

        public override void OnDestroy()
        {
            LuaNodeProxy?.OnDestroy();
        }

        public override void OnReset()
        {
            LuaNodeProxy?.OnReset();
        }

        public override void OnUpdate(float deltaTime)
        {
            LuaNodeProxy?.OnUpdate(deltaTime);
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            LuaNodeProxy?.OnFixedUpdate(deltaTime);
        }

        public override string[] OnGetEvents()
        {
            return LuaNodeProxy?.OnGetEvents();
        }

        public override void OnEnable()
        {
            LuaNodeProxy?.OnEnable();
        }

        public override void OnDisable()
        {
            LuaNodeProxy?.OnDisable();
        }

        public override void OnNotify(string evt, params object[] args)
        {
            LuaNodeProxy?.OnNotify(evt, args);
        }
    }
}