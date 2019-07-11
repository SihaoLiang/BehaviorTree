using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTree
{
    public interface ILuaNodeProxy
    {
        void OnAwake();

        void OnEnter();

        void OnExit();

        void OnDestroy();

        void OnRunning(float deltaTime);

        void OnReset();

        void OnUpdate(float deltaTime);

        void OnFixedUpdate(float deltaTime);

        string[] OnGetEvents();

        void OnNotify(string evt, params object[] args);
    }
}