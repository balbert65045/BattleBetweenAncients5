using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControl : MonoBehaviour {


    //Grid size (Note: may want to change to automatically update with new map)
    public static int xGridLength = 7;
    public static int zGridLength = 7;

    private EnviromentTile[] TilesTotal;
    public EnviromentTile[,] GridTiles;
    private Node[,] GridNodes;
    public EnviromentTile[] PathTiles;
    private EnviromentTile CurrentTile;
    public List<EnviromentTile> TileRange;
   

    private AStar astar;

    public int PathLength { get { return PathTiles.Length; } }

    Color Orange;

    //Simply Turns off selection (Note: may want to take tiles out of path)
    // TODO MOVE this to player controller
    public void ResetTiles()
    {
        if (CurrentTile != null) { CurrentTile.ChangeColor(CurrentTile.MatColorOriginal); }

        if (TileRange != null)
        {
            foreach (EnviromentTile Tile in TileRange)
            {
                 { Tile.ChangeColor(Tile.MatColorOriginal); }
               
            }
            //Deselect Range and last tile over
            TileRange.Clear();
            CurrentTile = null;
        }
    }


    //Returns the position of the Tile in the path 
    //If not in path returns -1
    public int CheckPathPosition(EnviromentTile CurrentTile)
    {
        for (int i = 0; i< PathTiles.Length; i++)
        {
            if (PathTiles[i] == CurrentTile)
            {
                return i;
            }
        }
        return -1;
    }

    //Returns the tile next in path
    public Transform FindNextTileInPath(EnviromentTile CurrentTile)
    {
        for (int i = 0; i < PathTiles.Length; i++)
        {
            if (PathTiles[i] == CurrentTile)
            {
                return PathTiles[i + 1].transform;
            }
        }
        return null;
    }

    // TODO MOVE this to player controller or return a 
    public void HighlightAttckRange(EnviromentTile TileStart, int MaxDistance)
    {
       
        int layer = MaxDistance;
        TileRange = new List<EnviromentTile>();
        TileRange.Clear();
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
        foreach (EnviromentTile Tile in TileRange)
        {
            if (Tile != TileStart) { Tile.ChangeColor(Orange); }
        }
        if (TileRange.Contains(TileStart)) { TileRange.Remove(TileStart); }

    }


    //TODO Modify this to have the range as an input  
    public bool FindEnemyInAttackRange(EnviromentTile TileOver)
    {
        if (TileRange.Contains(TileOver))
        {
            return true;
        }
        return false;
    }

    //TODO Move this to Player Controller
    public void FindTileInAttackRange(EnviromentTile TileOver)
    {
       
        if (TileRange.Contains(TileOver))
        {
            if (CurrentTile != null) { CurrentTile.ChangeColor(Orange); }
            CurrentTile = TileOver;
            CurrentTile.ChangeColor(Color.red);
        }
      
    }

    // Find the total range area that this object can move to and highlight it
    //TODO Return a range of tiles
    public void HighlightMoveRange(EnviromentTile TileStart, int MaxDistance)
    {
       
        int layer = MaxDistance;
        TileRange = new List<EnviromentTile>();
        TileRange.Clear();
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
        //TODO Move this to Player Controller
        foreach (EnviromentTile Tile in TileRange)
        {
            if (Tile != TileStart) { Tile.ChangeColor(Color.cyan); };
        }

        if (TileRange.Contains(TileStart)) { TileRange.Remove(TileStart); }
    }

    // Creates a path depending on a Start and end tile 
    //TODO return a path 
    public void FindTilesBetween(EnviromentTile TileStart, EnviromentTile TileEnd, int MaxDistance)
    {
        int Xdelta = TileEnd.X - TileStart.X;
        int Zdelta = TileEnd.Z - TileStart.Z;

        // At each new path created unhighlight previous path made
        if (PathTiles != null)
        {
            foreach (EnviromentTile Tile in PathTiles)
            {
                if (TileRange.Contains(Tile)) { Tile.ChangeColor(Color.cyan); }
                else { Tile.ChangeColor(Tile.MatColorOriginal); }
            }
        }

        // Create new empty path
        //PathTiles = new EnviromentTile[Mathf.Abs(Xdelta) + Mathf.Abs(Zdelta) + 1];
        //int TilePathIndex = 0;


        // Depending on delta positions, add Tiles into tile path
        // (Note: Currently adds z tiles first then x tiles)
        // TODO refactor this into cleaner code


        List<Node> PathNodes = new List<Node>();
        PathNodes = astar.FindPath(TileStart.GetComponent<Node>(), TileEnd.GetComponent<Node>(), GridNodes);
        //Debug.Log(PathNodes.Count );

        //// Add Tile currently standing on
        if (PathNodes.Count > 0)
        {
            if (PathNodes.Count < MaxDistance)
            {
                PathTiles = new EnviromentTile[PathNodes.Count + 1];
                PathTiles[0] = TileStart;
                PathTiles[0].ChangeColor(Color.blue);
                for (int i = 1; i <= PathNodes.Count; i++)
                {
                    PathTiles[i] = PathNodes[i - 1].GetComponent<EnviromentTile>();
                    PathTiles[i].ChangeColor(Color.blue);
                }
            }
            // Path excedes the max move distance
            else
            {
                PathTiles = new EnviromentTile[MaxDistance + 1];
                PathTiles[0] = TileStart;
                PathTiles[0].ChangeColor(Color.blue);
                for (int i = 1; i <= MaxDistance; i++)
                {
                    PathTiles[i] = PathNodes[i - 1].GetComponent<EnviromentTile>();
                    PathTiles[i].ChangeColor(Color.blue);
                }
            }
           
        }

        else { TileStart.ChangeColor(Color.blue); }





        //if (Zdelta > 0)
        //{
        //    for (int z = TileStart.Z; z <= TileEnd.Z; z++)
        //    {
        //        GridTiles[TileStart.X, z].ChangeColor(Color.blue);
        //        PathTiles[TilePathIndex] = GridTiles[TileStart.X, z];
        //        TilePathIndex++;

        //    }
        //}
        //else
        //{
        //    for (int z = TileStart.Z; z >= TileEnd.Z; z--)
        //    {
        //        GridTiles[TileStart.X, z].ChangeColor(Color.blue);
        //        PathTiles[TilePathIndex] = GridTiles[TileStart.X, z];
        //        TilePathIndex++;

        //    }
        //}

        //if (Xdelta > 0)
        //{
        //    for (int x = TileStart.X + 1; x <= TileEnd.X; x++)
        //    {
        //        GridTiles[x, TileEnd.Z].ChangeColor(Color.blue);
        //        PathTiles[TilePathIndex] = GridTiles[x, TileEnd.Z];
        //        TilePathIndex++;

        //    }
        //}
        //else if (Xdelta < 0)
        //{
        //    for (int x = TileStart.X - 1; x >= TileEnd.X; x--)
        //    {
        //        GridTiles[x, TileEnd.Z].ChangeColor(Color.blue);
        //        PathTiles[TilePathIndex] = GridTiles[x, TileEnd.Z];
        //        TilePathIndex++;

        //    }
        //}




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

        Orange = new Color(1, 0.5f, 0, 1);

        }

    
	
}
