using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    /// <summary>
    /// 对象池组 (警告:gameObject.name作为对象池key,外部请勿修改)
    /// </summary>
    public class ObjectPool
    {
        private Dictionary<string, Queue<RectTransform>> pool;
        private List<RectTransform> rectList;

        public ObjectPool()
        {
            pool = new Dictionary<string, Queue<RectTransform>>();
        }

        public RectTransform Get(RectTransform rect)
        {
            if (pool.TryGetValue(rect.name, out var arr) && arr.Count > 0)
            {
                var go = arr.Dequeue();
                go.gameObject.SetActive(true);
                return go;
            }
            var obj = Object.Instantiate(rect);
            //去除名字中的(Clone)
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", string.Empty);
            return obj;
        }

        public void Release(RectTransform go)
        {
            if (!pool.TryGetValue(go.name, out var arr))
            {
                arr = new Queue<RectTransform>();
                pool.Add(go.name, arr);
            }
            go.gameObject.SetActive(false);
            arr.Enqueue(go);
        }

        public void Clear()
        {
            pool.Clear();
        }
    }
}
