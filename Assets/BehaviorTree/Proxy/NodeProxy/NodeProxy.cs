using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class NodeProxy
    {
        public BaseNode Node = null;
        public NodeProxyInfo NodeProxyInfo;

        public string[] Events = null;

        public abstract void OnAwake();
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnDestroy();
        public abstract void OnReset();
        public abstract void OnUpdate(float deltaTime);
        public abstract void OnFixedUpdate(float deltaTime);
        public abstract string[] OnGetEvents();
        public abstract void OnNotify(string evt, params object[] args);
    }
}