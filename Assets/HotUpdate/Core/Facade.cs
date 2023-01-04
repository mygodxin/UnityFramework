using System;
public class Facade
{
    private EventTarget eventTarget = new EventTarget();

    private static Facade inst = null;
    public static Facade Inst
    {
        get
        {
            if (inst == null)
                inst = new Facade();
            return inst;
        }
    }

    public void init()
    {

    }

    public void Emit(string name, Object data)
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
