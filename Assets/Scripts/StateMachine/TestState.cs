using UnityEngine;

public class TestState : BaseState
{
    public void Enter()
    {
        Debug.Log("STATINNGGGNGNGNGNGNGNG");
    }

    public void Execute()
    {
        Debug.Log("UPDATINGGNGNGNGNGNGNGNGN");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.ChangeState(new TestState2());
        }
    }

    public void Exit()
    {
        Debug.Log("EXITINGNGNGNGNGNGNGNG");
    }
}

public class TestState2 : BaseState
{
    public void Enter()
    {
        Debug.Log("Start2");
    }

    public void Execute()
    {
        Debug.Log("Update2");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.ChangeState(new TestState());
        }
    }

    public void Exit()
    {
        Debug.Log("Exit2");
    }
}

