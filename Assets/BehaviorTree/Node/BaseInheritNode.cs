using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BaseInheritNode : BaseNode
    {

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node"></param>
        public virtual void AddNode(BaseNode node)
        {

        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        public virtual void RemoveNode(BaseNode node)
        {

        }

        /// <summary>
        /// 判断是否有节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual bool HasNode(BaseNode node)
        {
            return false;
        }
    }
}
