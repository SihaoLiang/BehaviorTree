using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    public interface ILuaNodeProxy
    {
        void OnAwake();

        void OnEnable();

        void OnDisable();

        void OnDestroy();

        void OnRunning(float deltaTime);

        void OnReset();

        void OnUpdate(float deltaTime);

        void OnFixedUpdate(float deltaTime);

        string[] OnGetEvents();

        void OnNotify(string evt, params object[] args);
    }
}