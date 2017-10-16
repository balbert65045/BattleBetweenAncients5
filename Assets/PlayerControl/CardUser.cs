using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CardUser : MonoBehaviour {

    CameraRaycaster cameraRaycaster;
    EnviromentTile Tile;
    CardHand cardHand;
    EnviromentTile OldTileOver;
    Spawner playerSpawner;
    public List<EnviromentTile> SpawnTiles;
    PowerCounter powerCounter;

    // Use this for initialization
    void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += CardShowUse;
        cardHand = FindObjectOfType<CardHand>();
        powerCounter = FindObjectOfType<PowerCounter>();
        Spawner[] spawners = FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            if (spawner.cardType == CardType.Player) { playerSpawner = spawner; }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	  if (CrossPlatformInputManager.GetButtonUp("pointer1"))
        {
            OnItemCreate();
            OldTileOver = null;
        }
        if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            if (cardHand.ReadyImage)
            {
                if (OldTileOver != null)
                {
                    OldTileOver.DestroyImage();
                    OldTileOver = null;
                }
                SpawnTiles = playerSpawner.CheckTilesAround();
                foreach (EnviromentTile tile in SpawnTiles)
                {
                    tile.ChangeColor(tile.MatColorOriginal);
                }
            }
        }
    }

    void CardShowUse(Transform newTransform)
    {
      //  Debug.Log("new tile " + newTransform.GetComponent<EnviromentTile>());
      //  Debug.Log("old tile " + OldTileOver);

      //  Debug.Log(cardHand.CardUsing);
        if (cardHand.CardUsing != null)
        {
            if (cardHand.CardUsing.GetComponent<CardSummon>() != null)
            {
               // Debug.Log("CreatingImage");
                OnItemCreateImage(newTransform);
            }
               
        }
        
    }

    void OnItemCreateImage(Transform newTransform)
    {

        SpawnTiles = playerSpawner.CheckTilesAround();
        foreach (EnviromentTile tile in SpawnTiles)
        {
            // Debug.Log("ColorTiles");
            tile.ChangeColor(Color.cyan);
        }
        Tile = newTransform.GetComponent<EnviromentTile>();
        GameObject newItem = cardHand.CardUsing.GetComponent<CardSummon>().SummonImageObject;

        if (OldTileOver != null)
        {
        //    Debug.Log("DestroyedObject");
            OldTileOver.DestroyImage();
        }
        if (SpawnTiles.Contains(Tile))
        {
       //     Debug.Log("Tile In Spawn Area");
            Tile.OnItemMake(newItem);
            OldTileOver = Tile;
        }
     
    }


    void OnItemCreate()
    {
        if (cardHand.CardUsing)
        {
            OldTileOver.DestroyImage();
            if (SpawnTiles.Contains(Tile))
            {

                powerCounter.RemovePower(cardHand.CardUsing.GetPowerAmount);
                GameObject newItem = cardHand.CardUsing.GetComponent<CardSummon>().SummonObject;
                OldTileOver.OnItemMake(newItem);
                cardHand.DestroyCardUsed();

            }
            foreach (EnviromentTile tile in SpawnTiles)
            {
                // Debug.Log("resetTiles");
                tile.ChangeColor(tile.MatColorOriginal);
            }
        }
        OldTileOver = null;



    }

}
