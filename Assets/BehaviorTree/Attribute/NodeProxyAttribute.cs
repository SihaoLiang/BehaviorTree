using System;
namespace BehaviorTree
{
    public class NodeProxyAttribute : Attribute
    {

        public string ClassType;
        public BehaviorNodeType NodeType;
        public bool NeedUpdate;

        public NodeProxyAttribute(string classType, BehaviorNodeType nodeType, bool needUpdate = true)
        {
            ClassType = classType;
            NodeType = nodeType;
            NeedUpdate = needUpdate;
        }
    }
}