﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilder : MonoBehaviour {


    //Grid size (Note: may want to change to automatically update with new map)
    public static int xGridLength = 7;
    public static int zGridLength = 7;

    private EnviromentTile[] TilesTotal;
    public EnviromentTile[,] GridTiles;
    private Node[,] GridNodes;
    public EnviromentTile[] PathTiles;
    public List<Node> PathNodes;

    private AStar astar;

    public int PathLength { get { return PathTiles.Length; } }

    //Simply Turns off selection (Note: may want to take tiles out of path)
    public void DeselectPath()
    {
        if (PathTiles != null)
        {
            foreach (EnviromentTile Tile in PathTiles)
            {
                Tile.ChangeColor(Tile.MatColorOriginal);
            }
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


    // Creates a path depending on a Start and end tile 
    //TODO needs to be modified for avoidance objects like terrain and other cards
    public void FindTilesBetween(EnviromentTile TileStart, EnviromentTile TileEnd, int MaxDistance)
    {
        int Xdelta = TileEnd.X - TileStart.X;
        int Zdelta = TileEnd.Z - TileStart.Z;

        // At each new path created unhighlight previous path made
        if (PathTiles != null)
        {
            foreach (EnviromentTile Tile in PathTiles)
            {
                Tile.ChangeColor(Tile.MatColorOriginal);
            }
        }

        // Create new empty path
        //PathTiles = new EnviromentTile[Mathf.Abs(Xdelta) + Mathf.Abs(Zdelta) + 1];
        //int TilePathIndex = 0;


        // Depending on delta positions, add Tiles into tile path
        // (Note: Currently adds z tiles first then x tiles)
        // TODO refactor this into cleaner code


        // TODO After 1 run breaks
        Debug.Log("Looking for path");
        PathNodes.Clear();
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
        }

    
	
}
