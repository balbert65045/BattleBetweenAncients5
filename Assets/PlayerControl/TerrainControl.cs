using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControl : MonoBehaviour {


    //Grid size (Note: may want to change to automatically update with new map)
    public int xGridLength = 10;
    public int zGridLength = 10;

    private EnviromentTile[] TilesTotal;
    public EnviromentTile[,] GridTiles;
    private Node[,] GridNodes;
   

    private AStar astar;



    public List<EnviromentTile> FindTilesOpenAround(EnviromentTile CurrentTile)
    {
        // x x x
        // x 0 x
        // x x x
        int Xhigh = Mathf.Clamp(CurrentTile.X + 1, 0, xGridLength - 1);
        int Xlow = Mathf.Clamp(CurrentTile.X - 1, 0, xGridLength - 1);
        int Zhigh = Mathf.Clamp(CurrentTile.Z + 1, 0, zGridLength - 1);
        int Zlow = Mathf.Clamp(CurrentTile.Z - 1, 0, zGridLength - 1);
        List<EnviromentTile> TilesAround = new List<EnviromentTile>();

        for (int i = Xlow; i <= Xhigh; i++)
        {
            for (int j = Zlow; j <= Zhigh; j++)
            {
                if (GridTiles[i, j].cardType == CardType.Open)
                {
                    TilesAround.Add(GridTiles[i, j]);
                }
            }
        }

        return TilesAround;
    }

    //Returns the position of the Tile in the path 
    //If not in path returns -1
    public int CheckPathPosition(EnviromentTile CurrentTile, List<EnviromentTile> path)
    {
        for (int i = 0; i< path.Count; i++)
        {
            if (path[i] == CurrentTile)
            {
                return i;
            }
        }
        return -1;
    }

    //Returns the tile next in path
    public Transform FindNextTileInPath(EnviromentTile CurrentTile, List<EnviromentTile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] == CurrentTile)
            {
                return path[i + 1].transform;
            }
        }
        return null;
    }

    // TODO MOVE this to player controller or return a 
    public List<EnviromentTile> FindAttackRange(EnviromentTile TileStart, int MaxDistance)
    {
       
        int layer = MaxDistance;
        List<EnviromentTile> TileRange = new List<EnviromentTile>();
        while (layer > 0)
        {
            for (int j = -layer; j <= layer; j++)
            {
                int i = Mathf.Abs(j) - layer;
                int Y = Mathf.Clamp(TileStart.Z + j, 0, zGridLength - 1);
                if (i == 0)
                {
                    int X = Mathf.Clamp(TileStart.X, 0, xGridLength - 1);
                    TileRange.Add(GridTiles[X, Y]);
                }
                else
                {
                    int X = Mathf.Clamp(TileStart.X + i, 0, xGridLength - 1);
                    TileRange.Add(GridTiles[X, Y]);
                    X = Mathf.Clamp(TileStart.X - i, 0, xGridLength - 1);
                    TileRange.Add(GridTiles[X, Y]);
                }
            }
            layer--;
        }
       
        if (TileRange.Contains(TileStart)) { TileRange.Remove(TileStart); }
        return (TileRange);

    }


    //TODO Modify this to have the range as an input  
    public bool FindEnemyInAttackRange(EnviromentTile TileOver, List<EnviromentTile> TileRange)
    {
        if (TileRange.Contains(TileOver))
        {
            return true;
        }
        return false;
    }


    // Find the total range area that this object can move to and highlight it
    //TODO Return a range of tiles
    public List<EnviromentTile> FindMoveRange(EnviromentTile TileStart, int MaxDistance)
    {
       
        int layer = MaxDistance;
        List<EnviromentTile> TileRange = new List<EnviromentTile>();

        List<Node> RangePath = new List<Node>();
        // Loop through layers (Where layers is essentially the radius of a circle shrinking in)
        while (layer > 0)
        {
            // Start at the lowest Z Tile and move towards the highest Z Tile
            for (int j = -layer; j <= layer; j++)
            {
                int i = Mathf.Abs(j) - layer;

                // Dont allow to search for tiles outside of boundaries
                int Y = Mathf.Clamp(TileStart.Z + j, 0, zGridLength-1);

                // if there is no change in the X position then only one tile is found 
                //   x
                // _ _ _
                // _ S _
                // _ _ _
                //   x
                if ( i == 0)
                {
                    int X = Mathf.Clamp(TileStart.X, 0, xGridLength-1);
                    // find the distance it would take 
                    int range = Mathf.Abs(X - TileStart.X) + Mathf.Abs(Y - TileStart.Z);
                    RangePath.Clear();
                    // find how long Astar calculates a path 
                    RangePath = astar.FindPath(TileStart.GetComponent<Node>(), GridTiles[X, Y].GetComponent<Node>(), GridNodes);
                    // If it is equal then no obstacles decreasing range
                    foreach (Node node in RangePath)
                    {
                        if (node == (GridTiles[X, Y]).GetComponent<Node>())
                        {
                            if (RangePath.IndexOf(node) < MaxDistance) { { TileRange.Add(GridTiles[X, Y]); } }
                        }
                    }

                }
                // if there is a change in the X position then 2 tiles are found 
                //   _
                // _ _ _
                // _ S _
                // x _ x
                //   _
                else
                {
                    int X = Mathf.Clamp(TileStart.X + i, 0, xGridLength-1);
                    int range = Mathf.Abs(X - TileStart.X) + Mathf.Abs(Y - TileStart.Z);
                    RangePath.Clear();
                    RangePath = astar.FindPath(TileStart.GetComponent<Node>(), GridTiles[X, Y].GetComponent<Node>(), GridNodes);
                    foreach (Node node in RangePath)
                    {
                        if (node == (GridTiles[X, Y]).GetComponent<Node>())
                        {
                            if (RangePath.IndexOf(node) < MaxDistance) { { TileRange.Add(GridTiles[X, Y]); } }
                        }
                    }
    
                       

                    X = Mathf.Clamp(TileStart.X - i, 0, xGridLength-1);
                    range = Mathf.Abs(X - TileStart.X) + Mathf.Abs(Y - TileStart.Z);
                    RangePath.Clear();
                    RangePath = astar.FindPath(TileStart.GetComponent<Node>(), GridTiles[X, Y].GetComponent<Node>(), GridNodes);
                    foreach (Node node in RangePath)
                    {
                        if (node == (GridTiles[X, Y]).GetComponent<Node>())
                        {
                            if (RangePath.IndexOf(node) < MaxDistance) { { TileRange.Add(GridTiles[X, Y]); } }
                        }
                    }

                }
            }
            layer--;
        }
        if (TileRange.Contains(TileStart)) { TileRange.Remove(TileStart); }
        return TileRange;
    }

    // Creates a path depending on a Start and end tile 
    //TODO return a path 
    public List<EnviromentTile> FindTilesBetween(EnviromentTile TileStart, EnviromentTile TileEnd, int MaxDistance)
    {

        List<Node> PathNodes = new List<Node>();
        PathNodes = astar.FindPath(TileStart.GetComponent<Node>(), TileEnd.GetComponent<Node>(), GridNodes);

        List<EnviromentTile> Path = new List<EnviromentTile>();
        //// Add Tile currently standing on
        Path.Add(TileStart);

        // Add rest of tiles
        if (PathNodes.Count > 0)
        {
            if (PathNodes.Count < MaxDistance)
            {
                for (int i = 1; i <= PathNodes.Count; i++)
                {
                    Path.Add(PathNodes[i - 1].GetComponent<EnviromentTile>());
                }
            }
            // Path excedes the max move distance
            else
            {
                for (int i = 1; i <= MaxDistance; i++)
                {
                    Path.Add(PathNodes[i - 1].GetComponent<EnviromentTile>());
                }
            }
        }

        return Path;

    }

    void Start () {

        // Creates a Grid out of all the Tiles 
        TilesTotal = FindObjectsOfType<EnviromentTile>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
        GridNodes = new Node[xGridLength, zGridLength];
            foreach (EnviromentTile Tile in TilesTotal)
            {
                GridTiles[Tile.X, Tile.Z] = Tile;
            GridNodes[Tile.X, Tile.Z] = Tile.GetComponent<Node>();
            }
        

        astar = GetComponent<AStar>();

      

        }

    
	
}
