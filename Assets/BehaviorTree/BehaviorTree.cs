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
        public string BehaviorTreeId;
        
        /// <summary>
        /// 行为树全部数据
        /// </summary>
        public AgentData BehaviorTreeData;
        
        /// <summary>
        /// 行为树开始节点
        /// </summary>
        public BaseNode StartNode;

        /// <summary>
        /// 行为树所有节点
        /// </summary>
        public List<BaseNode> AllNodes;

        /// <summary>
        /// 主体
        /// </summary>
        public Agent BehaviorAgent;

        public bool IsEnable = false;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="agentData"></param>
        public BehaviorTree(AgentData agentData,Agent agent)
        {

            if (BehaviorTreeData == null || BehaviorTreeData.StartNode == null)
            {
                Debug.LogError("行为树数据加载异常!!!!");
                return;
            }
            BehaviorAgent = agent;
            BehaviorTreeData = agentData;
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
            StartNode = BuildBehaviorTreeRecursive(BehaviorTreeData.StartNode);
        }

        /// <summary>
        /// 递归生成节点
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        protected BaseNode BuildBehaviorTreeRecursive(NodeData nodeData)
        {
            if (nodeData == null)
                return null;

            BaseNode parentNode = InstanceBehaviorTreeNodeByData(nodeData);
            AllNodes.Add(parentNode);

            if (nodeData.Childs == null || nodeData.Childs.Count <= 0)
                return StartNode;

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
        /// 创建节点
        /// </summary>
        /// <param name="nodeData"></param>
        /// <returns></returns>
        protected BaseNode InstanceBehaviorTreeNodeByData(NodeData nodeData)
        {
            string nodeType = nodeData.ClassType;
            BaseNode baseNode = BaseNode.GetBaseNode(nodeData.ClassType);
            baseNode.InitNode(nodeData, BehaviorAgent);
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

        /// <summary>
        /// 驱动行为树
        /// </summary>
        /// <param name="deltaTime"></param>
        public void OnUpdate(float deltaTime)
        {
            if (!IsEnable)
                return;

            if (StartNode != null)
                StartNode.OnUpdate(deltaTime);
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
                baseNode.OnDisable();
                baseNode.OnReset();
            }
        }
    }
}