using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree {
    public class BehaviorTree
    {
        /// <summary>
        /// 行为树Id
        /// </summary>
        public string Id;
        
        /// <summary>
        /// 行为树全部数据
        /// </summary>
        public AgentData BTAgentData;
        
        /// <summary>
        /// 行为树开始节点
        /// </summary>
        public BaseNode StartNode;

        /// <summary>
        /// 行为树所有节点
        /// </summary>
        public List<BaseNode> AllNodes = new List<BaseNode>();

        /// <summary>
        /// 主体
        /// </summary>
        public Agent BTAgent;

        /// <summary>
        /// 可用
        /// </summary>
        public bool IsEnable = false;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="agentData"></param>
        public BehaviorTree(AgentData agentData,Agent agent)
        {
            if (agentData == null || agentData.StartNode == null)
            {
                Debug.LogError("行为树数据加载异常!!!!");
                return;
            }
            BTAgent = agent;
            BTAgentData = agentData;
            BuildBehaviorTreeNodes();
        }

        /// <summary>
        /// 构建行为树
        /// </summary>
        /// <param name="agentData"></param>
        /// <returns></returns>
        protected void BuildBehaviorTreeNodes()
        {
            AllNodes.Clear();
            StartNode = BuildBehaviorTreeRecursive(BTAgentData.StartNode);
        }

        /// <summary>
        /// 递归生成节点
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        protected BaseNode BuildBehaviorTreeRecursive(NodeData nodeData)
        {
            if (nodeData == null)
            {
                Debug.LogError("NodeData is null");
                return null;
            }

            BaseNode parentNode = InstanceNodeByData(nodeData);
            AllNodes.Add(parentNode);

            if (nodeData.Childs == null || nodeData.Childs.Count <= 0)
                return parentNode;

            for (int index = 0; index < nodeData.Childs.Count; index++)
            {
                BaseNode childNode = BuildBehaviorTreeRecursive(nodeData.Childs[index]);
                childNode.Parent = parentNode;

                if (parentNode is BaseInheritNode)
                {
                    BaseInheritNode inheritNode = parentNode as BaseInheritNode;
                    inheritNode.AddNode(childNode);
                }
            }

            return parentNode;
        }

        /// <summary>
        /// 创建节点实例
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        protected BaseNode InstanceNodeByData(NodeData nodeData)
        {
            string nodeType = nodeData.ClassType;
            BaseNode baseNode = BaseNode.GetBaseNode(nodeData.ClassType);
            baseNode.InitNode(nodeData, BTAgent);
            return baseNode;
        }

        public void OnAwake()
        {
            if (AllNodes == null || AllNodes.Count <= 0)
                return;

            for (int index = 0; index < AllNodes.Count; index++)
            {
                BaseNode baseNode = AllNodes[index];
                baseNode.OnAwake();
            }
        }

        /// <summary>
        /// 删除的时候触发
        /// </summary>
        public void OnDestroy()
        {
            if (AllNodes == null || AllNodes.Count <= 0)
                return;

            for (int index = 0; index < AllNodes.Count; index++)
            {
                BaseNode baseNode = AllNodes[index];
                baseNode.OnDestroy();
            }
        }

        public void OnBegin()
        {

        }

        public void OnEnd()
        {
        }

        /// <summary>
        /// 驱动行为树
        /// </summary>
        /// <param name="deltaTime"></param>
        public void OnUpdate(float deltaTime)
        {
            if (!IsEnable)
                return;

            if (StartNode.Status == NodeStatus.ERROR)
                return;

            if (StartNode.Status == NodeStatus.SUCCESS || StartNode.Status == NodeStatus.FAILED)
                return;
       
            if (StartNode.Status == NodeStatus.READY)
                OnBegin();

            if (StartNode != null)
                StartNode.OnUpdate(deltaTime);

            if (StartNode.Status != NodeStatus.RUNNING)
                OnEnd();
        }

        /// <summary>
        /// 驱动行为树
        /// </summary>
        /// <param name="deltaTime"></param>
        public void OnFixedUpdate(float deltaTime)
        {
            if (!IsEnable)
                return;

            if (StartNode != null)
                StartNode.OnFixedUpdate(deltaTime);
        }

        /// <summary>
        /// 行为树出池时候触发
        /// </summary>
        public void OnEnable()
        {
            IsEnable = true;
        }

        /// <summary>
        /// 行为树回池会触发
        /// </summary>
        public void OnDisable()
        {
            IsEnable = false;
            ResetAllNode();
        }

        /// <summary>
        ///入池的时候重置所有节点
        /// </summary>
        public void ResetAllNode()
        {
            if (AllNodes == null || AllNodes.Count <= 0)
                return;

            for (int index = 0; index < AllNodes.Count; index++)
            {
                BaseNode baseNode = AllNodes[index];
                baseNode.OnExit();
                baseNode.OnReset();
            }
        }
    }
}