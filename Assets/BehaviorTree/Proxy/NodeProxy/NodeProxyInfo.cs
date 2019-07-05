using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 代理器类型
    /// </summary>
    public class NodeProxyInfo
    {
        public string ClassType;
        public BehaviorNodeType behaviorNodeType;
        public bool IsLua;
    }
}