using System;
using UnityEngine;
using HS;

/// <summary>
/// 播放序列帧动画
/// </summary>
public class AnimationUtil
{
    /// <summary>
    /// 播放animator
    /// </summary>
    //private TimerCallback update;
    public static void Play(Animator animator, string animationName, Action completeAction)
    {
        var aniComp = animator.GetComponent<GAnimation>();
        if (aniComp == null)
            aniComp = animator.gameObject.AddComponent<GAnimation>();
        aniComp.AnimationFinishedHandle = completeAction;
        animator.Play(animationName, 0, 0f);
    }

    public static void PlayDuration()
    {

    }
}