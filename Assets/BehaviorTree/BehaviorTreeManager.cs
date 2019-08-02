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

        /// /// <summary>
        /// 所有节点
        /// </summary>
        private readonly Dictionary<string, Type> NodeTypeDic = new Dictionary<string, Type>();
        private readonly Dictionary<string, NodeProxyInfo> RegistedCsNodeProxyTypeDic = new Dictionary<string, NodeProxyInfo>();
        private readonly Dictionary<string, NodeProxyInfo> RegistedLuaNodeProxyTypeDic = new Dictionary<string, NodeProxyInfo>();

        /// <summary>
        /// 执行代理的所有类型
        /// </summary>
        private readonly Dictionary<string, Type> AgentProxyTypeDic = new Dictionary<string, Type>();
        private readonly Dictionary<string, AgentProxyTypeInfo> RegistedCsAgentProxyTypeDic = new Dictionary<string, AgentProxyTypeInfo>();
        private readonly Dictionary<string, AgentProxyTypeInfo> RegistedLuaAgentProxyTypeDic = new Dictionary<string, AgentProxyTypeInfo>();

        /// <summary>
        /// 行为树池
        /// </summary>
        BehaviorTreePool Pools = null;

        /// <summary>
        /// 数据地址
        /// </summary>
        List<string> DataSources = new List<string>
        {
            "Assets/Product/Behavior/Guide/GuideData.bytes",
            "Assets/Product/Behavior/Dormitory/DormitoryData.bytes"
        };


        /// <summary>
        /// 初始化从本地加载数据
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            InitTypes();
            InitBehaviorTreePool();
            return Load();
        }

        /// <summary>
        /// 注册行为树缓冲池
        /// </summary>
        void InitBehaviorTreePool()
        {
            if (Pools == null)
            {
                GameObject gameObject = new GameObject("BehaviorTreePool", typeof(BehaviorTreePool));
                Pools = gameObject.GetComponent<BehaviorTreePool>();
            }
        }

        /// <summary>
        /// 添加数据源
        /// </summary>
        /// <param name="path"></param>
        public void AddDataSource(string path)
        {
            if (DataSources == null)
                DataSources = new List<string>();

            if (DataSources.Contains(path))
                return;

            DataSources.Add(path);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            AgentDataDic.Clear();

            if (DataSources == null || DataSources.Count == 0)
            {
                Debug.LogError($"XBehaviorTreeManager.Init() Error:LoadFail DataSources is empty");
                return false;
            }

            for (int i = 0; i < DataSources.Count; i++)
            {
                string dataUrl = DataSources[i];
                //IResource resource = ResourceManager.Load(dataUrl);
                //if (resource == null || !resource.Asset)
                //{
                //    XLog.Error($"XBehaviorTreeManager.Init() Error:LoadFail:[{dataUrl}]");
                //    return false;
                //}

                TextAsset textAsset = null;
                TreeData treeData = Serializer.DeSerialize<TreeData>(textAsset.bytes);


                if (treeData == null || treeData.Agents == null)
                {
                    Debug.LogError($"XBehaviorTreeManager.Init() Error:{dataUrl} Serializer.DeSerialize Error");
                    return false;
                }

                for (int index = 0; index < treeData.Agents.Count; index++)
                {
                    AgentData agentData = treeData.Agents[index];
                    if (AgentDataDic.ContainsKey(agentData.ID))
                    {
                        Debug.LogError($"XBehaviorTreeManager.Init() Error:Id Conflict Id:[{agentData.ID}]");
                        return false;
                    }
                    AgentDataDic.Add(agentData.ID, agentData);
                }
            }

            return true;
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
            return Pools.Spawn(id);
        }

        /// <summary>
        /// 回收行为树
        /// </summary>
        /// <param name="behaviorTree"></param>
        public void DisableBehaviorTree(BehaviorTree behaviorTree)
        {
            behaviorTree.OnDisable();
            behaviorTree.OnRecycle();
            behaviorTree.BTAgent = null;
            Pools.DeSpawn(behaviorTree);
        }

        /// <summary>
        /// 获取行为树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BehaviorTree GetBehaviorTreeById(string id)
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
                AgentData agentData = AgentDataDic[id];

                if (agentData == null)
                {
                    Debug.LogError($"错误！行为树数据不存在 Id:{id}");
                    return null;
                }

                behaviorTree = new BehaviorTree(agentData);
                behaviorTree.Id = agentData.ID;
                behaviorTree.OnAwake();
            }

            return behaviorTree;
        }

        /// <summary>
        /// 注册Cs节点代理
        /// </summary>
        /// <param name="classType">节点名字，唯一</param>
        /// <param name="nodeType">节点类型</param>
        /// <param name="isLua">是否Lua节点</param>
        /// <param name="needUpdate">是否Update</param>
        public void RegisterCsNodeProxy(string classType, BehaviorNodeType nodeType, bool isLua, bool needUpdate)
        {
            if (RegistedCsNodeProxyTypeDic.ContainsKey(classType))
            {
                Debug.LogError($"错误:重复注册行为树节点 ClassType:{classType} BehaviorNodeType:{nodeType}");
                return;
            }

            NodeProxyInfo nodeProxyInfo = new NodeProxyInfo
            {
                ClassType = classType,
                behaviorNodeType = nodeType,
                IsLua = isLua,
                NeedUpdate = needUpdate
            };

            RegistedCsNodeProxyTypeDic.Add(classType, nodeProxyInfo);
        }

        /// <summary>
        /// 注册Lua节点代理
        /// </summary>
        /// <param name="classType">代理名字，唯一</param>
        /// <param name="nodeType">代理类型</param>
        /// <param name="isLua">是否Lua代理</param>
        public void RegisterLuaNodeProxy(string classType, BehaviorNodeType nodeType, bool isLua, bool needUpdate)
        {
            if (RegistedLuaNodeProxyTypeDic.ContainsKey(classType))
            {
                Debug.LogError($"错误:Lua重复注册行为树节点 ClassType:{classType} BehaviorNodeType:{nodeType}");
                return;
            }

            NodeProxyInfo nodeProxyInfo = new NodeProxyInfo
            {
                ClassType = classType,
                behaviorNodeType = nodeType,
                IsLua = isLua,
                NeedUpdate = needUpdate
            };
            RegistedLuaNodeProxyTypeDic.Add(classType, nodeProxyInfo);
        }


        /// <summary>
        /// 注册luaAgent代理
        /// </summary>
        /// <param name="classType"></param>
        public void RegisterCsAgentProxy(string classType)
        {
            if (RegistedCsAgentProxyTypeDic.ContainsKey(classType))
            {
                Debug.LogError($"错误:重复注册行为主体 ClassType:{classType} ");
                return;
            }

            AgentProxyTypeInfo agentTypeInfo = new AgentProxyTypeInfo
            {
                AgentName = classType,
                IsLua = false,
            };

            RegistedCsAgentProxyTypeDic.Add(classType, agentTypeInfo);
        }

        /// <summary>
        /// 注册luaAgent代理
        /// </summary>
        /// <param name="classType"></param>
        public void RegisterLuaAgentProxy(string classType)
        {
            if (RegistedLuaAgentProxyTypeDic.ContainsKey(classType))
            {
                Debug.LogError($"错误:Lua重复注册行为主体 ClassType:{classType}");
                return;
            }

            AgentProxyTypeInfo agentTypeInfo = new AgentProxyTypeInfo
            {
                AgentName = classType,
                IsLua = true,
            };
            RegistedLuaAgentProxyTypeDic.Add(classType, agentTypeInfo);
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

                object[] nodeAttributes = type.GetCustomAttributes(typeof(NodeProxyAttribute), false);
                foreach (object attribute in nodeAttributes)
                {
                    NodeProxyAttribute nodeAttribute = attribute as NodeProxyAttribute;
                    if (nodeAttribute == null)
                        continue;

                    NodeTypeDic[nodeAttribute.ClassType] = type;
                    RegisterCsNodeProxy(nodeAttribute.ClassType, nodeAttribute.NodeType, false, nodeAttribute.NeedUpdate);
                }

                object[] agentAttributes = type.GetCustomAttributes(typeof(AgentProxyAttribute), false);
                foreach (object childAttribute in agentAttributes)
                {
                    AgentProxyAttribute agentAttribute = childAttribute as AgentProxyAttribute;
                    if (agentAttribute == null)
                        continue;

                    AgentProxyTypeDic[agentAttribute.AgentName] = type;
                    RegisterCsAgentProxy(agentAttribute.AgentName);
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
        /// 获取Agent代理
        /// </summary>
        /// <param name="agentName"></param>
        /// <returns></returns>
        public AgentProxyTypeInfo GetRegistedAgentType(string agentName)
        {
            AgentProxyTypeInfo agentTypeInfo = null;
            RegistedLuaAgentProxyTypeDic.TryGetValue(agentName, out agentTypeInfo);
            if (agentTypeInfo != null)
                return agentTypeInfo;

            RegistedCsAgentProxyTypeDic.TryGetValue(agentName, out agentTypeInfo);
            return agentTypeInfo;
        }

        /// <summary>
        /// 获取行为节点类型
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