using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;

namespace BehaviorTree
{
    [BehaviorNode("RateSelector",BehaviorNodeType.Composite)]
    public class RateSelectorNodeProxy : NodeCsProxy
    {
        List<int> PriorityList;
        protected BaseCompositeNode CompositeNode = null;
        protected List<BaseNode> Children = new List<BaseNode>();
        protected Dictionary<BaseNode, int> PriorityIndex = new Dictionary<BaseNode, int>();
        List<BaseNode> RandList = new List<BaseNode>();

        public override void OnAwake()
        {
            if (Node.NodeDatas == null || Node.NodeDatas["Priority"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            CompositeNode = Node as BaseCompositeNode;
            for (int index = 0; index < CompositeNode.Children.Count; index++)
            {
                Children.Add(CompositeNode.Children[index]);
            }

            RandList.Clear();

            PriorityList = new List<int>();
            RepeatIntField repeatIntField = Node.NodeDatas["Priority"] as RepeatIntField;
            List<int> tempList = repeatIntField.Value;

            for (int index = 0; index < tempList.Count; index++)
            {
                PriorityList.Add(tempList[index]);
            }
        }
        public override void OnEnable()
        {
            base.OnEnable();
            RateSortChildren();
        }

        /// <summary>
        /// 概率
        /// </summary>
        public void RateSortChildren()
        {
            if (Children == null || Children.Count <= 0)
                return;

            //先计算权重总和
            int prioritySum = 0;
            for (int index = 0; index < PriorityList.Count; index++)
            {
                prioritySum += PriorityList[index];
            }

            PriorityIndex.Clear();
            RandList.Clear();

            //遍历所有权重
            for (int index = 0; index < PriorityList.Count; index++)
            {
                //从 0 到最大权重随出一个随机数
                int randIndex = UnityEngine.Random.Range(0, prioritySum);
                //随机数 + 节点权重值 = 本次权重值
                int priority = randIndex + PriorityList[index];
                int pos = 0;

                //插入排序
                if (RandList.Count == 0)
                {
                    //插入第一个节点
                    RandList.Add(Children[index]);
                    //记录该节点权重
                    PriorityIndex.Add(Children[index], priority);
                }
                else
                {
                    for (int i = 0; i < RandList.Count; i++)
                    {
                        //最大的一端开始向下遍历，插入到第一个小于自己权重节点的位置
                        pos = i;
                        if (priority > PriorityIndex[RandList[i]])
                            break;
                        pos++;
                    }
                    //插入节点
                    RandList.Insert(pos, Children[index]);
                    //记录
                    PriorityIndex.Add(Children[index], priority);
                }
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            BaseNode runningNode = RandList[CompositeNode.RunningNodeIndex];

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