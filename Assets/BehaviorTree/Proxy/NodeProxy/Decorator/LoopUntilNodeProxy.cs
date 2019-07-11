using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
namespace BehaviorTree
{
    /// <summary>
    /// 直到某个值达成前一直循环
    /// </summary>
    [BehaviorNode("LoopUntil", BehaviorNodeType.Decorator)]
    public class LoopUntilNodeProxy : NodeCsProxy
    {
        string UntilKey = null;
        BaseField UntilValue = null;

        public override void OnAwake()
        {
            if (Node.Fields == null)
                return;

            if (Node.Fields["UntilKey"] == null || Node.Fields["UntilValue"] == null)
                return;

            UntilKey = Node.Fields["UntilKey"] as StringField;
            UntilValue = Node.Fields["UntilValue"];
        }

        public override void OnUpdate(float deltaTime)
        {
            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;

            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.FAILED || decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
            {
                BaseField baseFiled = Node.NodeAgent.GetVarDicByKey(UntilKey);
                if (baseFiled == UntilValue)
                    decoratorNode.Status = decoratorNode.ChildNode.Status;
                else
                    decoratorNode.ChildNode.OnReset();
            }
          
        }
    }
}