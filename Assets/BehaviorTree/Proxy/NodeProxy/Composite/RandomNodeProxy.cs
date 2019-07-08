using System;
using System.Collections.Generic;


namespace BehaviorTree
{
    /// <summary>
    /// ��������ڵ��ʱ����Ҫ����һ���ڵ���кͿ��Բ������б��Ӧ��Ȩ�ء�
    /// ��û��Ȩ�ص�����£����е��ӽڵ���ȫ���������Ȩ�ص�����£�
    /// �ӽڵ�ᰴ��Ȩ�ص�����ֵ��ѡ��ĳһ���ڵ������У����ҽ�����ڵ�����н����ʶΪ�Լ���״̬�����ҷ��ء�
    /// PS��Ȩ�صĸ����������ӽڵ��б�ĸ�����ͬ��Ȩ�ظ�������Ľ���1��䣬�����Ĳ��ֻᱻ�ض�
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
