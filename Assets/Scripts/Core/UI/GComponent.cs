using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityFramework
{
    /// <summary>
    /// 组件
    /// </summary>
    public abstract class GComponent: EventTarget
    {
        /// <summary>
        /// 显示对象
        /// </summary>
        public GameObject view;

        /// <summary>
        /// 数据
        /// </summary>
        public object data;

        public GComponent(GameObject view = null)
        {
            this.view = view;
            this.BindComponent();
            this.OnInit();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void OnInit()
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
        protected void BindComponent()
        {
            Type type = this.GetType();
            FieldInfo[] properties = type.GetFields();
            foreach (var prop in properties)
            {
                if (prop.FieldType.IsSubclassOf(typeof(UIBehaviour)))
                {
                    var a = this.view.transform.GetComponentsInChildren(prop.FieldType, true);
                    foreach (var item in a)
                    {
                        if (item.transform.name == prop.Name)
                        {
                            prop.SetValue(this, item);
                            break;
                        }
                    }
                }
            }
        }
        protected Transform GetTransform(string path)
        {
            return view.transform.Find("Canvas/" + path);
        }
        protected Image GetImage(string path)
        {
            return this.GetTransform(path).GetComponent<Image>();
        }
        protected RawImage GetRawImage(string path)
        {
            return this.GetTransform(path).GetComponent<RawImage>();
        }
        protected Mask GetMask(string path)
        {
            return this.GetTransform(path).GetComponent<Mask>();
        }
        protected Shadow GetShadow(string path)
        {
            return this.GetTransform(path).GetComponent<Shadow>();
        }
        protected Outline GetOutline(string path)
        {
            return this.GetTransform(path).GetComponent<Outline>();
        }
        protected Button GetButton(string path)
        {
            return this.GetTransform(path).GetComponent<Button>();
        }
        protected Toggle GetToggle(string path)
        {
            return this.GetTransform(path).GetComponent<Toggle>();
        }
        protected ToggleGroup GetToggleGroup(string path)
        {
            return this.GetTransform(path).GetComponent<ToggleGroup>();
        }
        protected Slider GetSlider(string path)
        {
            return this.GetTransform(path).GetComponent<Slider>();
        }
        protected Scrollbar GetScrollbar(string path)
        {
            return this.GetTransform(path).GetComponent<Scrollbar>();
        }
        protected Dropdown GetDropdown(string path)
        {
            return this.GetTransform(path).GetComponent<Dropdown>();
        }
        protected InputField GetInputField(string path)
        {
            return this.GetTransform(path).GetComponent<InputField>();
        }
        protected ScrollRect GetScrollRect(string path)
        {
            return this.GetTransform(path).GetComponent<ScrollRect>();
        }
        protected LayoutElement GetLayoutElement(string path)
        {
            return this.GetTransform(path).GetComponent<LayoutElement>();
        }
        protected ContentSizeFitter GetContentSizeFitter(string path)
        {
            return this.GetTransform(path).GetComponent<ContentSizeFitter>();
        }
        protected AspectRatioFitter GetAspectRatioFitter(string path)
        {
            return this.GetTransform(path).GetComponent<AspectRatioFitter>();
        }
        protected HorizontalLayoutGroup GetHorizontalLayoutGroup(string path)
        {
            return this.GetTransform(path).GetComponent<HorizontalLayoutGroup>();
        }
        protected VerticalLayoutGroup GetVerticalLayoutGroup(string path)
        {
            return this.GetTransform(path).GetComponent<VerticalLayoutGroup>();
        }
        protected GridLayoutGroup GetGridLayoutGroup(string path)
        {
            return this.GetTransform(path).GetComponent<GridLayoutGroup>();
        }
        protected TMP_Text GetTextTMP(string path)
        {
            return this.GetTransform(path).GetComponent<TMP_Text>();
        }
        protected GList GetList(string path)
        {
            return this.GetTransform(path).GetComponent<GList>();
        }
    }
}