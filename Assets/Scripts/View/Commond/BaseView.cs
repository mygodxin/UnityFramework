
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    /// <summary>
    /// ´°¿Ú
    /// </summary>
    public class BaseView : UIView
    {
        protected override void DoShowAnimation()
        {
            this.transform.localScale.Set(.1f, .1f, .1f);
            var tween = this.transform.DOScale(Vector3.one, 0.25f);
            tween.SetEase(Ease.OutBack);
            tween.onComplete += OnShow;
        }


        protected override void OnEnable()
        {
            this.RegisterEvent();
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            this.RemoveEvent();
            base.OnDisable();
        }

        protected override void DoHideAnimation()
        {
            var tween = this.transform.DOScale(new Vector3(.1f, .1f, .1f), 0.25f);
            tween.SetEase(Ease.InBack);
            tween.onComplete += HideImmediately;
        }

        Dictionary<string, EventCallback> callbackDic;
        protected virtual string[] eventList()
        {
            return null;
        }
        protected void RegisterEvent()
        {
            string[] eventList = this.eventList();
            if (eventList != null)
            {
                callbackDic = new Dictionary<string, EventCallback>();
                foreach (var str in eventList)
                {
                    EventCallback callback = delegate (object data)
                    {
                        OnEvent(str, data);
                    };
                    EventManager.Inst.On(str, callback);
                    callbackDic.Add(str, callback);
                }
            }
        }
        protected void RemoveEvent()
        {
            string[] eventList = this.eventList();
            if (eventList != null)
            {
                foreach (var str in callbackDic)
                {
                    EventManager.Inst.Off(str.Key, str.Value);
                }
            }
        }
        protected virtual void OnEvent(string eventName, object data)
        {
        }
    }

}