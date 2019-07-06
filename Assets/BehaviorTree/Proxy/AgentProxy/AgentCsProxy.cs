using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class AgentCsProxy : AgentProxy
    {
        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnFixedUpdate(float dedeltaTime)
        {
            base.OnFixedUpdate(dedeltaTime);
        }

        public override List<string> OnGetEvents()
        {
            return Events;
        }

        public override void OnNotify(string evt, params object[] args)
        {
            base.OnNotify(evt, args);
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate(float dedeltaTime)
        {
            base.OnUpdate(dedeltaTime);
        }
    }
}