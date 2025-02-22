using System;
using System.Collections.Generic;

namespace HS
{
    /// <summary>
    /// 事件处理器,Action<object>在实际使用中会存在拆箱和装箱的过程，事件不适合用于密集型运算的场景
    /// </summary>
    public class EventTarget
    {
        private Dictionary<string, Action<object>> eventHandles = new Dictionary<string, Action<object>>();

        public void Send(string name, object data = default)
        {
            if (eventHandles.ContainsKey(name))
            {
                eventHandles[name]?.Invoke(data);
            }
        }

        public void On(string name, Action<object> eventCallback)
        {
            if (eventHandles.ContainsKey(name))
            {
                eventHandles[name] += eventCallback;
            }
            else
            {
                eventHandles[name] = eventCallback;
            }
        }

        public void Off(string name, Action<object> eventCallback)
        {
            if (eventHandles.ContainsKey(name))
            {
                eventHandles[name] -= eventCallback;
            }
        }

        public void Clear()
        {
            eventHandles.Clear();
        }
        public void Clear(string name)
        {
            eventHandles[name] = null;
        }

        public bool Has(string name)
        {
            eventHandles.TryGetValue(name, out var value);
            return value == null;
        }
    }
}