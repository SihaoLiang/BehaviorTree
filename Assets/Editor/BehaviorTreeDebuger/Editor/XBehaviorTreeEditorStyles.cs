﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using BehaviorTree;

namespace BehaviorTreeDebuger
{
    public static class XBehaviorTreeEditorStyles
    {
        public const float StateWidth = 170f;
        public const float StateHeight = 35f;

        public static GUIStyle canvasBackground;
        public static GUIStyle wrappedLabel;
        public static GUIStyle instructionLabel;

        public static Color gridMinorColor;
        public static Color gridMajorColor;

        static GUIStyle normalNodeStyle;
        static GUIStyle errorNodeStyle;
        static GUIStyle runningNodeStyle;
        static GUIStyle successedNodeStyle;
        static GUIStyle failedNodeStyle;

        static XBehaviorTreeEditorStyles()
        {
            nodeStyleCache = new Dictionary<string, GUIStyle>();
            gridMinorColor = EditorGUIUtility.isProSkin ? new Color(0f, 0f, 0f, 0.18f) : new Color(0f, 0f, 0f, 0.1f);
            gridMajorColor = EditorGUIUtility.isProSkin ? new Color(0f, 0f, 0f, 0.28f) : new Color(0f, 0f, 0f, 0.15f);

            canvasBackground = "flow background";

            wrappedLabel = new GUIStyle("label")
            {
                fixedHeight = 0,
                wordWrap = true
            };

            instructionLabel = new GUIStyle("TL Selection H2")
            {
                padding = new RectOffset(3, 3, 3, 3),
                contentOffset = wrappedLabel.contentOffset,
                alignment = TextAnchor.UpperLeft,
                fixedHeight = 0,
                wordWrap = true
            };

            normalNodeStyle = GetNodeStyle(2, false, false);
            errorNodeStyle = GetNodeStyle(6, true, false);
            runningNodeStyle = GetNodeStyle(1, true, false);
            successedNodeStyle = GetNodeStyle(4, true, false);
            failedNodeStyle = GetNodeStyle(0, true, false);
        }

        private static Dictionary<string, GUIStyle> nodeStyleCache;

        private static string[] styleCache =
        {
            "flow node 0",
            "flow node 1",
            "flow node 2",
            "flow node 3",
            "flow node 4",
            "flow node 5",
            "flow node 6"
        };

        private static string[] styleCacheHex =
        {
            "flow node hex 0",
            "flow node hex 1",
            "flow node hex 2",
            "flow node hex 3",
            "flow node hex 4",
            "flow node hex 5",
            "flow node hex 6"
        };

        public static GUIStyle GetNodeStyle(int color, bool on, bool hex)
        {
            return GetNodeStyle(hex ? styleCacheHex[color] : styleCache[color], on, hex ? 8f : 2f);
        }

        public static GUIStyle GetNodeStyle(NodeDesigner node)
        {
            GUIStyle guiStyle = normalNodeStyle;

            if (node.baseNode.Status == NodeStatus.READY)
                guiStyle = normalNodeStyle;
            else if (node.baseNode.Status == NodeStatus.RUNNING)
                guiStyle = runningNodeStyle;
            else if (node.baseNode.Status == NodeStatus.FAILED)
                guiStyle = failedNodeStyle;
            else if (node.baseNode.Status == NodeStatus.SUCCESS)
                guiStyle = successedNodeStyle;
            else if (node.baseNode.Status == NodeStatus.ERROR)
                guiStyle = errorNodeStyle;

            return guiStyle;
        }

        private static GUIStyle GetNodeStyle(string styleName, bool on, float offset)
        {
            string str = on ? string.Concat(styleName, " on") : styleName;
            if (!nodeStyleCache.ContainsKey(str))
            {
                GUIStyle style = new GUIStyle(str);
                style.contentOffset = new Vector2(0, style.contentOffset.y - offset);
                if (on)
                {
                    style.fontStyle = FontStyle.Bold;
                }
                nodeStyleCache[str] = style;
            }
            return nodeStyleCache[str];
        }

        public static Color GetTransition(NodeDesigner node)
        {
            Color color = Color.white;

            if (node.baseNode.Status == NodeStatus.READY)
                color = Color.white;
            else if (node.baseNode.Status == NodeStatus.RUNNING)
                color = Color.cyan;
            else if (node.baseNode.Status == NodeStatus.FAILED)
                color = Color.gray;
            else if (node.baseNode.Status == NodeStatus.SUCCESS)
                color = Color.yellow;
            else if (node.baseNode.Status == NodeStatus.ERROR)
                color = Color.red;

            return color;
        }
    }
}