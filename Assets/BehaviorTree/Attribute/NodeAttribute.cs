using System;
namespace BehaviorTree
{
    public class BehaviorNodeAttribute : Attribute
    {

        public string ClassType;
        public BehaviorNodeType NodeType;
        public bool NeedUpdate;

        public BehaviorNodeAttribute(string classType, BehaviorNodeType nodeType,bool needUpdate = true)
        {
            ClassType = classType;
            NodeType = nodeType;
            NeedUpdate = needUpdate;
        }
    }
}