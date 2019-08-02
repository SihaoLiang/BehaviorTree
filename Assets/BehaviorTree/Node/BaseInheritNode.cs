
namespace BehaviorTree
{
    public abstract class BaseInheritNode : BaseNode
    {
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node"></param>
        public abstract void AddNode(BaseNode node);

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        public abstract void RemoveNode(BaseNode node);

        /// <summary>
        /// 判断是否有节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract bool HasNode(BaseNode node);
      
    }
}
