using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// ��˳��ڵ����ƣ�����ʱ��Ҫ����һ���ڵ��б�
    /// �����е�����ڵ�ʱ�����Ľڵ��һ����һ�������С�
    /// ��������ӽڵ���SUCCESS����ô���Ὣ�����ʶ��ΪSUCCESS����ֱ�ӷ��أ�
    /// ��������ӽڵ�״̬��RUNNING����ô���Ὣ����Ҳ��ʶ��RUNNING�����ҵȴ��ڵ㷵�����������
    /// ��������ӽڵ�״̬��FAILED����ô����������һ����
    /// �κ�һ���ڵ㶼û�з���SUCCESS������£������Ὣ�����ʶ��ΪFAILED���ҷ���
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
                        //��������ʧ�ܽ�����ʧ��
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
