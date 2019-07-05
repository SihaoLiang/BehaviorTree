using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTreeData
{
    public static class DataUtility {

        /// <summary>
        /// 复制AgentData
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static AgentData Clone(this AgentData source)
        {
            if (source == null)
            {
                Debug.LogError("The Object you want to clone is null");
                return null;
            }

            AgentData agentData = new AgentData();
            agentData.ID = source.ID;

            List<BaseField> fields = source.Fields;
            if (fields != null && fields.Count > 0)
            {
                agentData.Fields = new List<BaseField>(fields.Count);
                for (int index = 0; index < fields.Count; index++)
                {
                    agentData.Fields.Add(fields[index].Clone());
                }
            }

            if (source.StartNode != null )
                agentData.StartNode = source.StartNode.Clone();
            
            return agentData;
        }

        /// <summary>
        /// 克隆NodeData
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static NodeData Clone(this NodeData source)
        {

            if (source == null)
            {
                Debug.LogError("The Object you want to clone is null");
                return null;
            }

            NodeData nodeData = new NodeData();

            nodeData.ID = source.ID;
            nodeData.ClassType = source.ClassType;
            nodeData.ClassName = source.ClassName;

            List<BaseField> fileds = source.Fileds;
            if (fileds != null && fileds.Count > 0)
            {
                nodeData.Fileds = new List<BaseField>(fileds.Count);
                for (int index = 0; index < fileds.Count; index++)
                {
                    nodeData.Fileds.Add(fileds[index].Clone());
                }
            }

            List<NodeData> childs = source.Childs;

            if (childs != null && childs.Count > 0)
            {
                nodeData.Childs = new List<NodeData>(childs.Count);
                for (int index = 0; index < childs.Count; index++)
                {
                    nodeData.Childs.Add(childs[index].Clone());
                }
            }

            return nodeData;
        }
       
    }

    
}