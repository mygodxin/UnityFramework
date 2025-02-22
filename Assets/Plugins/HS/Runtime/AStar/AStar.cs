using System;
using System.Collections.Generic;

namespace HS
{
    /// <summary>
    /// 格子类型
    /// </summary>
    public enum GridType
    {
        Normal,
        Obstacle
    }
    /// <summary>
    /// 地图格子
    /// </summary> 
    public class Grid
    {
        public GridType Type;
        public int X;
        public int Y;
        public float F;
        public float G;
        public float H;
        public Grid Parent;
    }
    /// <summary>
    /// A星寻路
    /// </summary>
    public class AStar
    {
        private readonly int[,] Rounds = new int[8, 2] { { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };
        private Grid[,] map;
        private int mapWidth;
        private int mapHeight;

        public AStar()
        {

        }

        public void CreateMap(Grid[,] map)
        {
            this.map = map;
            mapWidth = this.map.GetLength(0);
            mapHeight = this.map.GetLength(1);
        }

        private Grid GetGridMinF(List<Grid> openList)
        {
            var minGrid = openList[0];
            var minF = minGrid.F;
            for (int i = 0; i < openList.Count; i++)
            {
                var grid = openList[i];
                if (grid.F < minF)
                {
                    minGrid = grid;
                    minF = grid.F;
                }
            }
            return minGrid;
        }

        private int GetGridH(Grid grid, Grid end)
        {
            //曼哈顿算法
            var disX = Math.Abs(grid.X - end.X);
            var disY = Math.Abs(grid.Y - end.Y);
            return disX + disY;
        }

        private float GetGridG(Grid grid, Grid parent)
        {
            if (parent == null)
                return 0;
            return (float)Math.Abs(Math.Sqrt(Math.Pow(grid.X - parent.X, 2) + Math.Pow(grid.Y - parent.Y, 2))) + parent.G;
        }

        private List<Grid> GetRoundGrids(Grid grid, List<Grid> closeList)
        {
            var roundGrids = new List<Grid>();
            int row = Rounds.GetLength(0);
            int col = Rounds.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                var offsetX = Rounds[i, 0];
                var offsetY = Rounds[i, 1];
                var x = grid.X + offsetX;
                var y = grid.Y + offsetY;
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    var point = map[x, y];
                    if (point != null && point.Type == GridType.Normal && closeList.IndexOf(point) < 0)
                    {
                        roundGrids.Add(point);
                    }
                }
            }
            return roundGrids;
        }


        public List<Grid> FindPath(Grid start, Grid end)
        {
            var pathList = new List<Grid>();
            //起始点与目标点一致直接返回
            if (start == end)
            {
                return pathList;
            }
            var openList = new List<Grid>();
            var closeList = new List<Grid>();
            openList.Add(start);
            while (openList.Count > 0)
            {
                var minGrid = GetGridMinF(openList);
                if (minGrid == end)
                {
                    break;
                }

                //遍历当前格子周围节点,默认8方向
                var roundGrids = GetRoundGrids(minGrid, closeList);
                for (int i = 0; i < roundGrids.Count; i++)
                {
                    var grid = roundGrids[i];
                    if (openList.IndexOf(grid) >= 0)
                    {
                        var newG = GetGridG(grid, minGrid);
                        //Debug.Log("计算新的g="+newG +",老的g="+grid.G+ ",格子x=" + grid.X + ",Y=" + grid.Y + ",当前格子x=" + minGrid.X + ",Y=" + minGrid.Y);
                        if (newG < grid.G)
                        {
                            //Debug.Log("设置为新的");
                            grid.Parent = minGrid;
                            grid.G = newG;
                            grid.F = grid.G + grid.H;
                        }
                    }
                    else
                    {
                        grid.Parent = minGrid;
                        grid.G = GetGridG(grid, grid.Parent);
                        grid.H = GetGridH(grid, end);
                        grid.F = grid.G + grid.H;
                        openList.Add(grid);
                    }
                }

                closeList.Add(minGrid);
                openList.Remove(minGrid);
            }

            //构建路径
            var g = end;
            while (g != start && g.Parent != null)
            {
                pathList.Add(g);
                g = g.Parent;
            }

            return pathList;
        }
    }
}