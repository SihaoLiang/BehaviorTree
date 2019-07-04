using BehaviorTreeData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BaseNode
    {
        /// <summary>
        /// 节点数据
        /// </summary>
        public NodeData NodeDatas;

        /// <summary>
        /// 节点状态
        /// </summary>
        public NodeStatus Status;

        /// <summary>
        /// 父节点
        /// </summary>
        public BaseNode Parent;

        /// <summary>
        /// 执行者
        /// </summary>
        public Agent NodeAgent;

        /// <summary>
        /// 代理
        /// </summary>
        public NodeProxy Proxy;

        /// <summary>
        /// 初始化代理器
        /// </summary>
        public void InitNode(NodeData nodeData,Agent agent)
        {
            NodeAgent = agent;
            NodeDatas = nodeData;
            InitProxy();
        }

        /// <summary>
        /// 初始化代理器
        /// </summary>
        void InitProxy()
        {
            string classType = NodeDatas.ClassType;
            NodeProxyInfo nodeProxy = BehaviorTreeManager.Instance.GetRegistedNodeType(classType);

            if (nodeProxy == null)
            {
                Debug.LogError($"错误！！行为树节点未注册 ClassType:{classType}");
                return;
            }

            if (!nodeProxy.IsLua)
            {
                Type type = BehaviorTreeManager.Instance.GetBehaviorNodeType(nodeProxy.ClassType);
                Proxy = Activator.CreateInstance(type) as NodeProxy;
            }
            else
            {
                Proxy = new NodeLuaProxy(nodeProxy);
            }

            if (Proxy == null)
            {
                Debug.LogError($"错误！！找不到行为树节点代理器 ClassType:{classType}");
                return;
            }

            Proxy.Node = this;
            Proxy.NodeProxyInfo = nodeProxy;
        }

        /// <summary>
        /// 节点运行
        /// </summary>
        public virtual void OnEnable()
        {
            if (Proxy != null && Proxy.Events != null)
            {
                for (int i = 0; i < Proxy.Events.Length; i++)
                {
                    string evt = Proxy.Events[i];
                    XGameEventManager.Instance.RegisterEvent(evt, Proxy.OnNotify);
                }
            }

            Proxy?.OnEnable();
        }

        /// <summary>
        /// 节点初始化
        /// </summary>
        public virtual void OnAwake()
        {
            Proxy?.OnAwake();
        }

        /// <summary>
        /// 节点重置
        /// </summary>
        public virtual void OnReset()
        {
            if (Status != NodeStatus.READY)
                OnDisable();

            Status = NodeStatus.READY;
            Proxy?.OnReset();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void OnUpdate(float deltaTime)
        {
            if (Status == NodeStatus.ERROR)
            {
                Debug.LogError("运行行为树节点出错！！！！");
                return;
            }

            if (Status == NodeStatus.READY)
            {
                Status = NodeStatus.RUNNING;
                OnEnable();
            }

            if(Status == NodeStatus.RUNNING)
                Proxy?.OnUpdate(deltaTime);

 

            if (Status == NodeStatus.SUCCESS || Status == NodeStatus.FAILED)
                OnDisable();
        }


        /// <summary>
        /// 固定更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void OnFixedUpdate(float deltaTime)
        {
            Proxy?.OnFixedUpdate(deltaTime);
        }

        /// <summary>
        /// 节点完成后调用
        /// </summary>
        public virtual void OnDisable()
        {
            Proxy?.OnDisable();
        }


        public virtual void OnDestroy()
        {
            Proxy?.OnDestroy();
        }
    }
}