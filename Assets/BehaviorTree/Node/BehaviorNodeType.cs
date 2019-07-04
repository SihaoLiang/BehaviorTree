using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum BehaviorNodeType
    {
        None,
        Inherit,//继承节点
        Composite,//组合节点
        Decorator,//修饰节点
        Condition,//条件节点
        Action,//叶节点
    }
}
