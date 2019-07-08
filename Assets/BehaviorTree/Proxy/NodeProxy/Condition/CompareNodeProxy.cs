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
            if (Node.NodeDatas == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }

            if (Node.NodeDatas["LeftType"] == null || Node.NodeDatas["CompareType"] == null ||Node.NodeDatas["RightType"] == null || Node.NodeDatas["LeftParameter"] == null || Node.NodeDatas["RightParameter"] == null)
            {
                Node.Status = NodeStatus.ERROR;
                return;
            }
            EnumField LeftTypeField = Node.NodeDatas["LeftType"] as EnumField;
            EnumField RightTypeField = Node.NodeDatas["RightType"] as EnumField;
            EnumField CompareSymbolField = Node.NodeDatas["CompareType"] as EnumField;
            LeftType = (CompareDataSource)LeftTypeField.Value;
            RightType = (CompareDataSource)RightTypeField.Value;
            CompareType = (CompareSymbol)CompareSymbolField.Value;

            LeftParameter = Node.NodeDatas["LeftParameter"] as StringField;
            RightParameter = Node.NodeDatas["RightParameter"] as StringField;
        }
    }
}