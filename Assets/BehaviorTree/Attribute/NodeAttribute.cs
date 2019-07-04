using System;
namespace BehaviorTree
{
    public class BehaviorNodeAttribute : Attribute
    {

        public string ClassType;
        public BehaviorNodeType NodeType;

        public BehaviorNodeAttribute(string classType, BehaviorNodeType nodeType)
        {
            ClassType = classType;
            NodeType = nodeType;
        }
    }
}