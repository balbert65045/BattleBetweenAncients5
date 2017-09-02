﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour, IDamageable
{

    [SerializeField]
    int MaxMoveDistance = 4;
    [SerializeField]
    int MaxAttackDistance = 1;
    [SerializeField]
    int AttackDamage = 2;

    public CardType cardType;
    public CardState cardState = CardState.Move;

    public Sprite CardImage;

    public bool Selected { get; private set; }
    public bool Moving { get; private set; }


    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;
    TerrainControl terrainControl;
    private float RemainingDistance;

    public delegate void OnMoveChange(bool inMoveState); // declare delegate type
    public event OnMoveChange MoveChangeObservers; //instantiate an observer set

    public delegate void OnStateChange(CardState state);
    public event OnStateChange StateChangeObservers;

    EnviromentTile CurrentTile;
    public EnviromentTile GetCurrentTile { get { return CurrentTile; } }




    // DAMAGE and HEEALTH
    [SerializeField]
    int maxHealthPoints = 100;
    public int currentHealthPoints;

    public float getCurrentHealthasPercentage { get { return (float)currentHealthPoints/ (float)maxHealthPoints; } }

    public void TakeDamage(int Damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); }
    }

    public void DealDamage(GameObject obj)
    {
        Component damageableComponent = obj.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(AttackDamage);
        }
    }


    // MOVEMENT
    public void OnCurrentTile(EnviromentTile tileTransform) { CurrentTile = tileTransform;}

    public void MakePath(EnviromentTile EndTile){ terrainControl.FindTilesBetween(CurrentTile, EndTile, MaxMoveDistance);}
    public void HighlightTileInAttackRange(EnviromentTile SlectedTile) { terrainControl.FindTileInAttackRange(SlectedTile); }

    public bool CheckAttackInRange(EnviromentTile AttackTile)
    {
        if (terrainControl.FindEnemyInAttackRange(AttackTile)) {return true;}
        return false;
    }

    public void SelectedObject()
    {
        Selected = true;
        CurrentTile.ChangeColor(Color.blue);
        terrainControl.HighlightMoveRange(CurrentTile, MaxMoveDistance);
    }

    public void DeselectObject()
    {
        Selected = false;
        CurrentTile.ChangeColor(CurrentTile.MatColorOriginal);
        terrainControl.ResetTiles();
    }

    public void enableMovement()
    {
        Moving = true;
        terrainControl.ResetTiles();
        if (MoveChangeObservers != null) MoveChangeObservers(Moving);
    }

    public void StateChange(CardState State)
    {
        cardState = State;
        terrainControl.ResetTiles();
        switch (cardState)
        {
            case CardState.Move:
                CurrentTile.ChangeColor(Color.blue);
                terrainControl.HighlightMoveRange(CurrentTile, MaxMoveDistance);
                break;
            case CardState.Attack:
                CurrentTile.ChangeColor(Color.red);
                terrainControl.HighlightAttckRange(CurrentTile, MaxAttackDistance);
                break;
            default:
                return;
        }
        if (StateChangeObservers != null) StateChangeObservers(cardState);
    }




    // Use this for initialization
    void Start()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        terrainControl = FindObjectOfType<TerrainControl>();
        currentHealthPoints = maxHealthPoints;

    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            // Move Character to next tile in Tile Path once reaching destination
            RemainingDistance = (transform.position - CurrentTile.transform.position).magnitude;
            if (RemainingDistance < aiCharacterControl.agent.stoppingDistance)
            {

                int CurrentPathLength = terrainControl.PathLength - 1;
                int CurrentTileIndex = terrainControl.CheckPathPosition(CurrentTile);
                if (CurrentTileIndex == -1) { Debug.LogError("CurrentPathIndex not in Path of pathBuilder"); }

                if (CurrentTileIndex != CurrentPathLength)
                {
                    Transform NextTileInPath = terrainControl.FindNextTileInPath(CurrentTile);
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


    void MoveToPosition(Transform newTansform)
    {
        // Move Off the current tile 
        CurrentTile.ObjectMovedOffTile();

        aiCharacterControl.SetTarget(newTansform);

        // Tile Change
        OnCurrentTile(newTansform.GetComponent<EnviromentTile>());
        CurrentTile.ObjectMovecOnTile(this.gameObject);

        // Make it so the Object has to be reselected to move again
    }
}