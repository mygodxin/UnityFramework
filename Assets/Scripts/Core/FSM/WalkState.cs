

using UnityEngine;

public class WalkState : BaseState
{
    public WalkState() : base(EState.walk)
    {

    }
    public override void Enter()
    {
        Debug.Log("walk-enter");
    }
    public override void Run()
    {
        Debug.Log("walk-run");
    }
    public override void Exit()
    {
        Debug.Log("walk-exit");
    }
}