using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    /// <summary>
    /// 失败节点
    /// </summary>
    [BehaviorNode("Failure", BehaviorNodeType.Decorator)]
    public class FailureNodeProxy : NodeCsProxy
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

            if (decoratorNode.ChildNode.Status != NodeStatus.RUNNING)
                decoratorNode.Status = NodeStatus.FAILED;

        }
    }
}