using UnityEngine;
namespace BehaviorTree
{
    /// <summary>
    /// 空节点
    /// </summary>
    [BehaviorNode("EmptyNodeProxy",BehaviorNodeType.Action)]
    public class EmptyNodeProxy : NodeCsProxy
    {
        public override void OnEnter()
        {
            Node.Status = NodeStatus.SUCCESS;
        }
    }
}