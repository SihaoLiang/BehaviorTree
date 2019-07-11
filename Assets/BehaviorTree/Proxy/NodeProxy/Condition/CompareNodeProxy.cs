using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
namespace BehaviorTree
{
    [BehaviorNode("Compare", BehaviorNodeType.Condition)]
    public class CompareNodeProxy : NodeCsProxy
    {
        protected CompareDataSource LeftType = CompareDataSource.Agent;
        protected CompareDataSource RightType = CompareDataSource.Agent;
        protected string LeftParameter;
        protected string RightParameter;
        protected CompareSymbol CompareType;


        public override void OnAwake()
        {
            if (Node.Fields == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            if (Node.Fields["LeftType"] == null || Node.Fields["CompareType"] == null ||Node.Fields["RightType"] == null || Node.Fields["LeftParameter"] == null || Node.Fields["RightParameter"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }
            EnumField LeftTypeField = Node.Fields["LeftType"] as EnumField;
            EnumField RightTypeField = Node.Fields["RightType"] as EnumField;
            EnumField CompareSymbolField = Node.Fields["CompareType"] as EnumField;
            LeftType = (CompareDataSource)LeftTypeField.Value;
            RightType = (CompareDataSource)RightTypeField.Value;
            CompareType = (CompareSymbol)CompareSymbolField.Value;

            LeftParameter = Node.Fields["LeftParameter"] as StringField;
            RightParameter = Node.Fields["RightParameter"] as StringField;
        }
    }
}