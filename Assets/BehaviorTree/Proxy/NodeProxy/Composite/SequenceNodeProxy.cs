using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 创建这个节点时，需要传入一个节点队列。
    /// 当运行到这个节点时。他的子节点会一个接一个的运行。
    /// 如果他的子节点状态是SUCCESS，那么他会运行下一个；
    /// 如果他的子节点状态是RUNNING，那么他会将自身也标识成RUNNING，
    /// 并且等待节点返回其他结果；如果他的子节点状态是FAILED，
    /// 那么他会把自己的状态标识为FAILED并且直接返回。
    /// 所有节点都返回结尾为SUCCESS那么他会将自身标识成为SUCCESS并且返回。
    /// </summary>
  
    [BehaviorNode("Sequence", BehaviorNodeType.Composite)]
    public class SequenceNodeProxy : NodeCsProxy
    {
        public override void OnUpdate(float deltaTime)
        {
            BaseCompositeNode compositeNode = Node as BaseCompositeNode;
            BaseNode runningNode = compositeNode.Children[compositeNode.RunningNodeIndex];

            runningNode.OnUpdate(deltaTime);

            switch (runningNode.Status)
            {
                case NodeStatus.SUCCESS:
                    {
                        compositeNode.RunningNodeIndex++;

                        if (compositeNode.RunningNodeIndex >= compositeNode.Children.Count)
                        {
                            compositeNode.Status = NodeStatus.SUCCESS;
                            break;
                        }
                        break;
                    }
                case NodeStatus.FAILED:
                    {
                        compositeNode.Status = NodeStatus.FAILED;
                        break;
                    }
                case NodeStatus.ERROR:
                    {
                        compositeNode.Status = NodeStatus.ERROR;
                        return;
                    }
            }
        }
    }
}
