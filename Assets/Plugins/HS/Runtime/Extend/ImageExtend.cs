using UnityEngine;
using UnityEngine.UI;

namespace HS
{






    public static class ImageExtend
    {
        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="url"></param>
        public static async void LoadImageAsync(this CircleImage image, string url, bool setNativeSize = false)
        {
            if (url == "") return;
            var sprite = await ResLoader.LoadAssetAsync<Sprite>(url);
            if (sprite == null)
            {
                var texture = await HttpRequest.Inst.GetTexture(url);
                if (texture != null)
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            if (sprite == null)
            {
                Debug.LogWarning("LoadImageAsync run fail,url=" + url);
                return;
            }
            if (image != null)// && image.isActiveAndEnabled)
            {
                image.Sprite = sprite;
                if (setNativeSize)
                    image.SetNativeSize();
            }
        }
        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="url"></param>
        public static async void LoadImageAsync(this Image image, string url, bool setNativeSize = false)
        {
            if (url == "") return;
            var sprite = await ResLoader.LoadAssetAsync<Sprite>(url);
            if (sprite == null)
            {
                var texture = await HttpRequest.Inst.GetTexture(url);
                if (texture != null)
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }

            if (sprite == null)
            {
#if UNITY_64
                //Debug.LogWarning("LoadImageAsync run fail,url=" + url);
#endif
                return;
            }




            if (image != null)// && image.isActiveAndEnabled)
            {
                image.sprite = sprite;
                if (setNativeSize)
                    image.SetNativeSize();
            }
        }

        public static async void LoadRawImageAsync(this RawImage rawImage, string url)
        {
            var modelPrefab = await ResLoader.LoadAssetAsync<GameObject>(url);
            var modelGO = Object.Instantiate(modelPrefab);
            var rawImageSize = rawImage.rectTransform.sizeDelta;
            var renderTexure = new RenderTexture((int)rawImageSize.x, (int)rawImageSize.y, 32);
            var renderCanvas = modelGO.GetComponentInChildren<Camera>();
            renderCanvas.targetTexture = renderTexure;
            renderCanvas.Render();
            renderCanvas.targetTexture = null;
            GameObject.Destroy(modelGO);
            rawImage.texture = renderTexure;
        }

        // 颜色混合
        public static Color NormalBlend(Color background, Color cover)
        {
            float CoverAlpha = cover.a;
            Color blendColor;
            blendColor.r = cover.r * CoverAlpha + background.r * (1 - CoverAlpha);
            blendColor.g = cover.g * CoverAlpha + background.g * (1 - CoverAlpha);
            blendColor.b = cover.b * CoverAlpha + background.b * (1 - CoverAlpha);
            blendColor.a = 1;
            return blendColor;
        }

        // 颜色混合- 带 a 通道的颜色混合
        public static Color NormalBlend2(Color background, Color cover)
        {
            float CoverAlpha = cover.a;
            Color blendColor;
            blendColor.a = cover.a + background.a * (1 - cover.a);
            blendColor.r = (cover.r * cover.a + background.r * background.a * (1 - cover.a)) / (blendColor.a);
            blendColor.g = (cover.g * cover.a + background.g * background.a * (1 - cover.a)) / (blendColor.a);
            blendColor.b = (cover.b * cover.a + background.b * background.a * (1 - cover.a)) / (blendColor.a);
            return blendColor;
        }

    }
}