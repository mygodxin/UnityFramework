using Spine.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpineManager
{
    private static SpineManager _inst = null;
    public static SpineManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new SpineManager();
            return _inst;
        }
    }

    public void Init()
    {
    }

    public void PlaySpine(SkeletonGraphic skeletonGraphic,string name, string skinName, string _animationName, bool _loop, bool _playing, int _frame)
    {
        SkeletonDataAsset asset = Addressables.LoadAssetAsync<SkeletonDataAsset>(name + "_SkeletonData.asset").WaitForCompletion();
        if (asset == null) return;
        var _spineAnimation = skeletonGraphic;
        skeletonGraphic.skeletonDataAsset = asset;

        Spine.SkeletonData dat = asset.GetSkeletonData(false);

        var skeletonData = dat;

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
        if (_spineAnimation.Skeleton.Skin != skin)
        {
            _spineAnimation.Skeleton.SetSkin(skin);
            _spineAnimation.Skeleton.SetSlotsToSetupPose();
        }
    }
}
