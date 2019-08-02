
namespace BehaviorTree
{
    public enum SUCCESS_POLICY
    {
        SUCCEED_ON_ONE = 1,//当某一个节点返回成功时退出；
        SUCCEED_ON_ALL//当全部节点都返回成功时退出；
    }

    public enum FAILURE_POLICY
    {
        FAIL_ON_ONE = 1,//当某一个节点返回失败时退出；
        FAIL_ON_ALL //当全部节点都返回失败时退出；
    }
    public enum CompareDataSource
    {
        Agent = 1,//
        Global //
    }

    public enum CompareSymbol
    {
        Less = 0,//小于 <
        Greater, //大于 >
        LEqual, //小于等于 <=
        GEqual,//大于等于 >=
        Equal,//大于等于 ==
        NotEqual,//不等于 ！=
    }

    public class BehaviorTreeConst
    {
    }
}
