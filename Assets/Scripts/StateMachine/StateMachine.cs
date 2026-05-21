using UnityEngine;
using UnityEngine.InputSystem;

public interface BaseState
{
    public virtual void Enter() {}

    public virtual void Execute() {}

    public virtual void Exit() {}

}

public class StateMachine : MonoBehaviour
{
    public static BaseState CurrentState;

    private void Awake()
    {
    }
    void Start()
    {
        CurrentState = new TestState();
        CurrentState.Enter();
    }

    void Update()
    {
        CurrentState.Execute(); 
    }

    public static void ChangeState(BaseState NewState)
    {
        CurrentState.Exit();
        CurrentState = NewState;
        CurrentState.Enter();
    }
}