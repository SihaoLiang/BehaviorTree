
namespace BehaviorTree
{
    public class NodeCsProxy : NodeProxy
    {
        public NodeCsProxy()
        {
            Events = OnGetEvents();
        }

        public override void OnAwake()
        {

        }

        public override void OnEnable()
        {

        }

        public override void OnDisable()
        {

        }

        public override void OnDestroy()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnEnter()
        {

        }

        public override void OnFixedUpdate(float deltaTime)
        {

        }

        public override string[] OnGetEvents()
        {
            return null;
        }

        public override void OnNotify(string evt, params object[] args)
        {

        }

        public override void OnReset()
        {


        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void SetNode(BaseNode baseNode)
        {
            this.Node = baseNode;
            this.NodeProxyInfo = baseNode.NodeInfo;
        }

        public override void SetAgent()
        {

        }

        public override void OnRecycle()
        {

        }
    }
}