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
        public int level;
        //当前节点内的对象
        public List<RectTransform> objList;
        //当前节点的范围
        public Rect bound;
        //当前节点的子节点
        public List<QuadTree> childList;
        public QuadTree(Rect rect, int level)
        {
            this.level = level;
            this.bound = rect;
            this.objList = new List<RectTransform>();
            this.childList = new List<QuadTree>();
        }
        /// <summary>
        /// 插入矩形
        /// </summary>
        /// <param name="rectTran"></param>
        public void Insert(RectTransform rectTran)
        {
            //该节点下有子节点，则检查是否匹配子节点
            if (this.childList.Count > 0)
            {
                var indexs = this.GetIndex(rectTran);
                foreach (var k in indexs)
                {
                    this.childList[k].Insert(rectTran);
                }
            }
            else
            {
                //否则放在当前节点下
                this.objList.Add(rectTran);

                //如果达到了划分条件，则开始划分并将节点放入对应象限
                if (this.childList.Count == 0 && this.objList.Count > this.MAX_OBJECT && this.level < this.MAX_LEVEL)
                {
                    this.Split();

                    for (var i = this.objList.Count - 1; i >= 0; i--)
                    {
                        var rt = this.objList[i];
                        this.objList.Remove(rt);
                        var indexs = this.GetIndex(rt);
                        foreach (var k in indexs)
                        {
                            this.childList[k].Insert(rt);
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
            this.objList.Clear();
            //this.objList = null;

            foreach (var k in this.childList)
            {
                k.Clear();
            }
            this.childList.Clear();
        }
        //获取所在象限
        private List<int> GetIndex(RectTransform rectTran)
        {
            //左下角(0,0)
            var indexList = new List<int>();
            var cx = this.bound.center.x;
            var cy = this.bound.center.y;

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
            var level = this.level + 1;
            var x = this.bound.x;
            var y = this.bound.y;
            var cx = this.bound.center.x;
            var cy = this.bound.center.y;
            var width = bound.width / 2;
            var height = bound.height / 2;

            this.childList.Add(new QuadTree(new Rect(cx, cy, width, height), level));
            this.childList.Add(new QuadTree(new Rect(x, cy, width, height), level));
            this.childList.Add(new QuadTree(new Rect(x, y, width, height), level));
            this.childList.Add(new QuadTree(new Rect(cx, y, width, height), level));
        }
        /// <summary>
        /// 获取碰撞到的目标
        /// </summary>
        /// <param name="rectTran"></param>
        /// <returns></returns>
        public List<RectTransform> Retrieve(RectTransform rectTran)
        {
            var rectTrans = new List<RectTransform>();
            if (this.childList.Count > 0)
            {
                var indexs = this.GetIndex(rectTran);
                foreach (var k in indexs)
                {
                    //result.AddRange(this.childList[k].Retrieve(rectTran));
                    var temp = this.childList[k].Retrieve(rectTran);
                    foreach(var j in temp)
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
                rectTrans.AddRange(this.objList);
            }
            var rect = this.GetRect(rectTran);
            var result = new List<RectTransform>();
            foreach(var rt in rectTrans)
            {
                if(this.RectCollision(this.GetRect(rt), rect))
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
            foreach (var c in objList)
            {
                Gizmos.DrawWireCube(this.GetRect(c).center, c.rect.size);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bound.center, bound.size);
            if (childList == null) return;
            foreach (var child in childList)
            {
                child.DrawLine();
            }
        }
    }
}