using UnityEngine;
using UnityEngine.UI;

namespace HS
{
    /// <summary>
    /// 围绕目标旋转组件
    /// </summary>
    public class GRotateAround : MonoBehaviour
    {
        public RectTransform Target; // 旋转的目标 UI 组件
        public float RotationSpeed = 10f; // 旋转速度

        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Target == null)
            {
                throw new System.Exception("use GRotateAround must be set Target.");
            }
            // 获取旋转中心点的世界坐标
            Vector3 targetWorldPos = Target.position;

            // 将世界坐标转换为本地坐标
            Vector3 targetLocalPos = rectTransform.InverseTransformPoint(targetWorldPos);

            // 计算旋转角度（根据 RotationSpeed 和 Time.deltaTime）
            float rotationAngle = RotationSpeed * Time.deltaTime;

            // 绕 Y 轴进行旋转
            rectTransform.RotateAround(targetLocalPos, Vector3.up, rotationAngle);
        }
    }
}
