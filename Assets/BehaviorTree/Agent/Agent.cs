using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree
{
    public class Agent : MonoBehaviour
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return gameObject.GetHashCode(); }
        }
        /// <summary>
        /// 执行主体代理器
        /// </summary>
        public AgentProxy Proxy;

        /// <summary>
        /// 代理类型
        /// </summary>
        public string ProxyType = string.Empty;

        /// <summary>
        /// 行为树
        /// </summary>
        public BehaviorTree BTree;

        /// <summary>
        /// 公共参数
        /// </summary>
        public Dictionary<string, object> VarDic = new Dictionary<string, object>();

        /// <summary>
        /// 初始化代理器
        /// </summary>
        public virtual void InitProxy()
        {
            AgentProxyTypeInfo agentTypeInfo = BehaviorTreeManager.Instance.GetRegistedAgentType(ProxyType);
            if (agentTypeInfo == null)
            {
                Debug.LogError($"行为主体未注册 AgentName ：{ProxyType}");
                return;
            }

            if (!agentTypeInfo.IsLua)
            {
                Type type = BehaviorTreeManager.Instance.GetAgentType(agentTypeInfo.AgentName);
                Proxy = Activator.CreateInstance(type) as AgentCsProxy;
            }
            else
            {
                Proxy = new AgentLuaProxy();
            }

            if (Proxy == null)
            {
                Debug.LogError($"错误！！找不到行为主体代理器 ClassType:{ProxyType}");
                return;
            }

            Proxy.SetAgent(this, agentTypeInfo.AgentName);
            Proxy?.OnAwake();
        }

        public virtual void Awake()
        {
            //  InitProxy();
            //  Proxy?.OnAwake();
        }


        public virtual void Start()
        {
            Proxy?.OnStart();
        }

        private void OnEnable()
        {


            Proxy?.OnEnable();

            if (Proxy != null && Proxy.Events != null)
            {
                for (int i = 0; i < Proxy.Events.Count; i++)
                {
                    string evt = Proxy.Events[i];
                    XGameEventManager.Instance.RegisterEvent(evt, Proxy.OnNotify);
                }
            }
        }

        private void OnDisable()
        {
            if (Proxy != null && Proxy.Events != null)
            {
                for (int i = 0; i < Proxy.Events.Count; i++)
                {
                    string evt = Proxy.Events[i];
                    XGameEventManager.Instance.RemoveEvent(evt, Proxy.OnNotify);
                }
            }

            Proxy?.OnDisable();
        }

        private void FixedUpdate()
        {
            Proxy?.OnFixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (!isActiveAndEnabled)
                return;

            try
            {
                Proxy?.OnUpdate(Time.deltaTime);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void OnDestroy()
        {
            Proxy?.OnDestroy();
        }

        /// <summary>
        /// 执行行为树
        /// </summary>
        /// <param name="id"></param>
        public void PlayBehavior(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("Error: Behavior Id IsNullOrEmpty!!!");
                return;
            }

            //上一棵树释放
            if (BTree != null && BTree.Id != id)
            {
                BehaviorTreeManager.Instance.DisableBehaviorTree(BTree);
                BTree = null;
            }

            //创建行为树
            if (BTree == null)
                BTree = BehaviorTreeManager.Instance.GetBehaviorTreeById(id);

            BTree.Reset();

            if (BTree == null)
            {
                Debug.LogError($"创建行为树失败 Id:{id}");
                return;
            }



            BTree.SetAgent(this);
            if (isActiveAndEnabled)
                BTree.OnEnable();
        }

        /// <summary>
        /// 获取公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetVarDicByKey(string key)
        {
            object baseField = null;
            if (!VarDic.TryGetValue(key, out baseField))
                Debug.LogError($"ID{ID} 找不到公共参数 KEY:{key}");

            return baseField;
        }

        /// <summary>
        /// 设置公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="baseFiled"></param>
        public void SetVarDicByKey(string key, object baseFiled)
        {
            if (!VarDic.ContainsKey(key))
                VarDic.Add(key, baseFiled);
            else
                VarDic[key] = baseFiled;
        }


        /// <summary>
        /// 设置公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="baseFiled"></param>
        public void ClearVarDic()
        {
            VarDic.Clear();
        }


        public bool CheckIsVarDicContainsKey(string key)
        {
            if (!VarDic.ContainsKey(key))
                Debug.LogError($"CheckIsVarDicContainsKey 找不到公共参数 KEY:{key}");

            return VarDic.ContainsKey(key);
        }
    }
}