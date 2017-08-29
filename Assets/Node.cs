using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Node : MonoBehaviour
{

    public Point Location;
    public bool IsWalkable;
    public float G;
    public float H;
    public float F { get { return this.G + this.H; } }
    public NodeState State;
    public Node ParentNode;


    //public Point Location { get; set; }
    //public bool IsWalkable { get; set; }
    //public float G { get; private set; }
    //public float H { get; private set; }
    //public float F { get { return this.G + this.H; } }
    //public NodeState State { get; set; }
    //public Node ParentNode { get; set; }


    EnviromentTile Tile;

    public static float GetTraversalCost(Point startLocation, Point endLocation)
    {
        return 1;
    }



    public void CalculateG(Point StartPoint)
    {
        G = Mathf.Abs(StartPoint.X - this.Location.X) + Mathf.Abs(StartPoint.Y - this.Location.Y);
    }

    public void CalculateH(Point EndPoint)
    {
        H = Mathf.Abs(EndPoint.X - this.Location.X) + Mathf.Abs(EndPoint.Y - this.Location.Y);
    }

    private void Start()
    {
        Tile = GetComponent<EnviromentTile>();
        Location = new Point(Tile.X, Tile.Z);
    }


}

public enum NodeState { Untested, Open, Closed }
