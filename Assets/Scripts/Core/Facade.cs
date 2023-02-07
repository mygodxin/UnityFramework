using System;
public class Facade
{
    private EventTarget eventTarget = new EventTarget();

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

    public void Emit(string name, Object data = null)
    {
        eventTarget.Emit(name, data);
    }

    public void On(string name, EventCallback eventCallback)
    {
        eventTarget.On(name, eventCallback);
    }

    public void Off(string name, EventCallback eventCallback)
    {
        eventTarget.Off(name, eventCallback);
    }
}
