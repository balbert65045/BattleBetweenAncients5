using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Use this for initialization
    SelectionTool selectionTool;
    PathBuilder pathBuilder;
    CardObject selectedCardObject;
    CameraRaycaster cameraRaycaster;
    EnviromentTile currentTileSelected;

    

    bool allowPathChange = true;
    bool allowObjectSelection;


    public void SelectObject(CardObject cardObject)
    {
        //Deselect object held and tile
        if (selectedCardObject != null)
        {
            deSelectObject();

        }
        //reset Path on new selection
        pathBuilder.DeselectPath();

        if (cardObject != null)
        {
            //Select new object held
            selectedCardObject = cardObject;
            selectedCardObject.SelectedObject();
            selectedCardObject.MoveChangeObservers += SelectedObjectMoveStateChange;
            //Highlight Tile
            currentTileSelected = selectedCardObject.GetCurrentTile;
            currentTileSelected.ChangeColor(Color.blue);
        }
        else
        {
            selectedCardObject = null;
            currentTileSelected = null;
        }
    }

	void Start () {
        selectionTool = FindObjectOfType<SelectionTool>();
        pathBuilder = FindObjectOfType<PathBuilder>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnPathChange;
    }


    //Change Path when a new tile is over cursor while an Object is Selected
    void OnPathChange(Transform newTransform)
    {
        if (selectedCardObject != null)
        {
            if (allowPathChange)
            {
                pathBuilder.FindTilesBetween(currentTileSelected, newTransform.GetComponent<EnviromentTile>());
            }
           
        }
    }

    //Method used to completely deslect the object (NOTE: may want to see if this can be refactored into fewer lines)
    void deSelectObject()
    {
        selectedCardObject.MoveChangeObservers -= SelectedObjectMoveStateChange;
        selectedCardObject.GetCurrentTile.ChangeColor(selectedCardObject.GetCurrentTile.MatColorOriginal);
        selectedCardObject.DeselectObject();
        selectedCardObject = null;
        currentTileSelected = null;
    }



    // Listens to Current Card Object for Movement Calls
    public void SelectedObjectMoveStateChange(bool Moving)
    {
   
        if (Moving)
        {
            //Deselect path made (NOTE: comment this if you want to see tiles diapear one at a time)
            pathBuilder.DeselectPath();

            //Disable ability to change path mid move and select different target
            allowPathChange = false;
            selectionTool.enabled = false;
        }
        else
        {
            //After Moving Disable the Object 
            //(Note: May want to keep object selected in order to do combat actions after move)
            deSelectObject();
            //Enable ability to change path mid move and select different target
            allowPathChange = true;
            selectionTool.enabled = true;
        }
       
    }

}
