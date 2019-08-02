using BehaviorTree;
using BehaviorTreeData;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace BehaviorTreeDebuger
{
    public class XBehaviorTreeEditor : XNodeEditor
    {
        public static XBehaviorTreeEditor Instance;

        private AgentDesigner Agent;

        public void SetAgent(Agent agent)
        {
            if (agent == null)
            {
                Agent = null;
                return;
            }

            Agent = new AgentDesigner();

            if (agent.BTree == null)
                return;

            titleContent = new GUIContent(agent.BTree.Id);

            //创建节点 
            for (int i = 0; i < agent.BTree.AllNodes.Count; i++)
            {
                BaseNode baseNode = agent.BTree.AllNodes[i];

                NodeDesigner nodeDesigner = new NodeDesigner();
                nodeDesigner.baseNode = baseNode;
                nodeDesigner.NodeData = baseNode.Fields;
                nodeDesigner.Rect = new Rect(nodeDesigner.NodeData.X, nodeDesigner.NodeData.Y, XBehaviorTreeEditorStyles.StateWidth, XBehaviorTreeEditorStyles.StateHeight);
                Agent.Nodes.Add(nodeDesigner);
            }

            //初始化Transition
            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    SetTransition(node, node.NodeData);
                }
                CenterView();
            }
        }

        public void SetTransition(NodeDesigner node, NodeData nodeData)
        {
            if (nodeData.Childs != null && nodeData.Childs.Count > 0)
            {
                node.Transitions = new List<Transition>(nodeData.Childs.Count);

                for (int i = 0; i < nodeData.Childs.Count; i++)
                {
                    NodeData tempData = nodeData.Childs[i];
                    Transition transition = new Transition();
                    transition.Set(FindById(tempData.ID), node);
                    node.Transitions.Add(transition);
                }
            }
        }

        public NodeDesigner FindById(int id)
        {
            if (Agent == null)
                return null;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner nodeDesigner = Agent.Nodes[i];
                if (nodeDesigner != null && nodeDesigner.ID == id)
                    return nodeDesigner;
            }

            return null;
        }

        private void OnSelectionChange()
        {
            GameObject gameObject = Selection.activeGameObject;
            if (gameObject != null)
            {
                Agent agent = gameObject.GetComponent<Agent>();
                if (agent != null)
                    SetAgent(agent);
            }
        }

        private bool centerView;
        private Rect propertyRect;
        private Rect preferencesRect;
        private Rect shortcutRect;
        private XMainToolbar mainToolbar;

        public static XBehaviorTreeEditor ShowWindow(Agent agent)
        {
            XBehaviorTreeEditor window = EditorWindow.GetWindow<XBehaviorTreeEditor>(agent.BTree.Id);
            window.SetAgent(agent);
            return window;
        }

        private void OnDestroy()
        {
            Selection.activeObject = null;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Instance = this;
            if (mainToolbar == null)
                mainToolbar = new XMainToolbar();
            mainToolbar.OnEnable();

            centerView = true;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
        }

        private void Update()
        {
            this.Repaint();

            if (Agent == null)
            {
                Agent agent = GameObject.FindObjectOfType<Agent>();
                if (agent != null)
                {
                    Selection.activeObject = agent;
                    //SetAgent(agent);
                }
            }
        }

        protected override void OnGUI()
        {
            GetCanvasSize();

            mainToolbar.OnGUI();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginHorizontal(GUILayout.Width(propertyRect.width));
                {
                    GUILayout.BeginVertical();
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                {

                    XZoomableArea.Begin(new Rect(propertyRect.width, 0f, scaledCanvasSize.width, scaledCanvasSize.height + 21), scale, IsDocked);
                    Begin();

                    if (Agent != null)
                    {
                        DoNodes();
                    }
                    else
                    {
                        XZoomableArea.End();
                    }
                    End();

                    preferencesRect.x -= propertyRect.width;
                    if (centerView)
                    {
                        CenterView();
                        centerView = false;
                    }

                    //GUI.Label(new Rect(5, 20, 300, 200), "Right click to create a node.", BehaviorTreeEditorStyles.instructionLabel);
                    Event ev = Event.current;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
        }

        protected override Rect GetCanvasSize()
        {
            shortcutRect = new Rect(canvasSize.width - 250, 17, 250, canvasSize.height - 17);
            return new Rect(0, 17, position.width - propertyRect.width, position.height);
        }

        private void DoNodes()
        {
            DoTransitions();
            DoChildIndex();

            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    DoNode(node, false);
                }
            }

            XZoomableArea.End();
        }

        private void DoNode(NodeDesigner node, bool on)
        {
            GUIStyle style = XBehaviorTreeEditorStyles.GetNodeStyle(node);
            GUI.Box(node.Rect, node.NodeData.ClassType + ":" + node.NodeData.ID, style);
        }

        private void DoTransitions()
        {
            if (Agent == null)
                return;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                for (int j = 0; j < node.Transitions.Count; j++)
                {
                    Transition transition = node.Transitions[j];
                    DoTransition(transition);
                }
            }
        }

        private void DoTransition(Transition trnansition)
        {
            NodeDesigner toNode = trnansition.ToNode;
            NodeDesigner fromNode = trnansition.FromNode;
            if (toNode != null && fromNode != null)
            {
                Color color = XBehaviorTreeEditorStyles.GetTransition(toNode);
                DrawConnection(fromNode.Rect.center, toNode.Rect.center, color, 1, false);
            }
        }

        private void DoChildIndex()
        {
            if (Agent == null)
                return;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];

                if (node.Transitions.Count > 1)
                {
                    for (int j = 0; j < node.Transitions.Count; j++)
                    {
                        Transition transition = node.Transitions[j];
                        Vector3 start = transition.FromNode.Rect.center;
                        Vector3 end = transition.ToNode.Rect.center;

                        Vector3 vector3 = (end + start) * 0.5f;
                        GUI.Label(new Rect(vector3.x, vector3.y, 0, 0), j.ToString(), XBehaviorTreeEditorStyles.instructionLabel);
                    }
                }

            }
        }

        protected override void CanvasContextMenu()
        {
            if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1)
            {
                return;
            }

            if (Agent == null)
                return;

            GenericMenu canvasMenu = new GenericMenu();
            canvasMenu.ShowAsContext();
        }

        private NodeDesigner MouseOverNode()
        {
            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                if (node.Rect.Contains(mousePosition))
                {
                    return node;
                }
            }
            return null;
        }

        private void UpdateUnitySelection()
        {
            //Selection.objects = selection1.ToArray();
        }

        public void Fresh()
        {
        }

        public void CenterView()
        {
            if (Agent == null)
                return;

            Vector3 center = Vector3.zero;
            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    center += new Vector3(node.Rect.center.x - scaledCanvasSize.width * 0.5f, node.Rect.center.y - scaledCanvasSize.height * 0.5f);
                }
                center /= Agent.Nodes.Count;
            }
            else
            {
                center = XNodeEditor.Center;
            }
            UpdateScrollPosition(center);
            Repaint();
        }

        public static void RepaintAll()
        {
            if (Instance != null)
            {
                Instance.Repaint();
            }
        }

        public bool IsDocked
        {
            get
            {
                BindingFlags fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                MethodInfo isDockedMethod = typeof(EditorWindow).GetProperty("docked", fullBinding).GetGetMethod(true);
                return (bool)isDockedMethod.Invoke(this, null);
            }
        }
    }
}