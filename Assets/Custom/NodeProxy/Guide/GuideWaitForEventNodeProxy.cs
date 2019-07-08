using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using BehaviorTreeData;
/// <summary>
/// 等待事件节点
/// </summary>
[BehaviorNode(" GuideWaitForEvent", BehaviorNodeType.Action)]
public class GuideWaitForEventNodeProxy : NodeCsProxy
{
    string WaitForEvent = string.Empty;
    const float WaitTimeOutWarn = 10f; //超时警告
    float WaitedTime = 0;

    public override void OnAwake()
    {
        if (Node.NodeDatas == null || Node.NodeDatas["WaitForEvent"] == null)
        {
            Debug.LogError("等待事件节点的事件参数不存在");
            Node.Status = NodeStatus.ERROR;
            return;
        }

        WaitForEvent = Node.NodeDatas["WaitForEvent"] as StringField;
        Events = new string[] { WaitForEvent };
    }

    public override string[] OnGetEvents()
    {
        return Events;
    }

    public override void OnUpdate(float deltaTime)
    {
        WaitedTime += deltaTime;
        if (WaitedTime >= WaitTimeOutWarn)
            Debug.LogWarning($"等待事件超时 :{WaitedTime}");
    }

    public override void OnNotify(string evt, params object[] args)
    {
        if (evt == WaitForEvent)
            Node.Status = NodeStatus.SUCCESS;
    }
}
