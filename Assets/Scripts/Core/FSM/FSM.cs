using System.Collections.Generic;

/// <summary>
/// 有限状态机
/// </summary>
public class FSM
{
    Dictionary<EState, BaseState> _states;
    private BaseState _curState;    //当前状态
    private BaseState _preState;    //上一个状态
    public FSM(EState initial, Dictionary<EState, BaseState> states)
    {
        _states = states;
    }

    public void Update()
    {
        _curState.Run();
    }
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    public void SwitchState(BaseState state)
    {
        this._preState = _curState;
        _curState.Exit();
        _curState = state;
        _curState.Enter();
    }
    public void RevertToPreState()
    {
        this.SwitchState(this._preState);
    }
    /// <summary>
    /// 当前状态
    /// </summary>
    /// <returns></returns>
    public BaseState CurState()
    {
        return this._curState;
    }
    /// <summary>
    /// 上一个状态
    /// </summary>
    /// <returns></returns>
    public BaseState PreState()
    {
        return this._preState;
    }
}