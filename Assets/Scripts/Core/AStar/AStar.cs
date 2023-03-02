using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum GridType
{
    normal,
    obstacle
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
        //曼哈顿算法
        var disX = Math.Abs(grid.x - end.x);
        var disY = Math.Abs(grid.y - end.y);
        return disX + disY;
    }

    private float GetGridG(Grid grid, Grid parent)
    {
        if (parent == null)
            return 0;
        return (float)Math.Abs(Math.Sqrt(Math.Pow(grid.x - parent.x, 2) + Math.Pow(grid.y - parent.y, 2))) + parent.g;
    }

    private int[,] _rounds = new int[8, 2] { { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };
    private List<Grid> GetRoundGrids(Grid grid, List<Grid> closeList)
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
                if (point != null && point.type == GridType.normal && closeList.IndexOf(point) < 0)
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
            var roundGrids = GetRoundGrids(minGrid, closeList);
            for (int i = 0; i < roundGrids.Count; i++)
            {
                var grid = roundGrids[i];
                if (openList.IndexOf(grid) >= 0)
                {
                    var newG = GetGridG(grid, minGrid);
                    //Debug.Log("计算新的g="+newG +",老的g="+grid.g+ ",格子x=" + grid.x + ",y=" + grid.y + ",当前格子x=" + minGrid.x + ",y=" + minGrid.y);
                    if (newG < grid.g)
                    {
                        //Debug.Log("设置为新的");
                        grid.parent = minGrid;
                        grid.g = newG;
                        grid.f = grid.g + grid.h;
                    }
                }
                else
                {
                    grid.parent = minGrid;
                    grid.g = GetGridG(grid, grid.parent);
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
        while (g != start && g.parent != null)
        {
            pathList.Add(g);
            g = g.parent;
        }

        return pathList;
    }
}