

using UnityEngine;

public class RunState : BaseState
{
    public RunState() : base(EState.run)
    {

    }
    public override void Enter()
    {
        Debug.Log("run-enter");
    }
    public override void Run()
    {
        Debug.Log("run-run");
    }
    public override void Exit()
    {
        Debug.Log("run-exit");
    }
}