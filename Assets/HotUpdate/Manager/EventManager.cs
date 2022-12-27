using System;
public class EventManager
{
    private EventTarget eventTarget = new EventTarget();

    private static EventManager instance = null;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EventManager();
            return instance;
        }
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
