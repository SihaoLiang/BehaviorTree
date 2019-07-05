using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SUCCESS_POLICY
{
    SUCCEED_ON_ONE = 0,//当某一个节点返回成功时退出；
    SUCCEED_ON_ALL//当全部节点都返回成功时退出；
}

public enum FAILURE_POLICY
{
    FAIL_ON_ONE = 0,//当某一个节点返回失败时退出；
    FAIL_ON_ALL //当全部节点都返回失败时退出；
}

public class BehaviorTreeConst {



}
