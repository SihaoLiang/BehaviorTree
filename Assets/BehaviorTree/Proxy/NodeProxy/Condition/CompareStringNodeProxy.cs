using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;

namespace BehaviorTree
{
    /// <summary>
    /// 对比String
    /// </summary>
    [BehaviorNode("CompareString", BehaviorNodeType.Condition)]
    public class CompareStringNodeProxy : CompareNodeProxy
    {
        protected string LeftField;
        protected string RightField;
        public override void OnEnable()
        {
            LeftField = this.Node.NodeAgent.GetVarDicByKey(LeftParameter) as StringField;
            RightField = this.Node.NodeAgent.GetVarDicByKey(RightParameter) as StringField;
        }

        public override void OnUpdate(float deltaTime)
        {
            bool result = false;
            switch (CompareType)
            {
                case CompareSymbol.Equal:
                    result = LeftField == RightField;
                    break;
                case CompareSymbol.NotEqual:
                    result = LeftField != RightField;
                    break;
            }

            if (result)
                Node.Status = NodeStatus.SUCCESS;
            else
                Node.Status = NodeStatus.FAILED;
        }
    }
}