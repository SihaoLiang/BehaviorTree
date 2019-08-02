﻿using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// 循环节点 -1无限循环
    /// </summary>
    [NodeProxy("Loop", BehaviorNodeType.Decorator)]
    public class LoopNodeProxy : NodeCsProxy {

        int LoopTimes = 0;
        int CurTimes = 0;

        public override void OnAwake()
        {

            if (Node.Fields == null || Node.Fields["LoopTimes"] == null)
                return;

            IntField field = Node.Fields["LoopTimes"] as IntField;
            LoopTimes = field.Value;
        }


        public override void OnEnter()
        {
            CurTimes = 0;
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
                CurTimes++;
                if (LoopTimes <= CurTimes && LoopTimes != -1)
                    decoratorNode.Status = NodeStatus.SUCCESS;
                else
                    decoratorNode.ChildNode.OnReset();
            }
        }

        public override void OnReset()
        {
            CurTimes = 0;
            base.OnReset();
        }

        public override void OnExit()
        {
            CurTimes = 0;
            base.OnExit();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
