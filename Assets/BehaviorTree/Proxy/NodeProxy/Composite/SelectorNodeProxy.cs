using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 与顺序节点类似，创建时需要传入一个节点列表，
    /// 当运行到这个节点时，他的节点会一个接一个的运行。
    /// 如果他的子节点是SUCCESS，那么他会将自身标识成为SUCCESS并且直接返回；
    /// 如果他的子节点状态是RUNNING，那么他会将自身也标识成RUNNING，并且等待节点返回其他结果；
    /// 如果他的子节点状态是FAILED，那么他会运行下一个。
    /// 任何一个节点都没有返回SUCCESS的情况下，他将会将自身标识成为FAILED并且返回
    /// </summary>
    
    [BehaviorNode("Selector", BehaviorNodeType.Composite)]
    public class SelectorNodeProxy : NodeCsProxy
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
                        compositeNode.Status = NodeStatus.SUCCESS;
                        break;
                    }
                case NodeStatus.FAILED:
                    {
                        compositeNode.RunningNodeIndex++;
                        //所有运行失败将返回失败
                        if (compositeNode.RunningNodeIndex >= compositeNode.Children.Count)
                        {
                            compositeNode.Status = NodeStatus.FAILED;
                            break;
                        }

                        break;
                    }
                case NodeStatus.ERROR:
                    {
                        compositeNode.Status = NodeStatus.ERROR;
                        break;
                    }
            }
        }
    }
}
