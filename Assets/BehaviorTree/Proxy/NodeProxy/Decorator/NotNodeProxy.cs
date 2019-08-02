
namespace BehaviorTree
{
    /// <summary>
    /// 取反节点
    /// </summary>
    [NodeProxy("Not", BehaviorNodeType.Decorator)]
    public class NotNodeProxy : NodeCsProxy
    {
        public override void OnUpdate(float deltaTime)
        {
            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;

            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
                decoratorNode.Status = NodeStatus.ERROR;
            else if (decoratorNode.ChildNode.Status == NodeStatus.FAILED)
                decoratorNode.Status = NodeStatus.SUCCESS;
            else if(decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
                decoratorNode.Status = NodeStatus.FAILED;
        }
    }
}