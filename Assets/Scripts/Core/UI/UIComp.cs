using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

namespace HS
{
    //public interface IPath
    //{
    //    public static abstract string path();
    //}
    /// <summary>
    /// 组件基类，继承自MonoBehaviour，使用必须覆盖path，Show会自动加载
    /// </summary>
    public abstract class UIComp : MonoBehaviour//,IPath
    {
        /// <summary>
        /// 资源路径，c# 11才有static abstract 等待升级
        /// </summary>
        //public static string path
        //{
        //    get
        //    {
        //        throw new System.Exception("未实现path");
        //    }
        //}
        /// <summary>
        /// 数据
        /// </summary>
        public object data;

        private void Awake()
        {
        }

        /// <summary>
        /// 请使用OnInit，如果必须调用Awake，请使用base.Awake
        /// </summary>
        private void Start()
        {
            this.OnInit();
        }

        public virtual void OnAddedToStage(object obj = null)
        {
            this.data = obj;
            this.OnShow();
        }
        /// <summary>
        /// 请使用OnHide，如果必须调用OnDisable，请使用base.OnDisable
        /// </summary>
        public virtual void OnRemovedFromStage()
        {
            this.OnHide();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInit()
        {

        }
        /// <summary>
        /// 打开
        /// </summary>
        protected virtual void OnShow()
        {

        }
        /// <summary>
        /// 关闭
        /// </summary>
        protected virtual void OnHide()
        {

        }

        public virtual void Hide()
        {
            this.OnRemovedFromStage();
            this.gameObject.SetActive(false);
        }
    }
}