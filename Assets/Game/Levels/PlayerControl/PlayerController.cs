﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    // Use this for initialization
    SelectionTool selectionTool;
    CardObject selectedCardObject;
    public CardObject getSelectedObject {get{return selectedCardObject;}}
    CardSelector cardCreator;
    public List<EnviromentTile> Path;
    public List<EnviromentTile> Range;
    EnviromentTile LastMoveTileOver;
    EnviromentTile LastTargetTileOver;

    PauseScreen pauseScreen;

    CameraRaycaster cameraRaycaster;
    SelectionPanel selectionPanel;
    RaycastHit m_hit;

    bool allowPathChange = true;

    Color Orange;


    public void DisableTools()
    {
        selectionTool.enabled = false;
        cardCreator.enabled = false;
        allowPathChange = false;
        if (selectedCardObject != null) { deSelectObject(); }
    } 

    public void ResetTools()
    {
        selectionTool.enabled = true;
        cardCreator.enabled = true;
        allowPathChange = true;
        CardObject[] PlayerCardsonField = FindObjectsOfType<CardObject>();
        foreach (CardObject cardObject in PlayerCardsonField)
        {
            if (cardObject.cardType == CardType.Player) { cardObject.ResetAbilities(); }
        }
    }



    // Listens to Current Card Object for Movement Calls
    public void SelectedObjectMoveStateChange(bool Moving, CardObject cardObject)
    {

        if (Moving)
        {
            //Deselect path made (NOTE: comment this if you want to see tiles diapear one at a time)

            //Disable ability to change path mid move and select different target
            allowPathChange = false;
            selectionTool.enabled = false;
            cardCreator.enabled = false;
        }
        else
        {
            //After Moving Disable the Object 
            //(Note: May want to keep object selected in order to do combat actions after move)
            // deSelectObject();
            //Enable ability to change path mid move and select different target

            allowPathChange = true;
            selectionTool.enabled = true;
            cardCreator.enabled = true;

            selectedCardObject.SelectedObject();
            Range = selectedCardObject.FindAttackRange();
            // Attack if selected on attack card 
            if (LastTargetTileOver != null)
            {
                selectedCardObject.EngageCombat(CombatType.Attack, LastTargetTileOver.ObjectHeld);
                ResetTiles();
            }
            // no attack card switch stats
            else
            {
                selectedCardObject.StateChange(CardState.Attack);
            }

            //


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

                        //EnableUI and show it
                        selectionPanel.gameObject.SetActive(true);
                        selectionPanel.SetObject(selectedCardObject);

                        selectedCardObject.SelectedObject();
                        selectedCardObject.MoveChangeObservers += SelectedObjectMoveStateChange;
                        selectedCardObject.StateChangeObservers += OnCurrentObjectStateChange;

                        if (selectedCardObject.MoveTurnUsed)
                        {
                            selectedCardObject.StateChange(CardState.Attack);
                        }
                        else
                        {
                            selectedCardObject.StateChange(CardState.Move);
                        }

                        break;
                    }
                case CardType.Enemy:
                    {
                        Debug.Log("Selected Enenemy");
                        // TODO figure out how to look at enemy 
                        selectedCardObject = cardObject;
                        cardObject.GetCurrentTile.ChangeColor(Color.gray);
                        selectionPanel.gameObject.SetActive(true);
                        selectionPanel.SetObject(cardObject);
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


    void ResetTiles()
    {
        foreach (EnviromentTile tile in Range)
        {
            tile.ChangeColor(tile.MatColorOriginal);
        }
        selectedCardObject.GetCurrentTile.ChangeColor(selectedCardObject.GetCurrentTile.MatColorOriginal);
    }

    //Method used to completely deslect the object (NOTE: may want to see if this can be refactored into fewer lines)
    public void deSelectObject()
    {
        selectionPanel.SetObject(null);
        selectionPanel.gameObject.SetActive(false);

        ResetTiles();

        selectedCardObject.MoveChangeObservers -= SelectedObjectMoveStateChange;
        selectedCardObject.StateChangeObservers -= OnCurrentObjectStateChange;
        selectedCardObject.DeselectObject();

        selectedCardObject = null;
    }

    void OnCurrentObjectStateChange(CardState state)
    {
        ResetTiles();
        var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
        switch (selectedCardObject.cardState)
        {
            case CardState.Move:
                selectedCardObject.GetCurrentTile.ChangeColor(Color.blue);
                Range = selectedCardObject.FindMoveRange();
                foreach (EnviromentTile tile in Range)
                {
                    tile.ChangeColor(Color.cyan);
                }

                if (hit.HasValue)
                {
                    RaycastHit m_hit;
                    m_hit = hit.Value;
                    // Check if their is another object on the tile/ if the tile is open
                    if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open)
                    {
                        if (Path != null) { foreach (EnviromentTile tile in Path)
                            {
                                if (Range.Contains(tile)){ tile.ChangeColor(Color.cyan); }
                                else { tile.ChangeColor(tile.MatColorOriginal); }
                            }
                        }
                        Path = selectedCardObject.MakePath(m_hit.transform.GetComponent<EnviromentTile>());
                        foreach (EnviromentTile tile in Path) { tile.ChangeColor(Color.blue); }
                    }
                }
            
                
                break;
            case CardState.Attack:
                selectedCardObject.GetCurrentTile.ChangeColor(Color.red);
                Range = selectedCardObject.FindAttackRange();
                foreach (EnviromentTile tile in Range)
                {
                    tile.ChangeColor(Orange);
                }
                if (hit.HasValue)
                {
                    RaycastHit m_hit;
                    m_hit = hit.Value;
                    // Check if their is another object on the tile/ if the tile is open
                    if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open || m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Enemy)
                    {
                      //  Debug.Log("Looking");
                        if (selectedCardObject.CheckAttackInRange(m_hit.transform.GetComponent<EnviromentTile>(), Range))
                        { m_hit.transform.GetComponent<EnviromentTile>().ChangeColor(Color.red); }
                       
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
        cardCreator = FindObjectOfType<CardSelector>();
        selectionPanel = FindObjectOfType<SelectionPanel>();
        selectionPanel.gameObject.SetActive(false);
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnPathChange;
        Orange = new Color(1, 0.5f, 0, 1);
        pauseScreen = FindObjectOfType<PauseScreen>();
        pauseScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (selectedCardObject != null)
        {
            switch (selectedCardObject.cardState)
            {
                case CardState.Move:
                    if (!selectedCardObject.MoveTurnUsed) { LookForMoveInput(); }
                    break;
                case CardState.Attack:
                    if (!selectedCardObject.AttackTurnUsed) { LookForAttackInput(); }
                    break;
                default:
                    return;
            }
            LookForStateChange();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.Pause();
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
            if (!selectedCardObject.Moving)
            {
                selectedCardObject.StateChange(CardState.Attack);
            }
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
                    if (selectedCardObject.CheckAttackInRange(m_hit.transform.GetComponent<EnviromentTile>(), Range))
                    {
                        selectedCardObject.EngageCombat(CombatType.Attack, m_hit.transform.GetComponent<EnviromentTile>().ObjectHeld);
                         ResetTiles();
                    }
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
                if (m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Open || m_hit.transform.GetComponent<EnviromentTile>().cardType == CardType.Enemy)
                {
                    //Tell Observers object moving

                    selectedCardObject.enableMovement(Path);
                    ResetTiles();
                }
            }
        }
    }


    //Change Path when a new tile is over cursor while an Object is Selected
    void OnPathChange(Transform newTransform)
    {
        if (selectedCardObject != null)
        {
            //Dont do anything if the selected object is an enemy
            if (selectedCardObject.cardType == CardType.Enemy)
            {

            }

            else if (allowPathChange)
            {
                switch (selectedCardObject.cardState)
                {
                    case CardState.Move:
                        if (newTransform.GetComponent<EnviromentTile>().cardType == CardType.Open)
                        {
                            if (LastTargetTileOver != null)
                            {
                                LastTargetTileOver.ChangeColor(LastTargetTileOver.MatColorOriginal);
                                LastTargetTileOver = null;
                            }

                            if (Path != null)
                            {
                                foreach (EnviromentTile tile in Path)
                                {
                                    if (Range.Contains(tile)) { tile.ChangeColor(Color.cyan); }
                                    else { tile.ChangeColor(tile.MatColorOriginal); }
                                }
                            }
                            Path = selectedCardObject.MakePath(newTransform.GetComponent<EnviromentTile>());
                            if (Path.Contains(newTransform.GetComponent<EnviromentTile>()))
                            {
                                foreach (EnviromentTile tile in Path) { tile.ChangeColor(Color.blue); }
                                LastMoveTileOver = newTransform.GetComponent<EnviromentTile>();
                            }
                            else
                            {
                                selectedCardObject.GetCurrentTile.ChangeColor(Color.blue);
                                LastMoveTileOver = null;
                            }
                                
                          
                        }
                        else if (newTransform.GetComponent<EnviromentTile>().cardType == CardType.Enemy)
                        {
                            if (selectedCardObject.CapableOfAttacking(LastMoveTileOver, newTransform.GetComponent<EnviromentTile>()))
                            {
                                Debug.Log("In Range");
                                if (LastTargetTileOver != null) { LastTargetTileOver.ChangeColor(LastTargetTileOver.MatColorOriginal); }
                                newTransform.GetComponent<EnviromentTile>().ChangeColor(Color.red);
                                LastTargetTileOver = newTransform.GetComponent<EnviromentTile>();
                            }
                            //newTransform.GetComponent<EnviromentTile>().ChangeColor(Color.red);
                            
                        }

                        break;
                    case CardState.Attack:
                        Range = selectedCardObject.FindAttackRange();
                        if (Range != null)
                        {
                            foreach (EnviromentTile tile in Range)
                            {
                                if (Range.Contains(tile)) { tile.ChangeColor(Orange); }
                            }
                        }

                        if (selectedCardObject.CheckAttackInRange(newTransform.GetComponent<EnviromentTile>(), Range))
                        { newTransform.GetComponent<EnviromentTile>().ChangeColor(Color.red); }
                        break;
                    default:
                        return;
                }
            }

        }
    }

}