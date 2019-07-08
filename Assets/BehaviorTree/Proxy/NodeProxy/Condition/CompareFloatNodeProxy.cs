using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeData;
namespace BehaviorTree
{
    /// <summary>
    /// 对比Float
    /// </summary>
    [BehaviorNode("CompareFloat", BehaviorNodeType.Condition)]
    public class CompareFloatNodeProxy : CompareNodeProxy
    {
        protected float LeftField;
        protected float RightField;

        public override void OnEnter()
        {
            LeftField = this.Node.NodeAgent.GetVarDicByKey(LeftParameter) as FloatField;
            RightField = this.Node.NodeAgent.GetVarDicByKey(RightParameter) as FloatField;
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