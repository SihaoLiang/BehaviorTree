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
        if(BehaviorUpdateMode == UpdateMode.Normal)
            base.OnUpdate(dedeltaTime);
    }


    public override void OnNotify(string evt, params object[] args)
    {
        
    }
}
