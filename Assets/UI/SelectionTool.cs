using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SelectionTool : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster cameraRaycaster;
    EnviromentTile playerObjectCreatorSelected;
    public CardObject cardObjectSelected;
    ItemCreator itemCreator;
    RaycastHit m_hit;
    PathBuilder pathBuilder;
    GameController gameController;

    



    void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        itemCreator = GetComponent<ItemCreator>();
        pathBuilder = FindObjectOfType<PathBuilder>();
        gameController = FindObjectOfType<GameController>();
    }

    //TODO Create a User Control Script that delegates button/mouseclick/key presses

   // Waits for left click mouse on a Terrain Object to try to select a target
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("pointer1"))
        {

            var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
                OnItemSelect(m_hit.transform);
            }
            else
            {
                if (playerObjectCreatorSelected != null)
                {
                    cardObjectSelected = null;
                    gameController.SelectObject(null);
                }
            }
        }
    }


    // Selects the target for GameController 
    // returns null if does not hold a card object
    void OnItemSelect(Transform transform)
    {
        if (cardObjectSelected != null)
        {
            cardObjectSelected = null;
            gameController.SelectObject(null);

        }
        if (cameraRaycaster.transormHit != null)
        {
            if (cameraRaycaster.transormHit.GetComponent<EnviromentTile>() != null)
            {
                if (cameraRaycaster.transormHit.GetComponent<EnviromentTile>().ObjectHeld != null)
                {
                    cardObjectSelected = cameraRaycaster.transormHit.GetComponent<EnviromentTile>().ObjectHeld.GetComponent<CardObject>();
                    gameController.SelectObject(cardObjectSelected);
                }
            }
        }
    }

}
