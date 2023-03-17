using System.Collections.Generic;

namespace UnityFramework
{
    /// <summary>
    /// 事件回调
    /// </summary>
    /// <param name="param"></param>
    public delegate void EventCallback(object param = null);

    /// <summary>
    /// 事件桥接
    /// </summary>
    public class EventBridge
    {
        /// <summary>
        /// 回调函数
        /// </summary>
        public EventCallback eventcallback;

        private static Stack<EventBridge> _pool = new Stack<EventBridge>();
        internal static EventBridge Get()
        {
            if (_pool.Count > 0)
            {
                EventBridge eventCallback = _pool.Pop();
                return eventCallback;
            }
            else
            {
                return new EventBridge();
            }
        }
        internal static void Put(EventBridge eventCallback)
        {
            _pool.Push(eventCallback);
        }
    }
}