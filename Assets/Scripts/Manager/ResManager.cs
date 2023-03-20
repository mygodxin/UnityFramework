
public class ResManager
{
    public static string AssetsPath = "Assets/";
    public static string UIPath = "Assets/Prefabs/UI/";

    private static ResManager _inst = null;
    public static ResManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new ResManager();
            return _inst;
        }
    }

    public void Init()
    {
    }
}
