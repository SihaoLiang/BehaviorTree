

namespace BehaviorTree
{
    public class BaseDecoratorNode : BaseInheritNode
    {
        public BaseNode ChildNode = null;
        public override void AddNode(BaseNode node)
        {
            ChildNode = node;
        }

        public override void RemoveNode(BaseNode node)
        {
            ChildNode = null;
        }

        public override bool HasNode(BaseNode node)
        {
            return ChildNode != null;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (ChildNode == null)
                Status = NodeStatus.ERROR;

            base.OnUpdate(deltaTime);
        }

        public override void OnReset()
        {
            base.OnReset();
            ChildNode?.OnReset();
        }
    }
}
