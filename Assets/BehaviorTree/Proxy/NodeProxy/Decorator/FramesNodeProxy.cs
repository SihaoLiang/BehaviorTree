using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// 帧数节点用于在指定的帧数内，持续调用其子节点
    /// </summary>
    [BehaviorNode("Frames", BehaviorNodeType.Decorator)]
    public class FramesNodeProxy : NodeCsProxy
    {
        int Frames = -1;
        int CurFrames = -1;

        public override void OnAwake()
        {
            if (Node.Fields == null || Node.Fields["Frames"] == null)
                return;

            IntField field = Node.Fields["Frames"] as IntField;
            Frames = field.Value;
        }

        public override void OnEnter()
        {
            CurFrames = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            CurFrames++;

            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;

            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (Frames <= CurFrames)
            {
                decoratorNode.Status = NodeStatus.SUCCESS;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.FAILED || decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
                decoratorNode.ChildNode.OnReset();

        }

        public override void OnReset()
        {
            CurFrames = 0;
            base.OnReset();
        }

    }
}