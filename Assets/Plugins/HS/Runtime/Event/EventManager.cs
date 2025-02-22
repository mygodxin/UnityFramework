using System;

namespace HS
{
    public class EventManager
    {
        private readonly EventTarget eventTarget = new EventTarget();

        public static EventManager Inst = new EventManager();

        public void Send(string name, object data = null)
        {
            eventTarget.Send(name, data);
        }

        public void On(string name, Action<object> eventCallback)
        {
            eventTarget.On(name, eventCallback);
        }

        public void Off(string name, Action<object> eventCallback)
        {
            eventTarget.Off(name, eventCallback);
        }
    }
}
