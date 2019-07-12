using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;

namespace BehaviorTree
{
    public class AgentProxy 
    {
        public Agent BTAgent = null;

        /// <summary>
        /// Agent对象
        /// </summary>
        public GameObject gameObject
        {
            get {
                if (BTAgent != null)
                    return BTAgent.gameObject;
                return null;
            }
        }
   
        /// <summary>
        /// 行为树
        /// </summary>
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
        /// 监听的事件
        /// </summary>
        public List<string> Events = new List<string>();
        
        public virtual void OnStart() {
           
        }

        public virtual void SetAgent(Agent agent,string classType)
        {

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

        public virtual void OnNotify(string evt, params object[] args) {}

       
        public void AddEvent(string evt)
        {
            if (!Events.Contains(evt))
                Events.Add(evt);
        }


        public void RemoveEvent(string evt)
        {
            if (Events.Contains(evt))
                Events.Remove(evt);
        }
    }
}