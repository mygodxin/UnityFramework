using System;
using UnityWebSocket;

namespace UnityFramework
{

    public class Facade
    {
        private EventTarget _eventTarget = new EventTarget();
        private ServerCommond _serverCommond = new ServerCommond();

        private static Facade _inst = null;
        public static Facade inst
        {
            get
            {
                if (_inst == null)
                    _inst = new Facade();
                return _inst;
            }
        }

        public void init()
        {

        }

        public void ExcuteServerCommond(object sender, MessageEventArgs e)
        {
            _serverCommond.OnMessage(sender, e);
        }

        public void Emit(string name, Object data = null)
        {
            _eventTarget.Emit(name, data);
        }

        public void On(string name, EventCallback eventCallback)
        {
            _eventTarget.On(name, eventCallback);
        }

        public void Off(string name, EventCallback eventCallback)
        {
            _eventTarget.Off(name, eventCallback);
        }
    }
}
