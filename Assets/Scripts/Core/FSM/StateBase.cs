public enum EState
{
    idle,
    walk,
    run,
    atk,
}
public abstract class StateBase
{
    private EState _state;
    public StateBase(EState state)
    {
        _state = state;
    }
    public EState state
    {
        get
        {
            return _state;
        }
    }
    public abstract void Enter();
    public abstract void Run();
    public abstract void Exit();
}