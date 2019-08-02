
namespace BehaviorTree
{
    /// <summary>
    /// 节点信息
    /// </summary>
    public class NodeProxyInfo
    {
        public string ClassType; //代理器名称
        public BehaviorNodeType behaviorNodeType; //节点类型
        public bool IsLua; //是否是lua代理
        public bool NeedUpdate = true; //是否需要调度
    }
}