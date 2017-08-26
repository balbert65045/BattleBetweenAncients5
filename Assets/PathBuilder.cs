using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilder : MonoBehaviour {


    
    public static int xGridLength = 7;
   
    public static int zGridLength = 7;

    // Use this for initialization
    private PlayerObjectCreator[] TilesTotal;
    public PlayerObjectCreator[,] GridTiles;
    private PlayerObjectCreator[] PathTiles;


    public void DeselectPath()
    {
        if (PathTiles != null)
        {
            foreach (PlayerObjectCreator Tile in PathTiles)
            {
                Tile.ChangeColor(Tile.MatColorOriginal);
            }
        }
    }

    public void FindTilesBetween(PlayerObjectCreator TileStart, PlayerObjectCreator TileEnd)
    {
        int Xdelta = TileEnd.X - TileStart.X;
        int Zdelta = TileEnd.Z - TileStart.Z;

        if (PathTiles != null)
        {
            foreach (PlayerObjectCreator Tile in PathTiles)
            {
                Tile.ChangeColor(Tile.MatColorOriginal);
            }
        }


        PathTiles = new PlayerObjectCreator[Mathf.Abs(Xdelta) + Mathf.Abs(Zdelta) + 2];
       // PathTiles = new PlayerObjectCreator[Mathf.Abs(Xdelta) + 1];

        int TilesinPath = 0;

        if (Zdelta > 0)
        {
            for (int z = TileStart.Z; z <= TileEnd.Z; z++)
            {
                GridTiles[TileStart.X, z].ChangeColor(Color.blue);
                PathTiles[TilesinPath] = GridTiles[TileStart.X, z];
                TilesinPath++;

            }
        }
        else
        {
            for (int z = TileEnd.Z; z <= TileStart.Z; z++)
            {
                GridTiles[TileStart.X, z].ChangeColor(Color.blue);
                PathTiles[TilesinPath] = GridTiles[TileStart.X, z];
                TilesinPath++;

            }
        }

        if (Xdelta > 0)
        {
            for (int x = TileStart.X; x <= TileEnd.X; x++)
            {
                GridTiles[x, TileEnd.Z].ChangeColor(Color.blue);
                PathTiles[TilesinPath] = GridTiles[x, TileEnd.Z];
                TilesinPath++;

            }
        }
        else
        {
            for (int x = TileEnd.X; x <= TileStart.X; x++)
            {
                GridTiles[x, TileEnd.Z].ChangeColor(Color.blue);
                PathTiles[TilesinPath] = GridTiles[x, TileEnd.Z];
                TilesinPath++;

            }
        }




    }

    void Start () {
        TilesTotal = FindObjectsOfType<PlayerObjectCreator>();
        GridTiles = new PlayerObjectCreator[xGridLength, zGridLength];
            foreach (PlayerObjectCreator Tile in TilesTotal)
            {
                GridTiles[Tile.X, Tile.Z] = Tile;
            }

        //Debug.Log(GridTiles[1, 1]);
        }

    
	
	// Update is called once per frame
	void Update () {
		
	}
}
