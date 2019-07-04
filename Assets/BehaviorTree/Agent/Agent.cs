using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
using System;

namespace BehaviorTree {
    public class Agent : MonoBehaviour {

        /// <summary>
        /// 执行主体代理器
        /// </summary>
        public AgentProxy Proxy;

        /// <summary>
        /// 是否是lua代理
        /// </summary>
        public bool IsLueProxy = false;

        /// <summary>
        /// 代理类型
        /// </summary>
        public string ProxyType = string.Empty;

        /// <summary>
        /// 行为树
        /// </summary>
        public BehaviorTree BehaviorTreeInstance;

        /// <summary>
        /// 公共参数
        /// </summary>
        public Dictionary<string, BaseField> VarDic;

        /// <summary>
        /// 初始化代理器
        /// </summary>
        public virtual void InitProxy()
        {
          
            if (!IsLueProxy)
            {
                Type type = BehaviorTreeManager.Instance.GetAgentType(ProxyType);
                Proxy = Activator.CreateInstance(type) as AgentCsProxy;
            }
            else
            {
                Proxy = new AgentLuaProxy(ProxyType);
            }

            if (Proxy == null)
            {
                Debug.LogError($"错误！！找不到行为主体代理器 ClassType:{ProxyType}");
                return;
            }

        }

        public virtual void Awake()
        {
            InitProxy();
            Proxy?.OnAwake();
            BehaviorTreeInstance?.OnAwake();
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
                for (int i = 0; i < Proxy.Events.Length; i++)
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
                for (int i = 0; i < Proxy.Events.Length; i++)
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
            BehaviorTreeInstance?.OnFixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {

            if (!isActiveAndEnabled)
                return;


            Proxy?.OnUpdate(Time.deltaTime);

            BehaviorTreeInstance?.OnUpdate(Time.deltaTime);
        }

        private void OnDestroy()
        {
            Proxy?.OnDestroy();
            BehaviorTreeInstance?.OnDestroy();
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

            BehaviorTreeInstance = BehaviorTreeManager.Instance.GetBehaviorTreeById(id);
            BehaviorTreeInstance.SetAgent(this);
            BehaviorTreeInstance.BuildBehaviorTree();
        }


        /// <summary>
        /// 获取公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BaseField GetVarDicByKey(string key)
        {
            if (VarDic == null || !VarDic.ContainsKey(key))
                return null;

            return VarDic[key];
        }

        /// <summary>
        /// 设置公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="baseFiled"></param>
        public void SetVarDicByKey(string key, BaseField baseFiled)
        {
            if (VarDic == null)
                VarDic = new Dictionary<string, BaseField>(8);

            if (!VarDic.ContainsKey(key))
                VarDic.Add(key, baseFiled);
            else
                VarDic[key] = baseFiled;
        }
    }
}