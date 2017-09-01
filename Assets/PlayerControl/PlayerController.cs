using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    SelectionTool selectionTool;
    TerrainControl terrainControl;
    CardObject selectedCardObject;
    CameraRaycaster cameraRaycaster;
    EnviromentTile currentTileSelected;
    SelectionPanel selectionPanel;

    bool allowPathChange = true;


    public void SelectObject(CardObject cardObject)
    {
        //Deselect object held and tile
        if (selectedCardObject != null)
        {
            deSelectObject();

        }
        //reset Path on new selection
        terrainControl.DeselectPath();

        if (cardObject != null)
        {
            switch (cardObject.cardType)
            {
                case CardType.Player:
                {
                        //Select new object held
                        selectedCardObject = cardObject;
                        selectedCardObject.SelectedObject();
                        selectedCardObject.MoveChangeObservers += SelectedObjectMoveStateChange;
                        selectedCardObject.StateChangeObservers += OnCurrentObjectStateChange;
                        //Highlight Tile
                        currentTileSelected = selectedCardObject.GetCurrentTile;
                        currentTileSelected.ChangeColor(Color.blue);
                        //HighlightRange
                        terrainControl.HighlightMoveRange(selectedCardObject.GetCurrentTile, selectedCardObject.GetMaxMoveDistance);
                        //EnableUI and show it
                        selectionPanel.gameObject.SetActive(true);
                        selectionPanel.SetObject(selectedCardObject);
                        break;
                }
                case CardType.Enemy:
                {
                    break;
                }
                default:
                {
                    return;
                }
            }
          
        }
        else
        {
            selectedCardObject = null;
            currentTileSelected = null;
        }
    }

    //Method used to completely deslect the object (NOTE: may want to see if this can be refactored into fewer lines)
    void deSelectObject()
    {
        selectionPanel.SetObject(null);
        selectionPanel.gameObject.SetActive(false);
        selectedCardObject.MoveChangeObservers -= SelectedObjectMoveStateChange;
        selectedCardObject.StateChangeObservers -= OnCurrentObjectStateChange;
        selectedCardObject.GetCurrentTile.ChangeColor(selectedCardObject.GetCurrentTile.MatColorOriginal);
        selectedCardObject.DeselectObject();
        selectedCardObject = null;
        currentTileSelected = null;
    }

    void OnCurrentObjectStateChange(CardState state)
    {
        switch (selectedCardObject.cardState)
        {
            case CardState.Move:
                currentTileSelected.ChangeColor(Color.blue);
                terrainControl.HighlightMoveRange(selectedCardObject.GetCurrentTile, selectedCardObject.GetMaxMoveDistance);
                var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
                if (hit.HasValue)
                {
                    RaycastHit m_hit;
                    m_hit = hit.Value;
                    // Check if their is another object on the tile/ if the tile is open
                    if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open)
                    {
                        terrainControl.FindTilesBetween(currentTileSelected, m_hit.transform.GetComponent<EnviromentTile>(), 
                            selectedCardObject.GetMaxMoveDistance);
                    }
                }
                break;
            case CardState.Attack:
                terrainControl.DeselectPath();
                currentTileSelected.ChangeColor(Color.red);
                terrainControl.HighlightAttckRange(selectedCardObject.GetCurrentTile, selectedCardObject.GetMaxAttackDistance);

                break;
            default:
                return;
        }
    }


    void Start () {
        selectionTool = FindObjectOfType<SelectionTool>();
        selectionPanel = FindObjectOfType<SelectionPanel>();
        selectionPanel.gameObject.SetActive(false);
        terrainControl = FindObjectOfType<TerrainControl>();
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
                switch (selectedCardObject.cardState)
                {
                    case CardState.Move:
                      
                        terrainControl.FindTilesBetween(currentTileSelected, newTransform.GetComponent<EnviromentTile>(), selectedCardObject.GetMaxMoveDistance);
                        break;
                    case CardState.Attack:
                        terrainControl.FindInAttackRange(newTransform.GetComponent<EnviromentTile>());
                        break;
                    default:
                        return;
                }
            }
           
        }
    }

   



    // Listens to Current Card Object for Movement Calls
    public void SelectedObjectMoveStateChange(bool Moving)
    {
   
        if (Moving)
        {
            //Deselect path made (NOTE: comment this if you want to see tiles diapear one at a time)
            terrainControl.DeselectPath();

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
