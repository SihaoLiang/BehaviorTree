namespace BehaviorTree
{
    /// <summary>
    /// 空节点
    /// </summary>
    [NodeProxy("Noop", BehaviorNodeType.Action)]
    public class EmptyNodeProxy : NodeCsProxy
    {
        public override void OnEnter()
        {
            Node.Status = NodeStatus.SUCCESS;
        }
    }
}