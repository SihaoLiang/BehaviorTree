
namespace BehaviorTree
{
    public interface ILuaNodeProxy
    {
        void OnAwake();

        void SetAgent();

        void OnEnable();

        void OnDisable();

        void OnEnter();

        void OnExit();

        void OnDestroy();

        void OnRunning(float deltaTime);

        void OnReset();

        void OnRecycle();

        void OnUpdate(float deltaTime);

        void OnFixedUpdate(float deltaTime);

        string[] OnGetEvents();


        void OnNotify(string evt, params object[] args);
    }
}