using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
namespace BehaviorTree
{
    /// <summary>
    /// 等待节点
    /// </summary>
    [BehaviorNode("Wait", BehaviorNodeType.Action)]
    public class WaitNodeProxy : NodeCsProxy
    {
        float WaitSecond = 0;
        float CurSecond = 0;
        public override void OnAwake()
        {
            if (Node.NodeDatas == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            if (Node.NodeDatas["WaitSecond"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            FloatField floatField = Node.NodeDatas["WaitSecond"] as FloatField;
            WaitSecond = floatField.Value;
        }

        public override void OnEnable()
        {
            CurSecond = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            CurSecond += deltaTime;

            if (WaitSecond <= CurSecond)
                Node.Status = NodeStatus.SUCCESS;
        }

        public override void OnDisable()
        {
            CurSecond = 0;
        }
    }
}