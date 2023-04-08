public enum EState
{
    idle,
    walk,
    run,
    atk,
}
public abstract class BaseState
{
    private EState _state;
    public BaseState(EState state)
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