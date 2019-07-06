using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// 指定时间内运行
    /// </summary>
    [BehaviorNode("Time", BehaviorNodeType.Decorator)]
    public class TimeNodeProxy : NodeCsProxy
    {
        int Duration = 0;
        float CurTime = 0;

        public override void OnAwake()
        {
            if (Node.NodeDatas == null || Node.NodeDatas["Duration"] == null)
                return;

            IntField field = Node.NodeDatas["Duration"] as IntField;
            Duration = field.Value / 1000;
        }

        public override void OnEnable()
        {
            CurTime = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            CurTime += deltaTime;

            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;

            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.FAILED || decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
                decoratorNode.ChildNode.OnReset();
       
            if (Duration <= CurTime)
                decoratorNode.Status = NodeStatus.SUCCESS;
        }

        public override void OnReset()
        {
            CurTime = 0;
            base.OnReset();
        }

        public override void OnDisable()
        {
            CurTime = 0;
            base.OnDisable();
        }

    }
}