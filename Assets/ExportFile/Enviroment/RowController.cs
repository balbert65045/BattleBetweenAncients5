using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowController : MonoBehaviour {


    PlayerObjectCreator[] Tiles;
    public PlayerObjectCreator[] OrderdTiles;

    private GameObject ReadyObject;
    int length;
    public bool HoldingImage = false;
    public int imageIndex;
    
    // Use this for initialization
    void Start () {
        Tiles = GetComponentsInChildren<PlayerObjectCreator>();
        OrderdTiles = GetComponentsInChildren<PlayerObjectCreator>();
        length = Tiles.Length - 1;

        for (int i =0; i < Tiles.Length; i++)
        {
            int index = 0;
            for (int n = 0; n < Tiles.Length; n++)
            {
                if (i == n)
                {

                }
                else if (Tiles[i].transform.position.x > Tiles[n].transform.position.x)
                {
                    index++;
                }
            }
            OrderdTiles[index] = Tiles[i];
        }
    }
	




    public bool CheckItemAvailability(Transform newTransform, GameObject newItem)
    {

      
        // Find which tile in order it is
        bool ItemPlaceAvailable = false;
        int TileIndex = 0;
        for (int i = 0; i < OrderdTiles.Length; i++)
        {
            if (newTransform == OrderdTiles[i].transform)
            {
                TileIndex = i;
            }
        }


        ItemPlaceAvailable = true;
      
        return (ItemPlaceAvailable);
    }
    

}
