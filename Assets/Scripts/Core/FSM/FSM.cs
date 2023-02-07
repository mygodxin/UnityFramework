using System.Collections.Generic;

/// <summary>
/// ÓÐÏÞ×´Ì¬»ú
/// </summary>
public class FSM
{
    Dictionary<EState, StateBase> _states;
    private StateBase _curState;
    public FSM(EState initial, Dictionary<EState, StateBase> states)
    {
        _states = states;
    }

    public void Update()
    {
        _curState.Run();
    }

    public void SwitchState(StateBase state)
    {
        _curState.Exit();
        _curState = state;
        _curState.Enter();
    }
}