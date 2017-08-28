using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilder : MonoBehaviour {


    //Grid size (Note: may want to change to automatically update with new map)
    public static int xGridLength = 7;
    public static int zGridLength = 7;

    private EnviromentTile[] TilesTotal;
    public EnviromentTile[,] GridTiles;
    public EnviromentTile[] PathTiles;

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
    public void FindTilesBetween(EnviromentTile TileStart, EnviromentTile TileEnd)
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
        PathTiles = new EnviromentTile[Mathf.Abs(Xdelta) + Mathf.Abs(Zdelta) + 1];
        int TilePathIndex = 0;


        // Depending on delta positions, add Tiles into tile path
        // (Note: Currently adds z tiles first then x tiles)
        // TODO refactor this into cleaner code
        if (Zdelta > 0)
        {
            for (int z = TileStart.Z; z <= TileEnd.Z; z++)
            {
                GridTiles[TileStart.X, z].ChangeColor(Color.blue);
                PathTiles[TilePathIndex] = GridTiles[TileStart.X, z];
                TilePathIndex++;

            }
        }
        else
        {
            for (int z = TileStart.Z; z >= TileEnd.Z; z--)
            {
                GridTiles[TileStart.X, z].ChangeColor(Color.blue);
                PathTiles[TilePathIndex] = GridTiles[TileStart.X, z];
                TilePathIndex++;

            }
        }

        if (Xdelta > 0)
        {
            for (int x = TileStart.X + 1; x <= TileEnd.X; x++)
            {
                GridTiles[x, TileEnd.Z].ChangeColor(Color.blue);
                PathTiles[TilePathIndex] = GridTiles[x, TileEnd.Z];
                TilePathIndex++;

            }
        }
        else if (Xdelta < 0)
        {
            for (int x = TileStart.X - 1; x >= TileEnd.X; x--)
            {
                GridTiles[x, TileEnd.Z].ChangeColor(Color.blue);
                PathTiles[TilePathIndex] = GridTiles[x, TileEnd.Z];
                TilePathIndex++;

            }
        }




    }

    void Start () {

        // Creates a Grid out of all the Tiles 
        TilesTotal = FindObjectsOfType<EnviromentTile>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
            foreach (EnviromentTile Tile in TilesTotal)
            {
                GridTiles[Tile.X, Tile.Z] = Tile;
            }
        }

    
	
}
