using UnityEngine;
using UnityEngine.InputSystem;

public interface BaseState
{
    void Enter();
    void Execute();
    void Exit();  
}

public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    public void Initialize(BaseState initialState)
    {
        CurrentState = initialState;
        CurrentState?.Enter();
    }

    public void Update()
    {
       CurrentState?.Execute(); 
    }

    public void ChangeState(BaseState NewState)
    {
        CurrentState?.Exit();
        CurrentState = NewState;
        CurrentState?.Enter();
    }
}
