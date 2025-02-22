using System.Runtime.CompilerServices;
using UnityEngine;

namespace HS
{
    public static class TransformExtend
    {
        /// <summary>
        /// 查找指定路径的组件,Game/Center/GameComp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T FindComponent<T>(this Transform transform, string path) where T : Component
        {
            string[] pathParts = path.Split('/'); // 拆分路径字符串

            // 从当前 Transform 开始逐级查找组件
            Transform current = transform;
            foreach (string part in pathParts)
            {
                current = current.Find(part); // 通过名字查找子 Transform
                if (current == null)
                {
                    Debug.LogWarning($"Transform '{part}' not found in Path '{path}'.");
                    return null;
                }
            }

            T component = current.GetComponent<T>(); // 获取指定类型的组件
            if (component == null)
            {
                Debug.LogWarning($"Component of Type '{typeof(T).Name}' not found in Path '{path}'.");
            }

            return component;
        }

        /// <summary>
        /// // 清空子节点
        /// </summary>
        public static void RemoveChildren(this Transform transform)
        {
            int childcount = transform.childCount;
            for (int i = 0; i < childcount; i++)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        /// <summary>
        /// 查找子物体
        /// </summary>
        /// <param name="current">当前变换组件</param>
        /// <param name="childName">查找的物体名称</param>
        /// <returns></returns>
        public static Transform GetChildWithName(this Transform current, string childName)
        {
            //在子物体中查找
            var transform = current;
            Transform childTf = transform.Find(childName);
            if (childTf != null) return childTf;

            for (int i = 0; i < transform.childCount; i++)
            {
                //将任务移交给子物体
                childTf = GetChildWithName(transform.GetChild(i), childName);
                if (childTf != null) return childTf;
            }
            return null;
        }
    }
}