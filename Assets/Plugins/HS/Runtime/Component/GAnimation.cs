using System;
using UnityEngine;

namespace HS
{
    /// <summary>
    /// animation回调组件
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class GAnimation : MonoBehaviour
    {
        public Action AnimationFinishedHandle;
        private void Awake()
        {
            var animator = this.transform.GetComponent<Animator>();
            var clips = animator.runtimeAnimatorController.animationClips;
            var animationEvent = new AnimationEvent();
            animationEvent.time = clips[0].length;
            animationEvent.functionName = "OnAnimationFinished";
            clips[0].AddEvent(animationEvent);
        }
        private void Start()
        {

        }
        private void Update()
        {

        }
        private void OnAnimationFinished()
        {
            this.AnimationFinishedHandle?.Invoke();
        }
    }
}