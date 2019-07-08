using BehaviorTreeData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    /// <summary>
    /// 对比Int
    /// </summary>
    [BehaviorNode("CompareInt",BehaviorNodeType.Condition)]
    public class CompareIntNodeProxy : CompareNodeProxy
    {
        protected int LeftField;
        protected int RightField;
        public override void OnEnable()
        {
            LeftField = this.Node.NodeAgent.GetVarDicByKey(LeftParameter) as IntField;
            RightField = this.Node.NodeAgent.GetVarDicByKey(RightParameter) as IntField;
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
                case CompareSymbol.GEqual:
                    result = LeftField >= RightField;
                    break;
                case CompareSymbol.LEqual:
                    result = LeftField <= RightField;
                    break;
                case CompareSymbol.Less:
                    result = LeftField < RightField;
                    break;
                case CompareSymbol.Greater:
                    result = LeftField > RightField;
                    break;
            }

            if (result)
                Node.Status = NodeStatus.SUCCESS;
            else
                Node.Status = NodeStatus.FAILED;
        }
    }
}