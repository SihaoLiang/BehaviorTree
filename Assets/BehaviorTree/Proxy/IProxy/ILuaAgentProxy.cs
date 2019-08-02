namespace BehaviorTree
{
    public interface ILuaAgentProxy
    {
        void OnStart();

        void OnAwake();


        void OnEnable();


        void OnDisable();


        void OnDestroy();


        void OnUpdate();


        // void OnFixedUpdate(float dedeltaTime);

        string[] OnGetEvents();

        void OnNotify(string evt, params object[] args);

    }
}