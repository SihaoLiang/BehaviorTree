using UnityEngine;

namespace BehaviorTreeDebuger
{
    public static class XFsmContent
    {
        public static GUIContent centerButtonStr;
        public static GUIContent refreshButtonStr;
        public static GUIContent behaviorTreeButtonStr;

        static XFsmContent()
        {
            centerButtonStr = new GUIContent("居中");
            refreshButtonStr = new GUIContent("刷新");
            behaviorTreeButtonStr = new GUIContent("行为树");
        }
    }
}