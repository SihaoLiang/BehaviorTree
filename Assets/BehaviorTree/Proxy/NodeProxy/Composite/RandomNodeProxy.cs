using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// 创建这个节点的时候需要传入一个节点队列和可以不传的列表对应的权重。
    /// 在没有权重的情况下，所有的子节点完全随机。在有权重的情况下，
    /// 子节点会按照权重的设置值来选择某一个节点来运行，并且将这个节点的运行结果标识为自己的状态，并且返回。
    /// PS：权重的个数可以与子节点列表的个数不同。权重个数不足的将以1填充，超过的部分会被截断
    /// </summary>

    [BehaviorNode("Random", BehaviorNodeType.Composite)]
    public class RandomNodeProxy : NodeCsProxy
    {
        BaseCompositeNode CompositeNode = null;
        public override void OnAwake()
        {
            CompositeNode = Node as BaseCompositeNode;
        }

        public override void OnEnter()
        {
            CompositeNode.RunningNodeIndex = UnityEngine.Random.Range(0, CompositeNode.Children.Count);
        }

        public override void OnUpdate(float deltaTime)
        {
            BaseNode runningNode = CompositeNode.Children[CompositeNode.RunningNodeIndex];

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
                        CompositeNode.Status = NodeStatus.FAILED;
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
