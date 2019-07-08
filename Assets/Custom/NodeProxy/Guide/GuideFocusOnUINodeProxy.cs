
using UnityEngine;
using BehaviorTree;
using BehaviorTreeData;
/// <summary>
/// Ui聚焦
/// </summary>
[BehaviorNode("GuideFocusOnUI", BehaviorNodeType.Action)]
public class GuideFocusOnUINodeProxy : NodeCsProxy
{
    string UiName;
    string PanelName;

    public override void OnAwake()
    {
        if (Node.NodeDatas == null)
        {
            Node.Status = NodeStatus.ERROR;
            return;
        }

        if (Node.NodeDatas["UiName"] == null || Node.NodeDatas["PanelName"] == null)
        {
            Node.Status = NodeStatus.ERROR;
            return;
        }

        UiName = Node.NodeDatas["UiName"] as StringField;
        PanelName = Node.NodeDatas["PanelName"] as StringField;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }
}
