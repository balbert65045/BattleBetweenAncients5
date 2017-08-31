﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IDamageable
{

    [SerializeField]
    int MaxMoveDistance = 4;
    public int GetMaxMoveDistance { get { return MaxMoveDistance; } }

    public CardType cardType;
    public CardState cardState = CardState.Move;

    public Sprite CardImage;

    public float RemainingDistance { get; private set; }
    public bool Selected { get; private set; }
    public bool Moving { get; private set; }
    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;
    PathBuilder pathBuilder;

    public delegate void OnMoveChange(bool inMoveState); // declare delegate type
    public event OnMoveChange MoveChangeObservers; //instantiate an observer set

    public delegate void OnStateChange(CardState state);
    public event OnStateChange StateChangeObservers;


    RaycastHit m_hit;
    EnviromentTile CurrentTile;
    public EnviromentTile GetCurrentTile { get { return CurrentTile; } }

    [SerializeField]
    float maxHealthPoints = 100f;
    float currentHealthPoints;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    public void TakeDamage(float Damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); }
    }



    public void OnCurrentTile(EnviromentTile tileTransform)
    {
        CurrentTile = tileTransform;
    }

    public void SelectedObject()
    {
        Selected = true;
    }
    
    public void DeselectObject()
    {
        Selected = false;
    }


    // Use this for initialization
    void Start () {
        aiCharacterControl = GetComponent<AICharacterControl>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        pathBuilder = FindObjectOfType<PathBuilder>();
        currentHealthPoints = maxHealthPoints;

    }
	
	// Update is called once per frame
	void Update () {
        // Move on right click
        if (Selected)
        {
            if (cardType == CardType.Player)
            {
                switch (cardState)
                {
                    case CardState.Move:
                        LookForMoveInput();
                        break;
                    case CardState.Attack:
                        break;
                    default:
                        return;
                }
               
                LookForStateChange();
            }

        }

        if (Moving)
        {
            // Move Character to next tile in Tile Path once reaching destination
            RemainingDistance = (transform.position - CurrentTile.transform.position).magnitude;
            if (RemainingDistance < aiCharacterControl.agent.stoppingDistance)
            {

                int CurrentPathLength = pathBuilder.PathLength - 1;
                int CurrentTileIndex = pathBuilder.CheckPathPosition(CurrentTile);
                if (CurrentTileIndex == -1) { Debug.LogError("CurrentPathIndex not in Path of pathBuilder"); }

                if (CurrentTileIndex != CurrentPathLength)
                {
                    Transform NextTileInPath = pathBuilder.FindNextTileInPath(CurrentTile);
                    if (NextTileInPath == null) { Debug.LogError("NextTileInPath not in Path of pathBuilder"); }
                    MoveToPosition(NextTileInPath);
                }
                else
                {
                    //Once reaching destination Stop moving and send message to observers 
                    Debug.Log("Stop Moving");
                    Moving = false;
                    if (MoveChangeObservers != null) MoveChangeObservers(Moving);
                }
            }  
        }
    }

    private void LookForStateChange()
    {
        if (CrossPlatformInputManager.GetButtonDown("MoveStateButton"))
        {
            cardState = CardState.Move;
            if (StateChangeObservers != null) StateChangeObservers(cardState);
           
        }
        else if (CrossPlatformInputManager.GetButtonDown("AttackStateButton"))
        {
            cardState = CardState.Attack;
            if (StateChangeObservers != null) StateChangeObservers(cardState);
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
                    Moving = true;
                    if (MoveChangeObservers != null) MoveChangeObservers(Moving);
                }
                // elseif (m_hit.transform.GetComponent<PlayerObjectCreator>().cardType == CardType.Enemy)
                //This will be how to handle attacking
            }
        }
    }

    void MoveToPosition(Transform newTansform)
    {
        // Move Off the current tile 
        CurrentTile.ObjectMovedOffTile();

        aiCharacterControl.SetTarget(newTansform);

        // Tile Change
        OnCurrentTile(newTansform.GetComponent<EnviromentTile>());
        CurrentTile.ObjectMovecOnTile(this.gameObject);

        // Make it so the Object has to be reselected to move again
        //DeselectObject();

    }
}
