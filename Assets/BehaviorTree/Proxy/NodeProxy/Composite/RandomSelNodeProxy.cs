using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// 随机选择
    /// </summary>
    [BehaviorNode("RandomSelector", BehaviorNodeType.Composite)]
    public class RandomSelNodeProxy : RandomSeqNodeProxy
    {
        public override void OnUpdate(float deltaTime)
        {
            BaseNode runningNode = Children[CompositeNode.RunningNodeIndex];

            runningNode.OnUpdate(deltaTime);

            switch (runningNode.Status)
            {
                case NodeStatus.SUCCESS:
                    {
                        CompositeNode.Status = NodeStatus.SUCCESS;
                        break;
                    }
                case NodeStatus.FAILED:
                    {
                        CompositeNode.RunningNodeIndex++;
                        //所有运行失败将返回失败
                        if (CompositeNode.RunningNodeIndex >= Children.Count)
                        {
                            CompositeNode.Status = NodeStatus.FAILED;
                            break;
                        }

                        break;
                    }
                case NodeStatus.ERROR:
                    {
                        CompositeNode.Status = NodeStatus.ERROR;
                        break;
                    }
            }
        }
    }
}