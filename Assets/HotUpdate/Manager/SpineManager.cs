using Spine.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpineManager
{
    private static SpineManager inst = null;
    public static SpineManager Inst
    {
        get
        {
            if (inst == null)
                inst = new SpineManager();
            return inst;
        }
    }

    public void Init()
    {
    }

    public void PlaySpine(string name, string skinName, string _animationName, bool _loop, bool _playing, int _frame)
    {
        SkeletonDataAsset asset = Addressables.LoadAssetAsync<SkeletonDataAsset>(name + "_SkeletonData").WaitForCompletion();
        if (asset == null) return;
        var _spineAnimation = SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(asset);
        _spineAnimation.gameObject.name = asset.name;
        Spine.SkeletonData dat = asset.GetSkeletonData(false);
        _spineAnimation.gameObject.transform.localScale = new Vector3(1 / asset.scale, 1 / asset.scale, 1);
        //_spineAnimation.gameObject.transform.localPosition = new Vector3(anchor.x, -anchor.y, 0);
        //SetWrapTarget(_spineAnimation.gameObject, cloneMaterial, width, height);
        var skeletonData = _spineAnimation.skeleton.Data;

        var state = _spineAnimation.AnimationState;
        Spine.Animation animationToUse = !string.IsNullOrEmpty(_animationName) ? skeletonData.FindAnimation(_animationName) : null;
        if (animationToUse != null)
        {
            var trackEntry = state.GetCurrent(0);
            if (trackEntry == null || trackEntry.Animation.Name != _animationName || trackEntry.IsComplete && !trackEntry.Loop)
                trackEntry = state.SetAnimation(0, animationToUse, _loop);
            else
                trackEntry.Loop = _loop;

            if (_playing)
                trackEntry.TimeScale = 1;
            else
            {
                trackEntry.TimeScale = 0;
                trackEntry.TrackTime = Mathf.Lerp(0, trackEntry.AnimationEnd - trackEntry.AnimationStart, _frame / 100f);
            }
        }
        else
            state.ClearTrack(0);

        var skin = !string.IsNullOrEmpty(skinName) ? skeletonData.FindSkin(skinName) : skeletonData.DefaultSkin;
        if (skin == null && skeletonData.Skins.Count > 0)
            skin = skeletonData.Skins.Items[0];
        if (_spineAnimation.skeleton.Skin != skin)
        {
            _spineAnimation.skeleton.SetSkin(skin);
            _spineAnimation.skeleton.SetSlotsToSetupPose();
        }
    }
}
