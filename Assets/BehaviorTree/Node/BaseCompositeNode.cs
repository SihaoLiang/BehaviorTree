using System.Collections.Generic;
namespace BehaviorTree
{
    public class BaseCompositeNode : BaseInheritNode
    {
        /// <summary>
        /// 所有子节点
        /// </summary>
        public List<BaseNode> Children = new List<BaseNode>();

        /// <summary>
        /// 当前运行中的节点索引
        /// </summary>
        public int RunningNodeIndex = 0;


        public override void OnAwake()
        {
            if (Children == null || Children.Count <= 0)
                Status = NodeStatus.ERROR;

            base.OnAwake();
        }

        public override void OnEnter()
        {
            RunningNodeIndex = 0;
            base.OnEnter();
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node"></param>
        public override void AddNode(BaseNode node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        public override void RemoveNode(BaseNode node)
        {
            node.Parent = null;
            Children.Remove(node);
        }

        /// <summary>
        /// 判断是否有节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool HasNode(BaseNode node)
        {
            return Children.Contains(node);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public override void OnReset()
        {
            base.OnReset();
            RunningNodeIndex = 0;

            for (int i = 0; i < Children.Count; i++)
            {
                var node = Children[i];
                if (node != null)
                    node.OnReset();
            }

        }
    }
}
