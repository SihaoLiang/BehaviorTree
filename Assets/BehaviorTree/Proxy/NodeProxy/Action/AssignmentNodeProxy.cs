using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    /// <summary>
    /// 赋值节点
    /// </summary>
    [BehaviorNode("Assignment", BehaviorNodeType.Action)]
    public class AssignmentNodeProxy : NodeCsProxy
    {
        BaseField AssignmentValue = null;
        string AssignmentKey = null;

        public override void OnAwake()
        {
            if (Node.Fields == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }
            
            if (Node.Fields["AssignmentKey"] == null || Node.Fields["AssignmentValue"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            AssignmentValue = Node.Fields["AssignmentValue"];
        }

        public override void OnEnter()
        {
            Node.NodeAgent.SetVarDicByKey(AssignmentKey, AssignmentValue);
            Node.Status = NodeStatus.SUCCESS;
        }
    }
}