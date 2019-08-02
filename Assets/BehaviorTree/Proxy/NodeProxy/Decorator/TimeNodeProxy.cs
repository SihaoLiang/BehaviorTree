using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// 指定时间内运行
    /// </summary>
    [NodeProxy("Time", BehaviorNodeType.Decorator)]
    public class TimeNodeProxy : NodeCsProxy
    {
        int Duration = 0;
        float CurTime = 0;

        public override void OnAwake()
        {
            if (Node.Fields == null || Node.Fields["Duration"] == null)
                return;

            IntField field = Node.Fields["Duration"] as IntField;
            Duration = field.Value / 1000;
        }

        public override void OnEnter()
        {
            CurTime = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            CurTime += deltaTime;

            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;

            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (Duration <= CurTime)
            {
                decoratorNode.Status = NodeStatus.SUCCESS;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.FAILED || decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
                decoratorNode.ChildNode.OnReset();
        }

        public override void OnReset()
        {
            CurTime = 0;
            base.OnReset();
        }

        public override void OnExit()
        {
            CurTime = 0;
            base.OnExit();
        }

    }
}