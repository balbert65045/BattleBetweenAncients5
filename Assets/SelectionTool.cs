using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SelectionTool : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster cameraRaycaster;
    PlayerObjectCreator playerObjectCreatorSelected;
    ItemCreator itemCreator;
    RaycastHit m_hit;
    



    void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        itemCreator = GetComponent<ItemCreator>();
    }

    // Update is called once per frame
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
                    playerObjectCreatorSelected.ObjectHeldDeselected();
                }
            }
        }
    }

    void OnItemSelect(Transform transform)
    {
        if (playerObjectCreatorSelected != null)
        {
            playerObjectCreatorSelected.ObjectHeldDeselected();
        }
        //TODO fix null exception with the raycaster 
        if (cameraRaycaster.transormHit != null)
        {
            if (cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>() != null)
            {
                playerObjectCreatorSelected = cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>();
                playerObjectCreatorSelected.ObjectHeldSelected();
            }
        }
    }

}
