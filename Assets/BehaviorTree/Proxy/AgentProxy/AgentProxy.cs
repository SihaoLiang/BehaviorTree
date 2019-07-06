using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECS;
using BehaviorTreeData;

namespace BehaviorTree
{
    public class AgentProxy : Entity
    {
        public Agent BTAgent = null;

        public GameObject gameObject
        {
            get {
                if (BTAgent != null)
                    return BTAgent.gameObject;
                return null;
            }
        }
   
        protected BehaviorTree BTree
        {
            get
            {
                if (BTAgent != null)
                    return BTAgent.BTree;
                return null;
            }
        }
        /// <summary>
        /// 公共参数
        /// </summary>
        public Dictionary<string, BaseField> VarDic;
        public List<string> Events = new List<string>();
        
        public virtual void OnStart() {
           
        }

        public virtual void OnAwake()
        {
            BTree?.OnAwake();
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }

        public virtual void OnDestroy()
        {
            BTree?.OnDestroy();
        }

        public virtual void OnUpdate(float dedeltaTime)
        {
            BTree?.OnUpdate(dedeltaTime);
        }

        public virtual void OnFixedUpdate(float dedeltaTime)
        {
            BTree?.OnFixedUpdate(dedeltaTime);
        }

        public virtual List<string> OnGetEvents()
        {
            return Events;
        }

        public virtual void OnNotify(string evt, params object[] args) { }

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

        public void AddEvent(string evt)
        {
            if (Events == null)
                Events = new List<string>();

            if (!Events.Contains(evt))
                Events.Add(evt);
        }


        public void RemoveEvent(string evt)
        {
            if (Events == null)
                Events = new List<string>();

            if (Events.Contains(evt))
                Events.Remove(evt);
        }
    }
}