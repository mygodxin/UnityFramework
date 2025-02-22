using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 连线
    /// </summary>
    public class GLine : MonoBehaviour
    {
        public Image Current;
        public Image Target;
        public Image ConnectionLine; // 用于绘制连接线的UI Image
        public float LineWidth = 2f; // 连接线的宽度

        private void Start()
        {
            // 设置连接线的颜色和宽度
            ConnectionLine.color = Color.yellow;
            ConnectionLine.rectTransform.sizeDelta = new Vector2(LineWidth, 0f);

            // 确保点的初始位置
            UpdateLinePositions();
        }

        private void Update()
        {
            // 更新连线的位置
            UpdateLinePositions();
        }

        private void UpdateLinePositions()
        {
            // 计算当前和治疗目标之间的距离和角度
            Vector3 angelPosition = Current.rectTransform.position;
            Vector3 targetPosition = Target.rectTransform.position;
            float distance = Vector3.Distance(angelPosition, targetPosition);
            float angle = Mathf.Atan2(targetPosition.y - angelPosition.y, targetPosition.x - angelPosition.x) * Mathf.Rad2Deg;

            // 更新连接线的位置和角度
            ConnectionLine.rectTransform.sizeDelta = new Vector2(distance, LineWidth);
            ConnectionLine.rectTransform.pivot = new Vector2(0f, 1f);
            ConnectionLine.rectTransform.position = (angelPosition + targetPosition) / 2f;
            ConnectionLine.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
