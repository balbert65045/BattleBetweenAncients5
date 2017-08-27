using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SelectionTool : MonoBehaviour {

    // Use this for initialization
    CameraRaycaster cameraRaycaster;
    PlayerObjectCreator playerObjectCreatorSelected;
    public CardObject cardObjectSelected;
    ItemCreator itemCreator;
    RaycastHit m_hit;
    PathBuilder pathBuilder;

    



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
                    cardObjectSelected.DeselectObject();
                    cardObjectSelected = null;
                    pathBuilder.DeselectPath();
                    
                }
            }
        }
    }

    // TODO Clean this up and disable when object is moving 
    void OnPathChange(Transform newTransform)
    {
        if (cardObjectSelected != null)
        {
            if (newTransform.GetComponent<PlayerObjectCreator>() != null)
            {
                pathBuilder.FindTilesBetween(cardObjectSelected.GetCurrentTile, newTransform.GetComponent<PlayerObjectCreator>());
            }
        }
        //if (newTransform.GetComponent<PlayerObjectCreator>() != null)
        //{
        //    if (playerObjectCreatorSelected != null)
        //    {
        //        if (playerObjectCreatorSelected.ObjectHeld != null)
        //        {
        //            pathBuilder.FindTilesBetween(playerObjectCreatorSelected, newTransform.GetComponent<PlayerObjectCreator>());
        //        }
        //    }
        //}
    }

    void OnItemSelect(Transform transform)
    {
        if (cardObjectSelected != null)
        {
            cardObjectSelected.DeselectObject();
            cardObjectSelected = null;
            pathBuilder.DeselectPath();
            
        }
        //TODO fix null exception with the raycaster 
        if (cameraRaycaster.transormHit != null)
        {
            if (cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>() != null)
            {
                if (cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>().ObjectHeld != null)
                {
                    cardObjectSelected = cameraRaycaster.transormHit.GetComponent<PlayerObjectCreator>().ObjectHeld.GetComponent<CardObject>();
                    cardObjectSelected.SelectedObject();
                    cardObjectSelected.GetCurrentTile.ChangeColor(Color.blue);
                }
            }
        }
    }

}
