
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework
{
    /// <summary>
    /// ´°¿Ú
    /// </summary>
    public class BaseWindow : Window
    {
        protected override void DoShowAnimation()
        {
            view.transform.localScale.Set(.1f, .1f, .1f);
            var tween = view.transform.DOScale(Vector3.one, 0.25f);
            tween.SetEase(Ease.OutBack);
            tween.onComplete += OnShow;
        }


        protected override void OnInited()
        {
            base.OnInited();
            registerEvent();
        }


        protected override void DoHideAnimation()
        {
            var tween = view.transform.DOScale(new Vector3(.1f, .1f, .1f), 0.25f);
            tween.SetEase(Ease.InBack);
            tween.onComplete += HideImmediately;
        }

        public override void HideImmediately()
        {
            removeEvent();
            base.HideImmediately();
        }

        Dictionary<string, EventCallback> callbackDic;
        protected virtual string[] eventList()
        {
            return null;
        }
        protected void registerEvent()
        {
            string[] eventList = this.eventList();
            if (eventList != null)
            {
                callbackDic = new Dictionary<string, EventCallback>();
                foreach (var str in eventList)
                {
                    EventCallback callback = delegate (object data)
                    {
                        onEvent(str, data);
                    };
                    EventManager.inst.On(str, callback);
                    callbackDic.Add(str, callback);
                }
            }
        }
        protected void removeEvent()
        {
            string[] eventList = this.eventList();
            if (eventList != null)
            {
                foreach (var str in callbackDic)
                {
                    EventManager.inst.Off(str.Key, str.Value);
                }
            }
        }
        protected virtual void onEvent(string eventName, object data)
        {
        }
    }

}