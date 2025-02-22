using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

//自动生成animation
public class FrameAnimationCreator : Editor
{
    [MenuItem("Assets/Create Frame Animation", true)]
    public static bool IsSelect()
    {
        UnityEngine.Object[] selectedObjects = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets);
        return selectedObjects.Length > 0;
    }
    private static string selectAssetPath;
    private static Action<string, DirectoryInfo> excute;
    [MenuItem("Assets/Create Frame Animation", false, 1)]
    public static void Select()
    {
        UnityEngine.Object[] selectedObjects = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets);
        UnityEngine.Object obj = selectedObjects[0];

        // 设置要处理的文件夹路径
        string folderPath = AssetDatabase.GetAssetPath(obj);

        selectAssetPath = folderPath;
        DirectoryInfo raw = new DirectoryInfo(folderPath);
        excute = (_selectAssetPath, raw) =>
        {
            if (raw.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo dictorys in raw.GetDirectories())
                {
                    Build(_selectAssetPath + "/" + dictorys.Name, dictorys);
                    if (dictorys.GetDirectories().Length > 0)
                    {
                        excute.Invoke(_selectAssetPath + "/" + dictorys.Name, dictorys);
                    }
                }
            }
            else
            {
                Build(_selectAssetPath, raw);
            }
        };
        excute.Invoke(selectAssetPath, raw);
    }
    private static void Build(string path, DirectoryInfo dictorys)
    {
        //图片数量大于1才会生成
        FileInfo[] images = dictorys.GetFiles("*.png");
        if (images.Length <= 0) return;
        //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
        AnimationClip clip = BuildAnimationClip(dictorys, path);
        //把所有的动画文件生成在一个AnimationController里
        AnimatorController controller = BuildAnimationController(clip, dictorys, path);
        //最后生成程序用的Prefab文件
        BuildPrefab(dictorys, controller, path);
    }

    private static AnimationClip BuildAnimationClip(DirectoryInfo dictorys, string path)
    {
        string animationName = dictorys.Name;
        FileInfo[] images = dictorys.GetFiles("*.png");
        AnimationClip clip = new AnimationClip();
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(Image);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[images.Length];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        float frameTime = 1 / 10f;
        for (int i = 0; i < images.Length; i++)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images[i].FullName));
            keyFrames[i] = new ObjectReferenceKeyframe();
            keyFrames[i].time = frameTime * i;
            keyFrames[i].value = sprite;
        }
        //动画帧率，30比较合适
        clip.frameRate = 30;

        SerializedObject serializedClip = new SerializedObject(clip);
        AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
        clipSettings.loopTime = true;
        serializedClip.ApplyModifiedProperties();

        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, path + "/" + animationName + ".anim");
        AssetDatabase.SaveAssets();
        return clip;
    }

    private static AnimatorController BuildAnimationController(AnimationClip clip, DirectoryInfo dictorys, string path)
    {
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(path + "/" + dictorys.Name + ".controller");
        AnimatorControllerLayer layer = animatorController.layers[0];
        AnimatorStateMachine sm = layer.stateMachine;
        AnimatorState state = sm.AddState(clip.name);
        state.motion = clip;
        //sm.defaultState = state;
        AssetDatabase.SaveAssets();
        return animatorController;
    }

    private static void BuildPrefab(DirectoryInfo dictorys, AnimatorController animatorCountorller, string path)
    {
        FileInfo images = dictorys.GetFiles("*.png")[0];
        GameObject go = new GameObject("FrameAnimationCreator");
        go.name = dictorys.Name;

        //SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
        //spriteRender.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images.FullName));
        Image image = go.AddComponent<Image>();
        image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images.FullName));

        Animator animator = go.AddComponent<Animator>();
        animator.runtimeAnimatorController = animatorCountorller;
        PrefabUtility.SaveAsPrefabAsset(go, path + "/" + go.name + ".prefab");
        DestroyImmediate(go);
    }

    public static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return path.Substring(path.IndexOf("Assets\\"));
        else
            return path.Substring(path.IndexOf("Assets/"));
    }

    class AnimationClipSettings
    {
        SerializedProperty m_Property;

        private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

        public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

        public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
        public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
        public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
        public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
        public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

        public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
        public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
        public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
        public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
        public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
        public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
        public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
        public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
        public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
        public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
    }
}