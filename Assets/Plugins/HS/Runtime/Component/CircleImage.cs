using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 圆形Image组件
    /// </summary>
    [AddComponentMenu("UI/CircleImage", 11)]
    public class CircleImage : MaskableGraphic, ICanvasRaycastFilter
    {
        /// <summary>
            /// 渲染类型
            /// </summary>
        public enum RenderType
        {
            Simple,
            Filled,
        }

        /// <summary>
            /// 填充类型
            /// </summary>
        public enum FilledType
        {
            Radial360,
        }

        /// <summary>
            /// 绘制起始点(填充类型-360度)
            /// </summary>
        public enum Origin360
        {
            Right,
            Top,
            Left,
            Bottom,
        }

        //Sprite图片
        [SerializeField]
        Sprite m_Sprite;
        public Sprite Sprite
        {
            get { return m_Sprite; }
            set
            {
                m_Sprite = value;
                // 模拟OnEnable逻辑
                //this.OnEnable();

                // 强制刷新
                this.SetAllDirty();

                // 模拟OnDisable逻辑
                //this.OnDisable();
            }
        }

        //贴图
        public override Texture mainTexture
        {
            get
            {
                if (m_Sprite == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return m_Sprite.texture;
            }
        }

        //渲染类型
        [SerializeField]
        RenderType m_RenderType;

        //填充类型
        [SerializeField]
        FilledType m_FilledType;

        //绘制起始点(填充类型-360度)
        [SerializeField]
        Origin360 m_Origin360;

        //是否为顺时针绘制
        [SerializeField]
        bool m_Clockwise;

        //填充度
        [SerializeField]
        [Range(0, 1)]
        float m_FillAmount;

        //多少个三角面组成
        [SerializeField]
        int segements = 100;

        List<Vector3> vertexCache = new List<Vector3>();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            vertexCache.Clear();

            switch (m_RenderType)
            {
                case RenderType.Simple:
                    GenerateSimpleSprite(vh);
                    break;
                case RenderType.Filled:
                    GenerateFilledSprite(vh);
                    break;
            }
        }

        void GenerateSimpleSprite(VertexHelper vh)
        {
            Vector4 uv = m_Sprite == null
              ? Vector4.zero
              : DataUtility.GetOuterUV(m_Sprite);
            float uvWidth = uv.z - uv.x;
            float uvHeight = uv.w - uv.y;
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            float dia = width > height ? width : height;
            float r = dia * 0.5f;
            Vector2 uvCenter = new Vector2((uv.x + uv.z) * 0.5f, (uv.y + uv.w) * 0.5f);
            Vector3 posCenter = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);
            float uvScaleX = uvWidth / width;
            float uvScaleY = uvHeight / height;
            float deltaRad = 2 * Mathf.PI / segements;

            float curRad = 0;
            int vertexCount = segements + 1;
            vh.AddVert(posCenter, color, uvCenter);
            for (int i = 0; i < vertexCount - 1; i++)
            {
                UIVertex vertex = new UIVertex();
                Vector3 posOffset = new Vector3(r * Mathf.Cos(curRad), r * Mathf.Sin(curRad));
                vertex.position = posCenter + posOffset;
                vertex.color = color;
                vertex.uv0 = new Vector2(uvCenter.x + posOffset.x * uvScaleX, uvCenter.y + posOffset.y * uvScaleY);
                vh.AddVert(vertex);
                vertexCache.Add(vertex.position);

                curRad += deltaRad;
            }

            for (int i = 0; i < vertexCount - 2; i++)
            {
                vh.AddTriangle(0, i + 1, i + 2);
            }
            vh.AddTriangle(0, segements, 1);
        }

        void GenerateFilledSprite(VertexHelper vh)
        {
            Vector4 uv = m_Sprite == null
              ? Vector4.zero
              : DataUtility.GetOuterUV(m_Sprite);
            float uvWidth = uv.z - uv.x;
            float uvHeight = uv.w - uv.y;
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            float dia = width > height ? width : height;
            float r = dia * 0.5f;
            Vector2 uvCenter = new Vector2((uv.x + uv.z) * 0.5f, (uv.y + uv.w) * 0.5f);
            Vector3 posCenter = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);
            float uvScaleX = uvWidth / width;
            float uvScaleY = uvHeight / height;
            float deltaRad = 2 * Mathf.PI / segements;

            switch (m_FilledType)
            {
                case FilledType.Radial360:
                    float quarterRad = 2 * Mathf.PI * 0.25f;
                    float curRad = quarterRad * (int)m_Origin360;
                    int vertexCount = m_FillAmount == 1
                      ? segements + 1
                      : Mathf.RoundToInt(segements * m_FillAmount) + 2;
                    vh.AddVert(posCenter, color, uvCenter);
                    for (int i = 0; i < vertexCount - 1; i++)
                    {
                        UIVertex vertex = new UIVertex();
                        Vector3 posOffset = new Vector3(r * Mathf.Cos(curRad), r * Mathf.Sin(curRad));
                        vertex.position = posCenter + posOffset;
                        vertex.color = color;
                        vertex.uv0 = new Vector2(uvCenter.x + posOffset.x * uvScaleX, uvCenter.y + posOffset.y * uvScaleY);
                        vh.AddVert(vertex);
                        vertexCache.Add(vertex.position);

                        curRad += m_Clockwise ? -deltaRad : deltaRad;
                    }

                    for (int i = 0; i < vertexCount - 2; i++)
                    {
                        vh.AddTriangle(0, i + 1, i + 2);
                    }
                    if (m_FillAmount == 1)
                    {
                        vh.AddTriangle(0, segements, 1);
                    }
                    break;
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Vector2 localPos;
            int crossPointCount;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPos);
            RayCrossing(localPos, out crossPointCount);
            return crossPointCount % 2 != 0;
        }

        public void RayCrossing(Vector2 localPos, out int crossPointCount)
        {
            crossPointCount = 0;
            for (int i = 0; i < vertexCache.Count; i++)
            {
                Vector3 p1 = vertexCache[i];
                Vector3 p2 = vertexCache[(i + 1) % vertexCache.Count];

                if (p1.y == p2.y) continue;
                if (localPos.y <= Mathf.Min(p1.y, p2.y)) continue;
                if (localPos.y >= Mathf.Max(p1.y, p2.y)) continue;
                float crossX = (localPos.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y) + p1.x;
                if (crossX >= localPos.x)
                {
                    crossPointCount++;
                }
            }
        }
    }
}