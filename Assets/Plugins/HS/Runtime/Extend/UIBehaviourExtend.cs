using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HS
{
    public static class UIExtend
    {
        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="button"></param>
        /// <param name="url"></param>
        public static void SetIcon(this GameObject ui, string url)
        {
            var iconImage = ui.transform.Find("Icon");
            if (iconImage == null)
            {
                Debug.LogWarning(ui.transform.Find("Icon"));
                return;
            }
            var img = iconImage.GetComponent<Image>();
            img.LoadImageAsync(url);
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="button"></param>
        /// <param name="str"></param>
        public static void SetTitle(this GameObject ui, string text)
        {
            var titleTText = ui.transform.Find("Title");
            if (titleTText == null)
            {
                Debug.LogWarning(ui.name + "no find it.");
                return;
            }
            var txt = titleTText.GetComponent<TextMeshProUGUI>();
            txt.text = text;
        }
        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="button"></param>
        /// <param name="url"></param>
        public static void SetIcon(this UIBehaviour ui, string url)
        {
            var iconImage = ui.transform.Find("Icon");
            if (iconImage == null)
            {
                Debug.LogWarning(ui.transform.Find("Icon"));
                return;
            }
            var img = iconImage.GetComponent<Image>();
            img.LoadImageAsync(url);
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="button"></param>
        /// <param name="str"></param>
        public static void SetTitle(this UIBehaviour ui, string text)
        {
            var titleTText = ui.transform.GetChildWithName("Title");
            if (titleTText == null)
            {
                Debug.LogWarning(ui.name + "no find it.");
                return;
            }
            var txt = titleTText.GetComponent<TextMeshProUGUI>();
            txt.text = text;
        }
        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="button"></param>
        /// <param name="url"></param>
        public static void SetIcon(this Transform ui, string url)
        {
            var iconImage = ui.transform.Find("Icon");
            if (iconImage == null)
            {
                Debug.LogWarning(ui.transform.Find("Icon"));
                return;
            }
            var img = iconImage.GetComponent<Image>();
            img.LoadImageAsync(url);
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="button"></param>
        /// <param name="str"></param>
        public static void SetTitle(this Transform ui, string text)
        {
            var titleTText = ui.transform.Find("Title");
            if (titleTText == null)
            {
                Debug.LogWarning(ui.name + "no find it.");
                return;
            }
            var txt = titleTText.GetComponent<TextMeshProUGUI>();
            txt.text = text;
        }

        ////参数分别为：1.UI修改目标的Transform		2.朝向向量		3.起始向量
        //public static void UILookAt(this Transform transform, Vector3 dir, Vector3 lookAxis)
        //{
        //    Quaternion q = Quaternion.identity;
        //    q.SetFromToRotation(lookAxis, dir);
        //    transform.rotation = q;
        //}

        public static void LookAt2D(this Transform transform, Vector3 localPostion)
        {
            Vector2 direction = transform.localPosition - localPostion;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
            transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}