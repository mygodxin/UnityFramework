using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UFO
{
    public class GuideManager
    {
        /// <summary>
        /// 引导进行到哪一步
        /// </summary>
        public int step = 0;
        private GuideComp guideComp;

        private static GuideManager _inst = null;
        public static GuideManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new GuideManager();
                return _inst;
            }
        }

        public void Init()
        {
        }

        public void Run()
        {

        }
        /// <summary>
        /// 打开引导
        /// </summary>
        /// <param name="rect"></param>
        public void ShowGuide(RectTransform rect)
        {
            if(this.guideComp == null)
            {
                var prefab = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/UI/GuideComp.prefab").WaitForCompletion();
                this.guideComp = Object.Instantiate(prefab).GetComponent<GuideComp>();
                this.guideComp.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
            this.guideComp.SetTarget(rect);
            this.guideComp.gameObject.SetActive(true);
        }
        /// <summary>
        /// 关闭引导
        /// </summary>
        public void HideGuide()
        {
            if(this.guideComp != null)
            {
                this.guideComp.gameObject.SetActive(false);
            }
        }
    }
}