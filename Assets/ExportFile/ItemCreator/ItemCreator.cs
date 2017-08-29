using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ItemCreator : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster cameraRaycaster;
    EnviromentTile playerObjectCreator;
    PlayerObjectHolder playerObjectHolder;
    CreatorButton[] creatorButtons;
    EnviromentTile OldTileImage;

    public bool ActiveImage = false;


    


	void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnItemCreateImage;
        playerObjectHolder = FindObjectOfType<PlayerObjectHolder>();
        creatorButtons = FindObjectsOfType<CreatorButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("pointer1"))
        {
            foreach (CreatorButton CB in creatorButtons)
            {
                ActiveImage = ActiveImage || CB.CheckMousePositionOnButton();
            }
         
        }
        else if (CrossPlatformInputManager.GetButtonUp("pointer1"))
        {
            ActiveImage = false;
            OnItemCreate();
            OldTileImage = null;
        }
    }


    // TODO find why first tile hover over does not show image
    void OnItemCreateImage(Transform newTransform)
    {
        if (newTransform.GetComponent<EnviromentTile>() != null)
        {
            if (ActiveImage)
            {
                playerObjectCreator = newTransform.GetComponent<EnviromentTile>();
                GameObject newItem = playerObjectHolder.ReadyImage;

                if (OldTileImage != null)
                {
                    OldTileImage.DestroyImage();
                }
                if (playerObjectCreator.cardType == CardType.Open)
                {
                    playerObjectCreator.OnItemMake(newItem);
                    OldTileImage = playerObjectCreator;
                } 
                
            }
        }
      }


    //TODO Make it so cant put multiple objects on one tile 
    void OnItemCreate()
        {
            if (OldTileImage != null)
            {
            OldTileImage.DestroyImage();
            if (playerObjectCreator.cardType == CardType.Open)
                {
                  
                    GameObject newItem = playerObjectHolder.ReadyObject;
                    OldTileImage.OnItemMake(newItem);
                }
            }
        }

    }
