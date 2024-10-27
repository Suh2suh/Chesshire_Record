using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptimalPathCalculator
{

    public static List<Node> FindPath(Vector2Int startGrid, Vector2Int endGrid, int gridWH,
                                                Func<Vector2Int, bool> startingPositionCondition, Func<Vector2Int, bool> passingConditionThrough)
	{
        return FindPath(startGrid, endGrid, gridWH, gridWH, startingPositionCondition, passingConditionThrough);
    }

    public static List<Node> FindPath(Vector2Int startGrid, Vector2Int endGrid, int gridW, int gridH,
                                                    Func<Vector2Int, bool> startingPositionCondition, Func<Vector2Int, bool> passingConditionThrough)
    {
        List<Node> FinalNodeList = new List<Node>();
        Node[,] NodeArray = new Node[gridW, gridH];
        for (int w = 0; w < gridW; w++)
            for (int h = 0; h < gridH; h++)
                NodeArray[w, h] = new Node(w, h);

        Node StartNode = NodeArray[startGrid.x, startGrid.y];
        Node TargetNode = NodeArray[endGrid.x, endGrid.y];
        Node CurrentNode;
        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();

        if (startingPositionCondition.Invoke(StartNode.Grid))
            OpenList.Add(StartNode);


        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurrentNode.F && OpenList[i].H < CurrentNode.H)
                    CurrentNode = OpenList[i];
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                Node TargetCurrentNode = TargetNode;
                while (TargetCurrentNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurrentNode);
                    TargetCurrentNode = TargetCurrentNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                return FinalNodeList;
            }

            OpenListAdd(CurrentNode.Grid + Vector2Int.up);
            OpenListAdd(CurrentNode.Grid + Vector2Int.right);
            OpenListAdd(CurrentNode.Grid + Vector2Int.down);
            OpenListAdd(CurrentNode.Grid + Vector2Int.left);
        }
        return null;


        void OpenListAdd(Vector2Int grid)
        {
            if (passingConditionThrough.Invoke(grid) && ClosedList.Contains(NodeArray[grid.x, grid.y]) == false)
            {
                Node NeighborNode = NodeArray[grid.x, grid.y];
                int MoveCost = CurrentNode.G + (Mathf.Sqrt(Mathf.Pow((grid - CurrentNode.Grid).x, 2f) + Mathf.Pow((grid - CurrentNode.Grid).y, 2f)) == 1 ? 10 : 14);

                if (MoveCost < NeighborNode.G || OpenList.Contains(NeighborNode) == false)
                {
                    NeighborNode.G = MoveCost;
                    NeighborNode.H = (Mathf.Abs(NeighborNode.Grid.x - TargetNode.Grid.x) + Mathf.Abs(NeighborNode.Grid.y - TargetNode.Grid.y)) * 10;
                    NeighborNode.ParentNode = CurrentNode;

                    OpenList.Add(NeighborNode);

                    //Debug.Log(grid + ": success");
                }
            }
        }
    }


}