using System;
using UFO;

public class EventManager
{
    private readonly EventTarget _eventTarget = new EventTarget();

    private static EventManager _inst = null;
    public static EventManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new EventManager();
            return _inst;
        }
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
