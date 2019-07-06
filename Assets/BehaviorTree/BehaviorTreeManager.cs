using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
using System;
namespace BehaviorTree
{
    public class BehaviorTreeManager : XSingleton<BehaviorTreeManager>
    {
        /// <summary>
        /// 所有行为树数据
        /// </summary>
        Dictionary<string, AgentData> AgentDataDic = new Dictionary<string, AgentData>();

        /// <summary>
        /// 节点代理的所有类型
        /// </summary>
        private readonly Dictionary<string, NodeProxyInfo> RegistedCsNodeProxyTypeDic = new Dictionary<string, NodeProxyInfo>();
        private readonly Dictionary<string, NodeProxyInfo> RegistedLuaNodeProxyTypeDic = new Dictionary<string, NodeProxyInfo>();

        /// <summary>
        /// 执行代理的所有类型
        /// </summary>
        private readonly Dictionary<string, Type> AgentProxyTypeDic = new Dictionary<string, Type>();

        /// <summary>
        /// 所有节点
        /// </summary>
        private readonly Dictionary<string, Type> NodeTypeDic = new Dictionary<string, Type>();

        /// <summary>
        /// 行为树池
        /// </summary>
        BehaviorTreePool Pools = null;

        /// <summary>
        /// 初始化从本地加载数据
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            InitTypes();
            Pools = Activator.CreateInstance<BehaviorTreePool>();
            Pools.name = "BehaviorTreePool";

            Load();

            return true;
        }


        public void Load()
        {

        }

        /// <summary>
        /// 判断是否存在行为树数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckBehaviorTreeDataExist(string id)
        {
            if (AgentDataDic == null || AgentDataDic.Count <= 0)
            {
                Debug.LogError($"错误！行为树数据未加载");
                return false;
            }

            return AgentDataDic.ContainsKey(id);
        }


        /// <summary>
        /// 从池中获取行为树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BehaviorTree GetBehaviorTreeFromPool(string id)
        {
            if (Pools == null)
            {
                Pools = Activator.CreateInstance<BehaviorTreePool>();
                Pools.name = "BehaviorTreePool";
            }

            return Pools.Spawn(id);
        }

        public void DisableBehaviorTree(BehaviorTree behaviorTree) {
            if (Pools == null)
            {
                Pools = Activator.CreateInstance<BehaviorTreePool>();
                Pools.name = "BehaviorTreePool";
            }

            behaviorTree.OnDisable();
            Pools.DeSpawn(behaviorTree);
        }

        /// <summary>
        /// 获取行为树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BehaviorTree GetBehaviorTreeById(string id,Agent agent)
        {
            if (!CheckBehaviorTreeDataExist(id))
            {
                Debug.LogError($"错误！行为树数据不存在 Id:{id}");
                return null;
            }

            //从池里面加载
            BehaviorTree behaviorTree = GetBehaviorTreeFromPool(id);
            if (behaviorTree == null)
            {
                AgentData agentData = AgentDataDic[id].Clone();

                if (agentData == null)
                {
                    Debug.LogError($"错误！行为树数据不存在 Id:{id}");
                    return null;
                }

                behaviorTree = new BehaviorTree(agentData, agent);
                behaviorTree.BehaviorTreeId = agentData.ID;
                behaviorTree.OnAwake();
            }

            behaviorTree.OnEnable();
            return behaviorTree;
        }

        /// <summary>
        /// 注册Cs节点代理
        /// </summary>
        /// <param name="classType">节点名字，唯一</param>
        /// <param name="nodeType">节点类型</param>
        /// <param name="isLua">Lua节点</param>
        public void RegistCsProxy(string classType, BehaviorNodeType nodeType, bool isLua)
        {
            if (RegistedCsNodeProxyTypeDic.ContainsKey(classType))
            {
                Debug.LogError($"错误:重复注册行为树节点 ClassType:{classType} BehaviorNodeType:{nodeType}");
                return;
            }

            NodeProxyInfo nodeProxyInfo = new NodeProxyInfo();
            nodeProxyInfo.ClassType = classType;
            nodeProxyInfo.behaviorNodeType = nodeType;
            nodeProxyInfo.IsLua = isLua;

            RegistedCsNodeProxyTypeDic.Add(classType, nodeProxyInfo);
        }

        /// <summary>
        /// 注册Lua节点代理
        /// </summary>
        /// <param name="classType">节点名字，唯一</param>
        /// <param name="nodeType">节点类型</param>
        /// <param name="isLua">Lua节点</param>
        public void RegistLuaProxy(string classType, BehaviorNodeType nodeType, bool isLua)
        {
            if (RegistedCsNodeProxyTypeDic.ContainsKey(classType))
            {
                Debug.LogError($"错误:重复注册行为树节点 ClassType:{classType} BehaviorNodeType:{nodeType}");
                return;
            }

            NodeProxyInfo nodeProxyInfo = new NodeProxyInfo();
            nodeProxyInfo.ClassType = classType;
            nodeProxyInfo.behaviorNodeType = nodeType;
            nodeProxyInfo.IsLua = isLua;

            RegistedCsNodeProxyTypeDic.Add(classType, nodeProxyInfo);
        }

        /// <summary>
        /// 初始化的类型
        /// </summary>
        private void InitTypes()
        {
            Type[] types = GetType().Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];

                object[] nodeAttributes = type.GetCustomAttributes(typeof(BehaviorNodeAttribute), false);
                foreach (object attribute in nodeAttributes)
                {
                    BehaviorNodeAttribute nodeAttribute = attribute as BehaviorNodeAttribute;
                    if (nodeAttribute == null)
                        continue;

                    NodeTypeDic[nodeAttribute.ClassType] = type;
                    RegistCsProxy(nodeAttribute.ClassType, nodeAttribute.NodeType, false);
                }

                object[] agentAttributes = type.GetCustomAttributes(typeof(AgentProxyAttribute), false);
                foreach (object childAttribute in agentAttributes)
                {
                    AgentProxyAttribute agentAttribute = childAttribute as AgentProxyAttribute;
                    if (agentAttribute == null)
                        continue;

                    AgentProxyTypeDic[agentAttribute.AgentName] = type;
                }
            }
        }


        /// <summary>
        /// 获取已经注册的代理器Type
        /// </summary>
        /// <param name="nodeType">代理名字</param>
        /// <returns></returns>
        public NodeProxyInfo GetRegistedNodeType(string nodeType)
        {
            NodeProxyInfo nodeProxyInfo = null;
            RegistedLuaNodeProxyTypeDic.TryGetValue(nodeType, out nodeProxyInfo);
            if (nodeProxyInfo != null)
                return nodeProxyInfo;

            RegistedCsNodeProxyTypeDic.TryGetValue(nodeType, out nodeProxyInfo);
            return nodeProxyInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public Type GetAgentType(string nodeType)
        {
            Type type = null;
            AgentProxyTypeDic.TryGetValue(nodeType, out type);
            return type;
        }

        /// <summary>
        /// 获取在C#端对应的行为树节点
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <returns></returns>
        public Type GetBehaviorNodeType(string nodeType)
        {
            Type type = null;
            NodeTypeDic.TryGetValue(nodeType, out type);
            return type;
        }

    }
}