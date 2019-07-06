using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BehaviorTreePool : MonoBehaviour
    {
        Dictionary<string, List<BehaviorTree>> Pools = new Dictionary<string,List<BehaviorTree>>();

        /// <summary>
        /// 从池中获取行为树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BehaviorTree Spawn(string id)
        {
            BehaviorTree behaviorTree = null;

            if (string.IsNullOrEmpty(id))
                return behaviorTree;

            if (!Pools.ContainsKey(id))
                return behaviorTree;

            List<BehaviorTree> behaviorTrees = Pools[id];

            if (behaviorTrees.Count > 0)
                behaviorTree = behaviorTrees[0];

            return behaviorTree;
        }

        /// <summary>
        /// 行为树入池
        /// </summary>
        /// <param name="behaviorTree"></param>
        public void DeSpawn(BehaviorTree behaviorTree)
        {
            string id = behaviorTree.BehaviorTreeId;
            if (!Pools.ContainsKey(id))
                Pools.Add(id, new List<BehaviorTree>());

            behaviorTree.BehaviorAgent = null;

            Pools[id].Add(behaviorTree);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (Pools.Count <= 0)
                return;

            foreach (var keyValue in Pools)
            {
                for (int index = 0; index < keyValue.Value.Count; index++)
                {
                    keyValue.Value[index].OnDestroy();
                }
            }
            Pools.Clear();
        }

        public void OnDestroy()
        {
            Clear();
        }
    }
}

