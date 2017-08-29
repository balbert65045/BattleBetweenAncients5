using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    public int MaxWidth = 7;
    public int MaxHeight = 7;
    private Node[,] nodes;
    private Node startNode;
    private Node endNode;
    private List<Node> OpenNodes;
    private List<Node> ClosedNodes;



    public List<Node> FindPath(Node Begin, Node End, Node[,] Map)
    {
        startNode = Begin;
        endNode = End;
        nodes = Map;
        foreach (Node node in nodes)
        {
            node.CalculateG(startNode.Location);
            node.CalculateH(endNode.Location);
            node.State = NodeState.Untested;
            node.ParentNode = null;

            if (node.GetComponent<EnviromentTile>().cardType == CardType.Open)
            {
                node.IsWalkable = true;
            }else
            {
                node.IsWalkable = false;
            }
            
        }
        OpenNodes = new List<Node>();
        ClosedNodes = new List<Node>();
        List<Node> path = new List<Node>();
        OpenNodes.Add(startNode);
        bool success = Search(startNode);
        Debug.Log(success);
        if (success)
        {
            Node node = this.endNode;
            while (node.ParentNode != null)
            {
                path.Add(node);
                node = node.ParentNode;
            }
            path.Reverse();
        }
        return path;
    }



    private bool Search(Node currentNode)
    {
        currentNode.State = NodeState.Closed;
        OpenNodes.Remove(currentNode);
        ClosedNodes.Add(currentNode);

        OpenNodes.AddRange(GetAdjacentWalkableNodes(currentNode));

        if (OpenNodes.Count > 0)
        {
            Node minFNode = OpenNodes[0];
            foreach (var nextNode in OpenNodes)
            {
                if (nextNode.F < minFNode.F)
                {
                    minFNode = nextNode;
                }
            }


            if (minFNode.Location == this.endNode.Location)
            {
                return true;
            }
            else
            {
                if (Search(minFNode)) // Note: Recurses back into Search(Node)
                    return true;
            }
        }
        return false;
    }




    //private bool Search(Node currentNode)
    //{
    //    currentNode.State = NodeState.Closed;
    //    List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
        
    //    nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
    //    foreach (var nextNode in nextNodes)
    //    {

    //        if (nextNode.Location == this.endNode.Location)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            if (Search(nextNode)) // Note: Recurses back into Search(Node)
    //                return true;
    //        }
    //    }
    //    return false;
    //}


    private Point[] GetAdjacentLocations(Point location)
    {
        Point[] AdjacentPoints = new Point[4];
        AdjacentPoints[0] = new Point(location.X, location.Y + 1);
        AdjacentPoints[1] = new Point(location.X, location.Y - 1);
        AdjacentPoints[2] = new Point(location.X + 1, location.Y);
        AdjacentPoints[3] = new Point(location.X - 1, location.Y);

        return (AdjacentPoints);
    }

    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in nextLocations)
        {
            int x = location.X;
            int y = location.Y;

            // Stay within the grid's boundaries
            if (x < 0 || x >= this.MaxWidth || y < 0 || y >= this.MaxHeight)
                continue;

            Node node = this.nodes[x, y];
            // Ignore non-walkable nodes
            if (!node.IsWalkable)
                continue;

            // Ignore already-closed nodes
            if (node.State == NodeState.Closed)
                continue;

            // Already-open nodes are only added to the list if their G-value is lower going via this route.
            if (node.State == NodeState.Open)
            {
                float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                float gTemp = fromNode.G + traversalCost;
                if (gTemp < node.G)
                {
                    node.ParentNode = fromNode;
                    walkableNodes.Add(node);
                }
            }
            else
            {
                // If it's untested, set the parent and flag it as 'Open' for consideration
                node.ParentNode = fromNode;
                node.State = NodeState.Open;
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }


}
