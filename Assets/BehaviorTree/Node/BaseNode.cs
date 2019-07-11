﻿using BehaviorTreeData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BaseNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 节点数据
        /// </summary>
        public NodeData Fields;

        /// <summary>
        /// 节点状态
        /// </summary>
        public NodeStatus Status = NodeStatus.READY;

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
        /// 节点的信息
        /// </summary>
        public NodeProxyInfo NodeInfo;

        /// <summary>
        /// 初始化代理器
        /// </summary>
        public void InitNode(NodeData nodeData, Agent agent)
        {
            this.ID = nodeData.ID;
            this.Fields = nodeData;
            this.NodeAgent = agent;

            InitProxy();
        }

        /// <summary>
        /// 通过类型获取节点
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static BaseNode GetBaseNode(string classType)
        {
            BaseNode baseNode = null;

            NodeProxyInfo nodeProxyInfo = BehaviorTreeManager.Instance.GetRegistedNodeType(classType);

            if (nodeProxyInfo == null)
            {
                Debug.LogError($"错误！！行为树节点未注册 ClassType:{classType}");
                return null;
            }

            switch (nodeProxyInfo.behaviorNodeType)
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
            baseNode.NodeInfo = nodeProxyInfo;
            return baseNode;
        }

        /// <summary>
        /// 初始化代理器
        /// </summary>
        void InitProxy()
        {
            if (!NodeInfo.IsLua)
            {
                Type type = BehaviorTreeManager.Instance.GetBehaviorNodeType(NodeInfo.ClassType);
                Proxy = Activator.CreateInstance(type) as NodeProxy;
            }
            else
            {
                Proxy = new NodeLuaProxy();
            }

            if (Proxy == null)
            {
                Debug.LogError($"错误！！找不到行为树节点代理器 ClassType:{NodeInfo.ClassType}");
                return;
            }

            Proxy.SetNode(this);
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
                Debug.LogError($"行为节点出错！！！！ClassType:{NodeInfo.ClassType} IsLua:{NodeInfo.IsLua}");
                return;
            }

            if (Status == NodeStatus.READY)
            {
                Enter();

                if (Status == NodeStatus.READY)
                    Status = NodeStatus.RUNNING;
            }

            if (Status == NodeStatus.RUNNING && NodeInfo.NeedUpdate)
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
            if (events != null && events.Length > 0)
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