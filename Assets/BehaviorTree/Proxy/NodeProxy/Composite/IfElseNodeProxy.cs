
namespace BehaviorTree
{
    /// <summary>
    /// 第一个节点为条件
    /// </summary>
    [NodeProxy("IfElse", BehaviorNodeType.Composite)]
    public class IfElseNodeProxy : NodeCsProxy
    {
        int CurrentRunningIndex = -1;
        BaseNode ConditionNode = null;
        public override void OnAwake()
        {
            BaseCompositeNode compositeNode = Node as BaseCompositeNode;
            if (compositeNode.Children == null || compositeNode.Children.Count != 3)
                compositeNode.Status = NodeStatus.ERROR;

            ConditionNode = compositeNode.Children[0];
        }

        public override void OnEnter()
        {
            CurrentRunningIndex = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            BaseCompositeNode compositeNode = Node as BaseCompositeNode;

            if (CurrentRunningIndex == 0)
            {
                ConditionNode.OnUpdate(deltaTime);

                if (ConditionNode.Status == NodeStatus.ERROR)
                {
                    compositeNode.Status = NodeStatus.ERROR;
                    return;
                }

                if (ConditionNode.Status == NodeStatus.RUNNING)
                    return;
            }

            if (ConditionNode.Status == NodeStatus.SUCCESS)
                CurrentRunningIndex = 1;
            else if (ConditionNode.Status == NodeStatus.FAILED)
                CurrentRunningIndex = 2;

            BaseNode CurNode = compositeNode.Children[CurrentRunningIndex];
            CurNode.OnUpdate(deltaTime);

            if (CurNode.Status == NodeStatus.ERROR)
            {
                compositeNode.Status = NodeStatus.ERROR;
                return;
            }

            compositeNode.Status = CurNode.Status;
        }



        public override void OnExit()
        {
            CurrentRunningIndex = 0;
            base.OnExit();
        }

    }
}