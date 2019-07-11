using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// ��������ڵ�ʱ����Ҫ����һ���ڵ���С�
    /// �����е�����ڵ�ʱ�������ӽڵ��һ����һ�������С�
    /// ��������ӽڵ�״̬��SUCCESS����ô����������һ����
    /// ��������ӽڵ�״̬��RUNNING����ô���Ὣ����Ҳ��ʶ��RUNNING��
    /// ���ҵȴ��ڵ㷵�������������������ӽڵ�״̬��FAILED��
    /// ��ô������Լ���״̬��ʶΪFAILED����ֱ�ӷ��ء�
    /// ���нڵ㶼���ؽ�βΪSUCCESS��ô���Ὣ�����ʶ��ΪSUCCESS���ҷ��ء�
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
