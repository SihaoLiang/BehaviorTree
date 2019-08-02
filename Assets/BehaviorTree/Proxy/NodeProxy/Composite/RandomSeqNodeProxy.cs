using System.Collections.Generic;
namespace BehaviorTree
{
    /// <summary>
    /// 随机序列
    /// </summary>
    [NodeProxy("RandomSequence", BehaviorNodeType.Composite)]
    public class RandomSeqNodeProxy : NodeCsProxy
    {
        protected BaseCompositeNode CompositeNode = null;
        protected List<BaseNode> Children = new List<BaseNode>();

        public override void OnAwake()
        {
            CompositeNode = Node as BaseCompositeNode;
            for (int index = 0; index < CompositeNode.Children.Count; index++)
            {
                Children.Add(CompositeNode.Children[index]);
            }
        }

        public override void OnDestroy()
        {
            CompositeNode = null;
            base.OnDestroy();
        }

        public override void OnEnter()
        {
            RandomChildren();
        }

        /// <summary>
        /// 打乱子节点
        /// </summary>
        public void RandomChildren()
        {
            if (Children == null || Children.Count <= 0)
                return;

            BaseNode baseNode = null;
            int count = Children.Count;

            for (int index = 0; index < count; index++)
            {
                int randIndex = UnityEngine.Random.Range(index, count);
                baseNode = Children[randIndex];
                Children[randIndex] = Children[index];
                Children[index] = baseNode;
            }

            baseNode = null;
        }

        public override void OnUpdate(float deltaTime)
        {
            BaseNode runningNode = Children[CompositeNode.RunningNodeIndex];

            runningNode.OnUpdate(deltaTime);

            switch (runningNode.Status)
            {
                case NodeStatus.SUCCESS:
                    {
                        CompositeNode.RunningNodeIndex++;

                        if (CompositeNode.RunningNodeIndex >= Children.Count)
                        {
                            CompositeNode.Status = NodeStatus.SUCCESS;
                            break;
                        }
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