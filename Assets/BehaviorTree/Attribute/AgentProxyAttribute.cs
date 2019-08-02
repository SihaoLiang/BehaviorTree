using System;

namespace BehaviorTree
{
    public class AgentProxyAttribute : Attribute
    {
        public string AgentName;

        public AgentProxyAttribute(string agentName)
        {
            AgentName = agentName;
        }
    }
}