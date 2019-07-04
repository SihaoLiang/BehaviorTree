using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECS;
public abstract class AgentProxy : Entity
{
    public GameObject Agent = null;
    public string[] Events = null;
    public abstract void OnStart();

    public abstract void OnAwake();

    public abstract void OnEnable();

    public abstract void OnDisable();

    public abstract void OnDestroy();

    public abstract void OnUpdate(float dedeltaTime);

    public abstract void OnFixedUpdate(float dedeltaTime);

    public abstract string[] OnGetEvents();

    public abstract void OnNotify(string evt, params object[] args);

}