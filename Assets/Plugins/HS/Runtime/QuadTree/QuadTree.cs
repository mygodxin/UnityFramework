using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    /// <summary>
    /// 四叉树碰撞检测
    /// </summary>
    public class QuadTree
    {
        //节点内最大对象数,当节点内对象大于这个数则生成子树
        private readonly int MAX_OBJECT = 4;
        //最大层级
        private readonly int MAX_LEVEL = 5;
        //层级
        public int Level;
        //当前节点内的对象
        public List<RectTransform> ObjectList;
        //当前节点的范围
        public Rect Bound;
        //当前节点的子节点
        public List<QuadTree> ChildList;
        public QuadTree(Rect rect, int level)
        {
            this.Level = level;
            this.Bound = rect;
            this.ObjectList = new List<RectTransform>();
            this.ChildList = new List<QuadTree>();
        }
        /// <summary>
        /// 插入矩形
        /// </summary>
        /// <param name="rectTran"></param>
        public void Insert(RectTransform rectTran)
        {
            //该节点下有子节点，则检查是否匹配子节点
            if (this.ChildList.Count > 0)
            {
                var indexs = this.GetIndex(rectTran);
                foreach (var k in indexs)
                {
                    this.ChildList[k].Insert(rectTran);
                }
            }
            else
            {
                //否则放在当前节点下
                this.ObjectList.Add(rectTran);

                //如果达到了划分条件，则开始划分并将节点放入对应象限
                if (this.ChildList.Count == 0 && this.ObjectList.Count > this.MAX_OBJECT && this.Level < this.MAX_LEVEL)
                {
                    this.Split();

                    for (var i = this.ObjectList.Count - 1; i >= 0; i--)
                    {
                        var rt = this.ObjectList[i];
                        this.ObjectList.Remove(rt);
                        var indexs = this.GetIndex(rt);
                        foreach (var k in indexs)
                        {
                            this.ChildList[k].Insert(rt);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            this.ObjectList.Clear();
            //this.ObjectList = null;

            foreach (var k in this.ChildList)
            {
                k.Clear();
            }
            this.ChildList.Clear();
        }
        //获取所在象限
        private List<int> GetIndex(RectTransform rectTran)
        {
            //左下角(0,0)
            var indexList = new List<int>();
            var cx = this.Bound.center.x;
            var cy = this.Bound.center.y;

            var rect = this.GetRect(rectTran);
            var top = rect.y + rect.height >= cy;
            var bottom = rect.y <= cy;
            var left = rect.x <= cx;
            var right = rect.x + rect.width >= cx;
            if (top && right)
                indexList.Add(0);
            if (top && left)
                indexList.Add(1);
            if (bottom && left)
                indexList.Add(2);
            if (bottom && right)
                indexList.Add(3);
            return indexList;
        }
        private void Split()
        {
            var level = this.Level + 1;
            var x = this.Bound.x;
            var y = this.Bound.y;
            var cx = this.Bound.center.x;
            var cy = this.Bound.center.y;
            var width = Bound.width / 2;
            var height = Bound.height / 2;

            this.ChildList.Add(new QuadTree(new Rect(cx, cy, width, height), level));
            this.ChildList.Add(new QuadTree(new Rect(x, cy, width, height), level));
            this.ChildList.Add(new QuadTree(new Rect(x, y, width, height), level));
            this.ChildList.Add(new QuadTree(new Rect(cx, y, width, height), level));
        }
        /// <summary>
        /// 获取碰撞到的目标
        /// </summary>
        /// <param name="rectTran"></param>
        /// <returns></returns>
        public List<RectTransform> Retrieve(RectTransform rectTran)
        {
            var rectTrans = new List<RectTransform>();
            if (this.ChildList.Count > 0)
            {
                var indexs = this.GetIndex(rectTran);
                foreach (var k in indexs)
                {
                    //result.AddRange(this.ChildList[k].Retrieve(rectTran));
                    var temp = this.ChildList[k].Retrieve(rectTran);
                    foreach (var j in temp)
                    {
                        if (rectTrans.IndexOf(j) < 0)
                        {
                            rectTrans.Add(j);
                        }
                    }
                }
            }
            else
            {
                rectTrans.AddRange(this.ObjectList);
            }
            var rect = this.GetRect(rectTran);
            var result = new List<RectTransform>();
            foreach (var rt in rectTrans)
            {
                if (this.RectCollision(this.GetRect(rt), rect))
                {
                    result.Add(rt);
                }

            }
            return result;
        }
        /// <summary>
        /// 矩形碰撞检测
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public bool RectCollision(Rect rect1, Rect rect2)
        {
            float minx = Mathf.Max(rect1.x, rect2.x);
            float miny = Mathf.Max(rect1.y, rect2.y);
            float maxx = Mathf.Min(rect1.x + rect1.width, rect2.x + rect2.width);
            float maxy = Mathf.Min(rect1.y + rect1.height, rect2.y + rect2.height);
            if (minx > maxx || miny > maxy) return false;
            return true;
        }
        private Rect GetRect(RectTransform rectTran)
        {
            var rect = rectTran.rect;
            return new Rect(rectTran.position.x + rect.x, rectTran.position.y + rect.y, rect.width, rect.height);
        }
        /// <summary>
        /// 绘制测试线
        /// </summary>
        public void DrawLine()
        {
            Gizmos.color = Color.green;
            foreach (var c in ObjectList)
            {
                Gizmos.DrawWireCube(this.GetRect(c).center, c.rect.size);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Bound.center, Bound.size);
            if (ChildList == null) return;
            foreach (var child in ChildList)
            {
                child.DrawLine();
            }
        }
    }
}