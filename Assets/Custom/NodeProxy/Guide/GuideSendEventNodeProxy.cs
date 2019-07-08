using BehaviorTree;
using BehaviorTreeData;
using UnityEngine;

/// <summary>
/// 打开UI
/// </summary>
[BehaviorNode("GuideOpenUI", BehaviorNodeType.Action)]

public class GuideSendEventNodeProxy : NodeCsProxy {
    string EventName;

    public override void OnAwake()
    {
        if (Node.NodeDatas == null)
        {
            Node.Status = NodeStatus.ERROR;
            return;
        }

        if (Node.NodeDatas["EventName"] == null)
        {
            Node.Status = NodeStatus.ERROR;
            return;
        }

        EventName = Node.NodeDatas["EventName"] as StringField;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }
}
