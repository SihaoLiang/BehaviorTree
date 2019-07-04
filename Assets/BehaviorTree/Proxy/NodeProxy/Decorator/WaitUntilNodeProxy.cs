using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    /// <summary>
    /// 阻塞，直到子节点返回true
    /// </summary>
    [BehaviorNode("WaitUntil", BehaviorNodeType.Decorator)]
    public class WaitUntilNodeProxy : NodeCsProxy
    {
        public override void OnUpdate(float deltaTime)
        {
            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;
            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.FAILED)
                decoratorNode.ChildNode.OnReset();
        }
    }
}