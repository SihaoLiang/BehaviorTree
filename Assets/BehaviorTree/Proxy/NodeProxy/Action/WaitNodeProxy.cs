
using BehaviorTreeData;
namespace BehaviorTree
{
    /// <summary>
    /// 等待节点
    /// </summary>
    [NodeProxy("Wait", BehaviorNodeType.Action)]
    public class WaitNodeProxy : NodeCsProxy
    {
        int WaitTime = 0;
        float WaitSecond = 0;
        float CurSecond = 0;
        public override void OnAwake()
        {
            if (Node.Fields == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            if (Node.Fields["Millisecond"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            IntField floatField = Node.Fields["Millisecond"] as IntField;
            WaitTime = floatField.Value;
        }

        public override void OnEnter()
        {
            CurSecond = 0;
            WaitSecond = WaitTime / 1000;
            if (WaitTime <= CurSecond)
                Node.Status = NodeStatus.SUCCESS;
        }

        public override void OnUpdate(float deltaTime)
        {
            CurSecond += deltaTime;

            if (WaitSecond <= CurSecond)
                Node.Status = NodeStatus.SUCCESS;
        }

        public override void OnExit()
        {
            CurSecond = 0;
        }
    }
}