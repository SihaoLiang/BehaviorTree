using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[AgentProxy("Guide")]


public class GuideAgentProxy : AgentCsProxy
{
    public enum UpdateMode
    {
        Normal = 1, //正常
        Manual, //手动
    }

    public UpdateMode BehaviorUpdateMode = UpdateMode.Normal;


    public override void OnUpdate(float dedeltaTime)
    {
        base.OnUpdate(dedeltaTime);
    }

}
