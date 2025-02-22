
namespace HS
{
    public abstract class StateBase
    {
        protected StateMachine stateMachine;

        public virtual void Init(IStateMachineOwner owner, int stateType, StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
    }
}