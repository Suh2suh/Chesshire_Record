using UnityEngine;

public class Node
{
    public Vector2Int Grid { get; }
    public Node ParentNode { get; set; }
    public int G { get; set; }
    public int H { get; set; }

    public Node(int x, int y)
    {
        Grid = new Vector2Int(x, y);
    }

    public int F { get => G + H; }



    public override string ToString()
    {
        return Grid.ToString();
    }
}
