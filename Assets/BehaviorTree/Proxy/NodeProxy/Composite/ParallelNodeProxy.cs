using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// 创建这个节点的时候需要传入一个节点队列。一个接一个的运行子节点。
    /// 失败条件：FAIL_ON_ONE
    /// 那么只要有一个子节点的状态是FAILED，那么它会将自己标识为FAILED并且直接返回；
    /// 失败条件：FAIL_ON_ALL
    /// 那么只要全部子节点状态是FAILED，那么它会将自己标识为FAILED并且直接返回；
    /// 
    /// 成功条件:SUCCEED_ON_ONE
    /// 如果其中一个子节点的状态是SUCCESS它会将自己的标识为SUCCESS并且返回，否则他会将自己标识为RUNNING。
    /// 成功条件:SUCCEED_ON_ALL
    /// 如果子节点的状态是SUCCESS或者RUNNING，那么它会运行下一个节点。只有所有的节点都标识为SUCCESS它会将自己的标识为SUCCESS并且返回，
    /// 否则他会将自己标识为RUNNING。
    /// </summary>

    [NodeProxy("Parallel", BehaviorNodeType.Composite)]
    public class ParallelNodeProxy : NodeCsProxy
    {
        int FailPolicy = -1;
        int SucceedPolicy = -1;

        public override void OnAwake()
        {
            EnumField enumFieldFailPolicy = Node.Fields["FailType"] as EnumField;
            FailPolicy = enumFieldFailPolicy.Value;

            EnumField enumFieldSucceedPolicy = Node.Fields["SuccessType"] as EnumField;
            SucceedPolicy = enumFieldSucceedPolicy.Value;
        }


        public override void OnUpdate(float deltaTime)
        {
            int failCount = 0;
            int successCound = 0;

            BaseCompositeNode compositeNode = Node as BaseCompositeNode;

            for (int i = 0; i < compositeNode.Children.Count; i++)
            {
                BaseNode node = compositeNode.Children[i];

                node.OnUpdate(deltaTime);

                if (node.Status == NodeStatus.FAILED)
                {
                    failCount++;

                    if (FailPolicy == (int)FAILURE_POLICY.FAIL_ON_ONE)
                    {
                        Node.Status = NodeStatus.FAILED;
                        break;
                    }
                    else if (FailPolicy == (int)FAILURE_POLICY.FAIL_ON_ALL && failCount == compositeNode.Children.Count)
                    {
                        Node.Status = NodeStatus.FAILED;
                        break;
                    }

                }
                else if (node.Status == NodeStatus.SUCCESS)
                {
                    successCound++;

                    if (SucceedPolicy == (int)SUCCESS_POLICY.SUCCEED_ON_ONE)
                    {
                        Node.Status = NodeStatus.SUCCESS;
                        break;
                    }
                    else if (SucceedPolicy == (int)SUCCESS_POLICY.SUCCEED_ON_ALL && successCound == compositeNode.Children.Count)
                    {
                        Node.Status = NodeStatus.SUCCESS;
                        break;
                    }
                }
                else if (node.Status == NodeStatus.ERROR)
                {
                    Node.Status = NodeStatus.ERROR;
                    break;
                }
            }

            if ((failCount + successCound) == compositeNode.Children.Count)
                Node.Status = NodeStatus.SUCCESS;

        }
    }
}
