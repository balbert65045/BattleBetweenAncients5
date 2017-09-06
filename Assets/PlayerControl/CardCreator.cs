using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CardCreator : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster cameraRaycaster;
    EnviromentTile Tile;
    PlayerObjectHolder playerObjectHolder;
    CreatorButton[] creatorButtons;
    EnviromentTile OldTileOver;
    

    public bool ActiveImage = false;
    bool turnUsed = false;

    public void ResetTurn()
    {
        turnUsed = false;
    }


	void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnItemCreateImage;
        playerObjectHolder = FindObjectOfType<PlayerObjectHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnUsed)
        {
            if (CrossPlatformInputManager.GetButtonDown("pointer1"))
            {
                creatorButtons = FindObjectsOfType<CreatorButton>();
                foreach (CreatorButton CB in creatorButtons)
                {
                    ActiveImage = ActiveImage || CB.CheckMousePositionOnButton();

                }

            }
            else if (CrossPlatformInputManager.GetButtonUp("pointer1"))
            {
                ActiveImage = false;
                OnItemCreate();
                OldTileOver = null;
            }

            //Cancel Object to spawn
            if (CrossPlatformInputManager.GetButtonDown("Cancel"))
            {
                if (OldTileOver != null)
                {
                    ActiveImage = false;
                    OldTileOver.DestroyImage();
                    OldTileOver = null;
                }
            }
        }
    }


    // TODO find why first tile hover over does not show image
    void OnItemCreateImage(Transform newTransform)
    {
        
        if (newTransform.GetComponent<EnviromentTile>() != null)
        {
            if (ActiveImage)
            {
                Tile = newTransform.GetComponent<EnviromentTile>();
                GameObject newItem = playerObjectHolder.ReadyImage;

                if (OldTileOver != null)
                {
                    OldTileOver.DestroyImage();
                }
                if (Tile.cardType == CardType.Open)
                {
                    Tile.OnItemMake(newItem);
                    OldTileOver = Tile;
                } 
                
            }
        }
      }


    void OnItemCreate()
        {
            if (OldTileOver != null)
            {
            OldTileOver.DestroyImage();
            if (Tile.cardType == CardType.Open)
                {
                    GameObject newItem = playerObjectHolder.ReadyObject;
                    OldTileOver.OnItemMake(newItem);
                    playerObjectHolder.DestroyCardUsed();
                    turnUsed = true;
                }
            }
        }

    }
