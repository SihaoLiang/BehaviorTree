using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public interface ILuaAgentProxy
    {
        void OnStart();

        void OnAwake();


        void OnEnable();


        void OnDisable();


        void OnDestroy();


        void OnUpdate(float dedeltaTime);


        void OnFixedUpdate(float dedeltaTime);

        string[] OnGetEvents();

        void OnNotify(string evt, params object[] args);

    }
}