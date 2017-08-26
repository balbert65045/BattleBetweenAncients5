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
    PathBuilder pathBuilder;

    bool ObjectSelected;
    



    void Start () {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnPathChange;
        itemCreator = GetComponent<ItemCreator>();
        pathBuilder = FindObjectOfType<PathBuilder>();
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
                    pathBuilder.DeselectPath();
                    ObjectSelected = false;
                }
            }
        }
    }

    void OnPathChange(Transform newTransform)
    {
        if (newTransform.GetComponent<PlayerObjectCreator>() != null)
        {
            if (playerObjectCreatorSelected != null)
            {
                if (playerObjectCreatorSelected.ObjectHeld != null)
                {
                    pathBuilder.FindTilesBetween(playerObjectCreatorSelected, newTransform.GetComponent<PlayerObjectCreator>());
                }
            }
        }
    }

    void OnItemSelect(Transform transform)
    {
        if (playerObjectCreatorSelected != null)
        {
            playerObjectCreatorSelected.ObjectHeldDeselected();
            pathBuilder.DeselectPath();
            ObjectSelected = false;
        }
        //TODO fix null exception with the raycaster 
        if (cameraRaycaster.transormHit != null)
        {
            if (cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>() != null)
            {
                playerObjectCreatorSelected = cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>();
                playerObjectCreatorSelected.ObjectHeldSelected();
                ObjectSelected = true;
            }
        }
    }

}
