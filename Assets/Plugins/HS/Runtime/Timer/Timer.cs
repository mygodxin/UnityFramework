using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace HS
{
    /// <summary>
    /// 计时器
    /// </summary>
    public class Timer
    {
        private ObjectPool<TimerNode> timerPool;
        private Dictionary<int, TimerNode> timers = new Dictionary<int, TimerNode>();
        private Dictionary<int, TimerNode> addList = new Dictionary<int, TimerNode>();
        private List<TimerNode> removeList = new List<TimerNode>();

        private int nextTimerId = 1;

        private static Timer inst;
        public static Timer Inst
        {
            get
            {
                if (inst == null)
                    inst = new Timer();
                return inst;
            }
            private set
            {
                inst = value;
            }
        }

        public Timer(bool useDefaultUpdate = true)
        {
            if (useDefaultUpdate)
            {
                var go = new GameObject("Timer");
                go.hideFlags = HideFlags.HideInHierarchy;
                go.AddComponent<TimerMono>();
            }

            timerPool = new ObjectPool<TimerNode>(() =>
            {
                return new TimerNode();
            }, (node) =>
            {
                node.IsRemove = false;
            }, (node) =>
            {
                node.IsRemove = true;
            });

        }
        /// <summary>
        /// 添加每帧调用计时器
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="onTick"></param>
        /// <returns></returns>
        public int SetInterval(float interval, Action onTick)
        {
            return AddTimer(interval, -1, 0, onTick, null);
        }
        /// <summary>
        /// 添加延迟调用计时器
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public int SetTimeout(float delay, Action onComplete)
        {
            return AddTimer(0, 1, delay, null, onComplete);
        }
        /// <summary>
        /// 添加计时器
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="repeatCount"></param>
        /// <param name="delay"></param>
        /// <param name="onTick"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public int AddTimer(float interval, int repeatCount, float delay, Action onTick, Action onComplete)
        {
            int timerId = nextTimerId;

            TimerNode timer = timerPool.Get();
            timer.TimeId = timerId;
            this.addList.Add(timerId, timer);
            timer.Initialize(interval, repeatCount, onTick, onComplete, delay);

            UpdateNextTimerId();
            return timerId;
        }

        private void UpdateNextTimerId()
        {
            nextTimerId++;
            if (nextTimerId == int.MaxValue) // 达到上限
            {
                // 重置计时器标识符，确保不与已有的计时器标识符冲突
                nextTimerId = 1;
                while (timers.ContainsKey(nextTimerId))
                {
                    nextTimerId++;
                }
            }
        }
        /// <summary>
        /// 移除指定id计时器
        /// </summary>
        /// <param name="timerId"></param>
        public void RemoveTimer(int timerId)
        {
            if (addList.TryGetValue(timerId, out var addTimer))
            {
                timerPool.Release(addTimer);
                addList.Remove(timerId);
            }

            //只标记不直接删，防止dic迭代过程中报错
            if (timers.TryGetValue(timerId, out var timer))
            {
                timer.IsRemove = true;
            }
        }
        /// <summary>
        /// 暂停指定id计时器
        /// </summary>
        /// <param name="timerId"></param>
        public void PauseTimer(int timerId)
        {
            if (timers.ContainsKey(timerId))
            {
                timers[timerId].Pause();
            }
        }
        /// <summary>
        /// 恢复指定id计时器
        /// </summary>
        /// <param name="timerId"></param>
        public void ResumeTimer(int timerId)
        {
            if (timers.ContainsKey(timerId))
            {
                timers[timerId].Resume();
            }
        }
        /// <summary>
        /// 暂停所有计时器
        /// </summary>
        public void PauseAllTimers()
        {
            foreach (var timer in timers.Values)
            {
                timer.Pause();
            }
        }
        /// <summary>
        /// 恢复所有计时器
        /// </summary>
        public void ResumeAllTimers()
        {
            foreach (var timer in timers.Values)
            {
                timer.Resume();
            }
        }

        public void Update(float deltaTime)
        {
            //添加到timers中
            foreach (var item in addList)
            {
                this.timers.Add(item.Key, item.Value);
            }
            this.addList.Clear();
            //迭代
            foreach (var timer in timers.Values)
            {
                if (timer.IsRemove)
                {
                    this.removeList.Add(timer);
                    continue;
                }
                try
                {
                    timer.Update(deltaTime);
                }
                catch (Exception e)
                {
                    timer.IsRemove = true;
                    Debug.LogError(e);
                    continue;
                }
            }

            // 移除完成的计时器
            foreach (var timer in removeList)
            {
                timerPool.Release(timer);
                timers.Remove(timer.TimeId);
            }
            removeList.Clear();
        }
    }

    public class TimerNode
    {
        private float interval;           //触发间隔
        private int repeatCount;          //循环次数
        private Action onTick;            //间隔回调
        private Action onComplete;        //完成回调
        private float elapsedTime;        //运行时间
        public bool IsRemove;             //是否移除
        public int TimeId;                //计时器id
        private float delay;              //延迟时间
        private bool paused;              //暂停

        public TimerNode()
        {

        }

        public void Initialize(float interval, int repeatCount, Action onTick, Action onComplete, float delay)
        {
            this.interval = interval;
            this.repeatCount = repeatCount;
            this.onTick = onTick;
            this.onComplete = onComplete;

            elapsedTime = 0f;

            this.delay = delay;

            paused = false;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }

        public void Update(float deltaTime)
        {
            if (paused)
                return;

            if (delay > 0)
            {
                delay -= deltaTime;
                if (delay > 0f)
                    return;
                delay = 0;
            }

            elapsedTime += deltaTime;

            if (elapsedTime >= interval)
            {
                elapsedTime -= interval > 0 ? interval : deltaTime;

                if (onTick != null)
                    onTick.Invoke();

                if (repeatCount > 0)
                {
                    repeatCount--;
                    if (repeatCount == 0)
                    {
                        CompleteTimer();
                        IsRemove = true;
                    }
                }
            }
        }

        private void CompleteTimer()
        {
            if (onComplete != null)
                onComplete.Invoke();
            this.IsRemove = true;
        }
    }

    class TimerMono : MonoBehaviour
    {
        private void Update()
        {
            Timer.Inst.Update(Time.deltaTime);
        }
    }

}