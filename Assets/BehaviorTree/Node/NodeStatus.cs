namespace BehaviorTree
{
    public enum NodeStatus
    {
        READY,//READY：准备状态，节点还没有被调用过。或者已经调用结束被Reset之后的状态
        RUNNING,//正在运行的状态，通常父节点会等待子节点Runing结束才会将自己的状态标示为结束，当然部分节点不会理会子节点的Runing状态
        SUCCESS,//运行成功
        FAILED,//运行失败
        ERROR,//运行出错
    }
}
