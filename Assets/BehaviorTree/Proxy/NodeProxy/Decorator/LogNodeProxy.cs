using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
namespace BehaviorTree
{
    /// <summary>
    /// 输出节点
    /// </summary>
    [BehaviorNode("Log",BehaviorNodeType.Decorator)]
    public class LogNodeProxy : NodeCsProxy
    {
        string LogContent = string.Empty;
        public override void OnAwake()
        {
            if (Node.NodeDatas == null || Node.NodeDatas["Content"] == null)
                return;

            // Node.NodeDatas["Content"].
        }
        public override void OnUpdate(float deltaTime)
        {
            BaseDecoratorNode decoratorNode = Node as BaseDecoratorNode;
  
            decoratorNode.ChildNode.OnUpdate(deltaTime);

            if (decoratorNode.ChildNode.Status == NodeStatus.ERROR)
            {
                decoratorNode.Status = NodeStatus.ERROR;
                return;
            }

            if (decoratorNode.ChildNode.Status == NodeStatus.FAILED || decoratorNode.ChildNode.Status == NodeStatus.SUCCESS)
            {
                Debug.Log(LogContent);
                decoratorNode.Status = NodeStatus.SUCCESS;
            }
        }
    }
}
