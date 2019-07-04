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
            if (Node.NodeDatas == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }
            
            if (Node.NodeDatas["AssignmentKey"] == null || Node.NodeDatas["AssignmentValue"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            AssignmentValue = Node.NodeDatas["AssignmentValue"];
        }

        public override void OnUpdate(float deltaTime)
        {
            Node.NodeAgent.SetVarDicByKey(AssignmentKey, AssignmentValue);
            Node.Status = NodeStatus.SUCCESS;

        }
    }
}