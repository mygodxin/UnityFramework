using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public delegate void TimerCallback(object param);
/// <summary>
/// 计时器
/// </summary>
public class Timers
{
    GameObject _gameObject;
    TimerMono timerMono;

    Dictionary<TimerCallback, TimerNode> curTimers;
    Dictionary<TimerCallback, TimerNode> waitAdd;
    List<TimerNode> waitRemove;

    private static Timers _inst = null;
    public static Timers inst
    {
        get
        {
            if (_inst == null)
                _inst = new Timers();
            return _inst;
        }
    }

    public Timers()
    {
        curTimers = new Dictionary<TimerCallback, TimerNode>();
        waitAdd = new Dictionary<TimerCallback, TimerNode>();
        waitRemove = new List<TimerNode>();

        _gameObject = new GameObject();
        _gameObject.hideFlags = HideFlags.HideInHierarchy;
        _gameObject.SetActive(true);
        Object.DontDestroyOnLoad(_gameObject);

        timerMono = _gameObject.AddComponent<TimerMono>();
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="interval"></param>间隔(s)
    /// <param name="repeat"></param> 循环次数
    /// <param name="callback"></param> 回调
    public void Add(float interval, int repeat, TimerCallback callback)
    {
        Add(interval, repeat, callback, null);
    }

    public void Add(float interval, int repeat, TimerCallback callback, object callbackParam)
    {
        if (callback == null)
        {
            return;
        }

        TimerNode t;

        if (curTimers.TryGetValue(callback, out t))
        {
            t.Set(interval, repeat, callback, callbackParam);
            t.elapsed = 0;
            t.deleted = false;
            return;
        }

        if (waitAdd.TryGetValue(callback, out t))
        {
            t.Set(interval, repeat, callback, callbackParam);
            return;
        }

        t = new TimerNode();
        t.interval = interval;
        t.repeat = repeat;
        t.callback = callback;
        t.param = callbackParam;
        waitAdd.Add(callback, t);
    }

    public bool IsExist(TimerCallback callback)
    {
        if (waitAdd.ContainsKey(callback))
        {
            return true;
        }

        if(curTimers.TryGetValue(callback, out var tn))
        {
            return !tn.deleted;
        }

        return false;
    }

    public void Remove(TimerCallback callback)
    {
        TimerNode t;
        if (waitAdd.TryGetValue(callback, out t))
        {
            waitAdd.Remove(callback);
        }
        
        if(curTimers.TryGetValue(callback, out t))
        {
            t.deleted = true;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        float dt = Time.unscaledDeltaTime;
        Dictionary<TimerCallback, TimerNode>.Enumerator iter;

        if (curTimers.Count > 0)
        {
            iter = curTimers.GetEnumerator();
            while (iter.MoveNext())
            {
                TimerNode i = iter.Current.Value;

                if (i.deleted)
                {
                    waitRemove.Add(i);
                    continue;
                }

                i.elapsed += dt;
                if (i.elapsed < i.interval)
                    continue;

                i.elapsed -= i.interval;
                if (i.elapsed < 0 || i.elapsed > 0.03f)
                    i.elapsed = 0;

                if (i.repeat > 0)
                {
                    i.repeat--;
                    if (i.repeat == 0)
                    {
                        i.deleted = true;
                        waitRemove.Add(i);
                    }
                }

                if (i.callback != null)
                {
                    i.callback(i.param);
                }
            }
            iter.Dispose();
        }

        int removeLen = waitRemove.Count;
        if (removeLen > 0)
        {
            for(int i = 0; i < removeLen; i++)
            {
                TimerNode node = waitRemove[i];
                if(node.deleted && node.callback != null)
                {
                    curTimers.Remove(node.callback);
                }
            }
            waitRemove.Clear();
        }
        if(waitAdd.Count > 0)
        {
            iter = waitAdd.GetEnumerator();
            while (iter.MoveNext())
            {
                curTimers.Add(iter.Current.Key, iter.Current.Value);
            }
            iter.Dispose();
            waitAdd.Clear();
        }
    }
}

class TimerNode
{
    public int repeat;
    public float interval;
        public float elapsed;
    public bool deleted;

    public TimerCallback callback;
    public object param;

    public void Set(float interval, int repeat, TimerCallback callback, object param)
    {
        this.interval = interval;
        this.repeat = repeat;
        this.callback = callback;
        this.param = param;
    }
}

class TimerMono : MonoBehaviour
{
    private void Update()
    {
        Timers.inst.Update();
    }
}
