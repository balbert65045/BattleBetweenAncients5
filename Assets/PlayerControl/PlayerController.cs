using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    // Use this for initialization
    SelectionTool selectionTool;
    CardObject selectedCardObject;
    CameraRaycaster cameraRaycaster;
    SelectionPanel selectionPanel;
    RaycastHit m_hit;

    bool allowPathChange = true;


    // Listens to Current Card Object for Movement Calls
    public void SelectedObjectMoveStateChange(bool Moving)
    {

        if (Moving)
        {
            //Deselect path made (NOTE: comment this if you want to see tiles diapear one at a time)

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

    public void SelectObject(CardObject cardObject)
    {
        //Deselect object held and tile
        if (selectedCardObject != null)
        {
            deSelectObject();

        }
        //reset Path on new selection

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
        }
    }


    //Method used to completely deslect the object (NOTE: may want to see if this can be refactored into fewer lines)
    void deSelectObject()
    {
        selectionPanel.SetObject(null);
        selectionPanel.gameObject.SetActive(false);
        selectedCardObject.MoveChangeObservers -= SelectedObjectMoveStateChange;
        selectedCardObject.StateChangeObservers -= OnCurrentObjectStateChange;
        selectedCardObject.DeselectObject();
        selectedCardObject = null;
    }

    void OnCurrentObjectStateChange(CardState state)
    {
        var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
        switch (selectedCardObject.cardState)
        {
            case CardState.Move:
              
                if (hit.HasValue)
                {
                    RaycastHit m_hit;
                    m_hit = hit.Value;
                    // Check if their is another object on the tile/ if the tile is open
                    if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open)
                    {
                        selectedCardObject.MakePath(m_hit.transform.GetComponent<EnviromentTile>());
                    }
                }
            
                
                break;
            case CardState.Attack:
                if (hit.HasValue)
                {
                    RaycastHit m_hit;
                    m_hit = hit.Value;
                    // Check if their is another object on the tile/ if the tile is open
                    if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open || m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Enemy)
                    {
                        Debug.Log("Looking");
                        selectedCardObject.HighlightTileInAttackRange(m_hit.transform.GetComponent<EnviromentTile>());
                    }
                }

                break;
            default:
                return;
        }
    }


    void Start()
    {
        selectionTool = FindObjectOfType<SelectionTool>();
        selectionPanel = FindObjectOfType<SelectionPanel>();
        selectionPanel.gameObject.SetActive(false);
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnPathChange;
    }

    private void Update()
    {
        if (selectedCardObject != null)
        {
            switch (selectedCardObject.cardState)
            {
                case CardState.Move:
                    LookForMoveInput();
                    break;
                case CardState.Attack:
                    LookForAttackInput();
                    break;
                default:
                    return;
            }
            LookForStateChange();
        }
    }

    private void LookForStateChange()
    {
        if (CrossPlatformInputManager.GetButtonDown("MoveStateButton"))
        {
            selectedCardObject.StateChange(CardState.Move);
        }
        else if (CrossPlatformInputManager.GetButtonDown("AttackStateButton"))
        {
            selectedCardObject.StateChange(CardState.Attack);
        }
    }

    //TODO check if enemy is in attack radius
    private void LookForAttackInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("pointer2"))
        {
            var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
                // Check if their is another object on the tile/ if the tile is open
                if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Enemy)
                {
                    if (selectedCardObject.CheckAttackInRange(m_hit.transform.GetComponent<EnviromentTile>()))
                    { selectedCardObject.AttackObject(m_hit.transform.GetComponent<EnviromentTile>().ObjectHeld); }
                }
            }
        }
    }

    private void LookForMoveInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("pointer2"))
        {
            var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
                // Check if their is another object on the tile/ if the tile is open
                if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open)
                {
                    //Tell Observers object moving
                    selectedCardObject.enableMovement();
                }
            }
        }
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
                        selectedCardObject.MakePath(newTransform.GetComponent<EnviromentTile>());
                        break;
                    case CardState.Attack:
                        selectedCardObject.HighlightTileInAttackRange(newTransform.GetComponent<EnviromentTile>());
                        break;
                    default:
                        return;
                }
            }

        }
    }

}