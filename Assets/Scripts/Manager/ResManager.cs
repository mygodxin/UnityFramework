
public class ResManager
{
    public static string AssetsPath = "Assets/AssetsPackage";
    public static string UIPath = "Assets/AssetsPackage/UI/";

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
