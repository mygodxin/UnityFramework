using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

/// <summary>
/// 格子类型
/// </summary>
public enum GridType
{
    normal,

}
/// <summary>
/// 地图格子
/// </summary>
public class Grid
{
    public GridType type;
    public int x;
    public int y;
    public float f;
    public float g;
    public float h;
    public Grid parent;

    public Vector2 pos;
}
/// <summary>
/// A星寻路
/// </summary>
public class AStar
{
    private Grid[,] _map;
    private int _mapWidth;
    private int _mapHeight;
    public AStar()
    {

    }

    public void CreateMap(Grid[,] map)
    {
        _map = map;
        _mapWidth = _map.GetLength(0);
        _mapHeight = _map.GetLength(1);
    }

    private Grid getGridMinF(List<Grid> openList)
    {
        var minGrid = openList[0];
        var minF = minGrid.f;
        for (int i = 0; i < openList.Count; i++)
        {
            var grid = openList[i];
            if (grid.f < minF)
            {
                minGrid = grid;
                minF = grid.f;
            }
        }
        return minGrid;
    }

    private int GetGridH(Grid grid, Grid end)
    {
        var disX = Math.Abs(grid.x - end.x);
        var disY = Math.Abs(grid.y - end.y);
        return disX + disY;
    }

    private float GetGridG(Grid grid, Grid parent)
    {
        return (float)Math.Abs(Math.Sqrt(Math.Pow(grid.x - parent.x, 2) + Math.Pow(grid.y - parent.y, 2))) + parent.g;
    }

    private int[,] _rounds = new int[8, 2] { { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };
    private List<Grid> GetRoundGrids(Grid grid)
    {
        var roundGrids = new List<Grid>();
        int row = _rounds.GetLength(0);
        int col = _rounds.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            var offsetX = _rounds[i, 0];
            var offsetY = _rounds[i, 1];
            var x = grid.x + offsetX;
            var y = grid.y + offsetY;
            if (x >= 0 && x < _mapWidth && y >= 0 && y < _mapHeight)
            {
                var point = _map[x, y];
                if (point != null && point.type == GridType.normal)
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
            var minGrid = getGridMinF(openList);
            if (minGrid == end)
            {
                break;
            }

            //遍历当前格子周围节点,默认8方向
            var roundGrids = GetRoundGrids(minGrid);
            for (int i = 0; i < roundGrids.Count; i++)
            {
                var grid = roundGrids[i];
                if (openList.IndexOf(grid) >= 0)
                {
                    var gg = GetGridG(grid, grid.parent);
                    if (gg < grid.g)
                    {
                        grid.parent = minGrid;
                        grid.g =gg;
                        grid.f = minGrid.g + minGrid.h;
                    }
                }
                else
                {
                    grid.parent = minGrid;
                    grid.g = GetGridG(grid, minGrid);
                    grid.h = GetGridH(grid, end);
                    grid.f = grid.g + grid.h;
                    openList.Add(grid);
                }
            }

            closeList.Add(minGrid);
            openList.Remove(minGrid);
        }

        //构建路径
        var g = end;
        while (g.parent != start)
        {
            pathList.Add(g);
            g = g.parent;
        }

        return pathList;
    }
}