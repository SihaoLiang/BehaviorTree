using BehaviorTree;
using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[BehaviorNode("GuideOpenUI", BehaviorNodeType.Action)]

public class GuideOpenUINodeProxy : NodeCsProxy
{
    string UiName;
    public override void OnAwake()
    {
        if (Node.NodeDatas == null || Node.NodeDatas["UiName"] == null)
        {
            Debug.LogError("等待事件节点的事件参数不存在");
            Node.Status = NodeStatus.ERROR;
            return;
        }

        UiName = Node.NodeDatas["UiName"] as StringField;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }
}
