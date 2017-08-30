using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour {

    public static int xGridLength = 7;
    public static int zGridLength = 7;

    [SerializeField]
     GameObject[] enmyCardObjects;

    public EnviromentTile[] TilesTotal;
    public EnviromentTile[,] GridTiles;


    // Use this for initialization
    void Start () {
        TilesTotal = FindObjectsOfType<EnviromentTile>();
        GridTiles = new EnviromentTile[xGridLength, zGridLength];
        foreach (EnviromentTile Tile in TilesTotal)
        {
            GridTiles[Tile.X, Tile.Z] = Tile;
        }

        GridTiles[5, 5].OnItemMake(enmyCardObjects[0]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
