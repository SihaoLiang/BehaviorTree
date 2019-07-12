using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    /// <summary>
    /// 行为树事件中转
    /// </summary>
    public class BehaviorTreeEventManager : XSingleton<BehaviorTreeEventManager>
    {
        public void OnBehaviorTreeEnable(string treeId)
        {

        }
        public void OnBehaviorTreeBegin(string treeId)
        {
        }

        public void OnBehaviorTreeEnd(string treeId)
        {
        }
        public void OnBehaviorTreeDiable(string treeId)
        {

        }

        public void OnNodeError(string treeId, int nodeId, string nodeType ,bool IsLua)
        {
            Debug.LogError($"行为节点报错 TreeId:{treeId},nodeId:{nodeId},nodeType:{nodeType},IsLua:{IsLua}");
        }

        public void OnNodeStateChange(string treeId, int nodeId, string nodeType,NodeStatus nodeStatus,BehaviorNodeType behaviorNodeType,bool IsLua)
        {
            if(behaviorNodeType == BehaviorNodeType.Action && nodeStatus == NodeStatus.RUNNING)
                Debug.LogError($"行为节点状态修改 TreeId:{treeId},nodeId:{nodeId},nodeType:{nodeType},NodeStatus:{nodeStatus},IsLua:{IsLua}");
        }

        public void OnNodeEnter(string treeId,int nodeId,string nodeType,bool IsLua)
        {
            //Debug.Log($"行为节点进入 TreeId:{treeId},nodeId:{nodeId},nodeType:{nodeType},IsLua:{IsLua}");
        }

        public void OnNodeExit(string treeId, int nodeId, string nodeType, bool IsLua)
        {
            //Debug.Log($"行为节点退出 TreeId:{treeId},nodeId:{nodeId},nodeType:{nodeType},IsLua:{IsLua}");
        }

    }
}