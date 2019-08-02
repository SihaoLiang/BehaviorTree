using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// CsAgent代理
    /// </summary>
    public class AgentCsProxy : AgentProxy
    {
        public override void SetAgent(Agent agent, string classType)
        {
            BTAgent = agent;
        }

        public override List<string> OnGetEvents()
        {
            return Events;
        }
    }
}