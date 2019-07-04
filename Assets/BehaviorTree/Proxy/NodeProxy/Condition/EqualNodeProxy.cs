using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    [BehaviorNode("Equal", BehaviorNodeType.Condition)]
    public class EqualNodeProxy : NodeCsProxy
    {
        BaseField ConditionValue = null;
        string ConditionKey = null;

        public override void OnAwake()
        {
            if (Node.NodeDatas == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }
            if (Node.NodeDatas["ConditionValue"] == null || Node.NodeDatas["ConditionKey"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            ConditionKey = Node.NodeDatas["ConditionKey"] as StringField;
            ConditionValue = Node.NodeDatas["ConditionValue"];
        }

        public override void OnUpdate(float deltaTime)
        {
            BaseField baseFiled = Node.NodeAgent.GetVarDicByKey(ConditionKey);

            if (baseFiled == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            if (baseFiled == ConditionValue)
                Node.Status = NodeStatus.SUCCESS;
            else
                Node.Status = NodeStatus.FAILED;
        }
    }
}