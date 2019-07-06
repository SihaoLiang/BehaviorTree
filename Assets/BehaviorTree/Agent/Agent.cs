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
        public BehaviorTree BTree;

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
            Proxy.BTAgent = this;
        }

        public virtual void Awake()
        {
            InitProxy();
            Proxy?.OnAwake();
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

            Proxy?.OnUpdate(Time.deltaTime);
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
            if (BTree != null)
                BehaviorTreeManager.Instance.DisableBehaviorTree(BTree);

            //创建行为树
            BTree = BehaviorTreeManager.Instance.GetBehaviorTreeById(id,this);

            //初始化AgentData字段
            if (BTree.BehaviorTreeData != null && BTree.BehaviorTreeData.Fields != null)
            {
                List<BaseField> fields = BTree.BehaviorTreeData.Fields;
                for (int index = 0; index < fields.Count; index++)
                {
                    SetVarDicByKey(fields[index].FieldName, fields[index]);
                }
            }

        }


        /// <summary>
        /// 获取公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BaseField GetVarDicByKey(string key)
        {
           return Proxy?.GetVarDicByKey(key);
        }

        /// <summary>
        /// 设置公共参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="baseFiled"></param>
        public void SetVarDicByKey(string key, BaseField baseFiled)
        {
            Proxy?.SetVarDicByKey(key, baseFiled);
        }
    }
}