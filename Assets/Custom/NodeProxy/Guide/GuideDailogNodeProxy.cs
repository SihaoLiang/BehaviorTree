using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using BehaviorTreeData;

/// <summary>
/// 引导设置对话
/// </summary>
[BehaviorNode("GuideDailog", BehaviorNodeType.Action)]
public class GuideDailogNodeProxy : NodeCsProxy
{
    string Image;
    string Name;
    string Content;

    public override void OnAwake()
    {
        if (Node.NodeDatas == null)
        {
            Node.Status = NodeStatus.ERROR;
            return;
        }

        if (Node.NodeDatas["Image"] == null || Node.NodeDatas["Name"] == null || Node.NodeDatas["Content"] == null)
        {
            Node.Status = NodeStatus.ERROR;
            return;
        }

        Image = Node.NodeDatas["Image"] as StringField;
        Name = Node.NodeDatas["Name"] as StringField;
        Content = Node.NodeDatas["Content"] as StringField;
    }

    public override void OnUpdate(float deltaTime)
    {
        //todo
    }
}
