using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


    public CardType cardType;
    public EnviromentTile TileOn;
    TerrainControl terrainControl;
    public List<EnviromentTile> TilesNear;
	// Use this for initialization
	void Start () {
        terrainControl = FindObjectOfType<TerrainControl>();
        EnviromentTile[] Tiles = FindObjectsOfType<EnviromentTile>();
        foreach(EnviromentTile tile in Tiles)
        {
            if (tile.ObjectHeld == this.gameObject) { TileOn = tile; }
        }

    }

    public List<EnviromentTile> CheckTilesAround()
    {
        return (terrainControl.FindTilesOpenAround(TileOn));
    }
	
	// Update is called once per frame
	void Update () {
       // TilesNear = terrainControl.FindTilesOpenAround(TileOn);
    }
}
