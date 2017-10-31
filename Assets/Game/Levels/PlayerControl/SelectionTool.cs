using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class SelectionTool : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster cameraRaycaster;
    //EnviromentTile playerObjectCreatorSelected;
    public CardObject cardObjectSelected;
    RaycastHit m_hit;

    PlayerController gameController;

    RectTransform CardAreaRect;


    



    void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        CardAreaRect = FindObjectOfType<CardHand>().transform.parent.GetComponent<RectTransform>();
        gameController = FindObjectOfType<PlayerController>();
    }

    //TODO Create a User Control Script that delegates button/mouseclick/key presses

   // Waits for left click mouse on a Terrain Object to try to select a target
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("pointer1"))
        {
            if (Input.mousePosition.y > CardAreaRect.position.y + (CardAreaRect.sizeDelta.y / 2))
            {
                var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
                if (hit.HasValue)
                {
                    m_hit = hit.Value;
                    OnItemSelect(m_hit.transform);
                    return;
                }
            }
            else
            {
                //Unselect if click in the card area
                if (gameController.getSelectedObject != null)
                {
                    gameController.deSelectObject();
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
