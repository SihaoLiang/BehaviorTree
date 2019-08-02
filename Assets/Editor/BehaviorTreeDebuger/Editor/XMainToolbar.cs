using UnityEngine;
using UnityEditor;
using BehaviorTree;

namespace BehaviorTreeDebuger
{
    [System.Serializable]
    public class XMainToolbar
    {
        public XMainToolbar()
        {
        }

        public void OnEnable()
        {
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button(XFsmContent.behaviorTreeButtonStr, EditorStyles.toolbarDropDown, GUILayout.Width(80)))
            {
                Agent[] agents = Object.FindObjectsOfType<Agent>();
                if (agents.Length == 0)
                    return;

                GenericMenu menu = new GenericMenu();

                for (int i = 0; i < agents.Length; i++)
                {
                    Agent agent = agents[i];
                    menu.AddItem(new GUIContent(agent.BTree.Id), false, delegate ()
                    {
                        XBehaviorTreeEditor.Instance.SetAgent(agent);
                    });
                }

                menu.ShowAsContext();
            }


            GUILayout.FlexibleSpace();


            if (GUILayout.Button(XFsmContent.refreshButtonStr, EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                Agent[] agents = Object.FindObjectsOfType<Agent>();
                if (agents.Length == 0)
                    return;

                XBehaviorTreeEditor.Instance.SetAgent(agents[0]);
            }

            GUILayout.Space(10);

            if (GUILayout.Button(XFsmContent.centerButtonStr, EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                XBehaviorTreeEditor.Instance.CenterView();
            }
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }
    }
}