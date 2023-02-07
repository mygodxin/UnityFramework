
/// <summary>
/// UI»ùÀà
/// </summary>
public class GComponent : EventTarget
{
    public GComponent()
    {
        On("onAddedToStage", onAddedToStage);
        On("onRemovedFromStage", onRemovedFromStage);
    }

    protected virtual void onAddedToStage(object data)
    {
    }
    protected virtual void onRemovedFromStage(object data)
    {
    }
}
