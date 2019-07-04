﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 成功节点
    /// </summary>
    [BehaviorNode("Success", BehaviorNodeType.Decorator)]
    public class SuccessNodeProxy : NodeCsProxy
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
                decoratorNode.Status = NodeStatus.SUCCESS;

        }
    }
}