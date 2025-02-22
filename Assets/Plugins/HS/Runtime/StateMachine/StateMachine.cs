using System.Collections.Generic;

namespace HS
{
    public interface IStateMachineOwner { }

    /// <summary>
    /// 状态机 运行需要调用Update,LateUpdate,FixedUpdate
    /// </summary>
    public class StateMachine
    {
        public int Type { get; private set; } = -1;
        private StateBase current;
        private IStateMachineOwner owner;

        private Dictionary<int, StateBase> stateDic = new Dictionary<int, StateBase>();

        public void Init(IStateMachineOwner owner)
        {
            this.owner = owner;
        }

        public bool ChangeState<T>(int newStateType, bool reCurrstate = false) where T : StateBase, new()
        {
            // 状态一致，并且不需要刷新状态，则切换失败
            if (newStateType == Type && !reCurrstate) return false;

            // 退出当前状态
            if (current != null)
            {
                current.Exit();
            }
            // 进入新状态
            current = GetState<T>(newStateType);
            Type = newStateType;
            current.Enter();

            return true;
        }

        private StateBase GetState<T>(int stateType) where T : StateBase, new()
        {
            if (stateDic.ContainsKey(stateType)) return stateDic[stateType];

            StateBase state = new T();
            state.Init(owner, stateType, this);
            stateDic.Add(stateType, state);
            return state;
        }

        public void Stop()
        {
            current.Exit();
            Type = -1;
            current = null;

            stateDic.Clear();
        }

        public void Destory()
        {
            Stop();
            owner = null;
        }

        public void Update()
        {
            current.Update();
        }

        public void LateUpdate()
        {
            current.LateUpdate();
        }

        public void FixedUpdate()
        {
            current.FixedUpdate();
        }
    }
}