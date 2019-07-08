using BehaviorTreeData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BaseNode
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id;

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
        /// 需要Update
        /// </summary>
        public bool NeedUpdate = true;

        /// <summary>
        /// 初始化代理器
        /// </summary>
        public void InitNode(NodeData nodeData, Agent agent)
        {
            Id = NodeDatas.ID;
            NodeAgent = agent;
            NodeDatas = nodeData;
            InitProxy();
        }

        public static BaseNode GetBaseNode(string classType)
        {
            BaseNode baseNode = null;

            NodeProxyInfo nodeProxyinfo = BehaviorTreeManager.Instance.GetRegistedNodeType(classType);
            if (nodeProxyinfo == null)
            {
                Debug.LogError($"错误！！行为树节点未注册 ClassType:{classType}");
                return null;
            }

            switch (nodeProxyinfo.behaviorNodeType)
            {
                case BehaviorNodeType.Action:
                    baseNode = new BaseActionNode();
                    break;
                case BehaviorNodeType.Condition:
                    baseNode = new BaseConditionNode();
                    break;
                case BehaviorNodeType.Composite:
                    baseNode = new BaseCompositeNode();
                    break;
                case BehaviorNodeType.Decorator:
                    baseNode = new BaseDecoratorNode();
                    break;
                default:
                    Debug.LogError($"错误！！行为树节点类型错误 ClassType:{classType}");
                    break;
            }
            baseNode.NeedUpdate = nodeProxyinfo.NeedUpdate;
            return baseNode;
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

        public virtual void Enter()
        {
            OnEnter();

            string[] events = OnGetEvents();
            if (events != null && events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    string evt = events[i];
                    XGameEventManager.Instance.RegisterEvent(evt, OnNotify);
                }
            }
        }

        /// <summary>
        /// 节点运行注册事件
        /// </summary>
        public virtual void OnEnter()
        {
            Proxy?.OnEnter();     
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
                Enter();

                if (Status == NodeStatus.READY)
                    Status = NodeStatus.RUNNING;
            }

            if (Status == NodeStatus.RUNNING && NeedUpdate)
                Proxy?.OnUpdate(deltaTime);

            if (Status == NodeStatus.SUCCESS || Status == NodeStatus.FAILED)
                Exit();
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
        public virtual void Exit()
        {
            string[] events = OnGetEvents();
            if (events != null && events != null)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    string evt = events[i];
                    XGameEventManager.Instance.RemoveEvent(evt, OnNotify);
                }
            }

            OnExit();
        }



        /// <summary>
        /// 节点完成后调用
        /// </summary>
        public virtual void OnExit()
        {
            if (Proxy != null && Proxy.Events != null)
            {
                for (int i = 0; i < Proxy.Events.Length; i++)
                {
                    string evt = Proxy.Events[i];
                    XGameEventManager.Instance.RemoveEvent(evt, Proxy.OnNotify);
                }
            }

            Proxy?.OnExit();
        }

        public virtual string[] OnGetEvents()
        {
            return Proxy?.Events;
        }


        public virtual void OnDestroy()
        {
            Proxy?.OnDestroy();
        }

        public virtual void OnNotify(string evt, params object[] args)
        {
            Proxy?.OnNotify(evt, args);
        }
    }
}