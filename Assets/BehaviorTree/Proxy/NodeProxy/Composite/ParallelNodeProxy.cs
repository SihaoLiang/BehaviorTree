using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// ��������ڵ��ʱ����Ҫ����һ���ڵ���С�һ����һ���������ӽڵ㡣
    /// ʧ��������FAIL_ON_ONE
    /// ��ôֻҪ��һ���ӽڵ��״̬��FAILED����ô���Ὣ�Լ���ʶΪFAILED����ֱ�ӷ��أ�
    /// ʧ��������FAIL_ON_ALL
    /// ��ôֻҪȫ���ӽڵ�״̬��FAILED����ô���Ὣ�Լ���ʶΪFAILED����ֱ�ӷ��أ�
    /// 
    /// �ɹ�����:SUCCEED_ON_ONE
    /// �������һ���ӽڵ��״̬��SUCCESS���Ὣ�Լ��ı�ʶΪSUCCESS���ҷ��أ��������Ὣ�Լ���ʶΪRUNNING��
    /// �ɹ�����:SUCCEED_ON_ALL
    /// ����ӽڵ��״̬��SUCCESS����RUNNING����ô����������һ���ڵ㡣ֻ�����еĽڵ㶼��ʶΪSUCCESS���Ὣ�Լ��ı�ʶΪSUCCESS���ҷ��أ�
    /// �������Ὣ�Լ���ʶΪRUNNING��
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
