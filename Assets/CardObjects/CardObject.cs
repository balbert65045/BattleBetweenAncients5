﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour, IDamageable
{

    public int MaxMoveDistance;

    [SerializeField]
    int initialMaxMoveDistance = 4;

    public int MaxAttackDistance;
    [SerializeField]
    int initialMaxAttackDistance = 1;
    [SerializeField]
    int AttackDamage = 2;

    public CardType cardType;
    public CardState cardState = CardState.Move;

    public Sprite CardImage;

    public bool Selected { get; private set; }
    public bool Moving { get; private set; }

    public bool MoveTurnUsed { get; private set; }
    public bool AttackTurnUsed { get; private set; }

    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;
    TerrainControl terrainControl;
    private float RemainingDistance;

    public delegate void OnMoveChange(bool inMoveState); // declare delegate type
    public event OnMoveChange MoveChangeObservers; //instantiate an observer set

    public delegate void OnStateChange(CardState state);
    public event OnStateChange StateChangeObservers;
    private Rigidbody m_Rigidbody;

    EnviromentTile CurrentTile;
    public EnviromentTile GetCurrentTile { get { return CurrentTile; } }

    private GameObject ObjectAttacking;

    private List<EnviromentTile> pathTaking;

    // DAMAGE and HEEALTH
    [SerializeField]
    int maxHealthPoints = 100;
    public int currentHealthPoints;

    public float getCurrentHealth { get { return (float)currentHealthPoints; } }

    public void ResetAbilities()
    {
        MoveTurnUsed = false;
        AttackTurnUsed = false;
        MaxAttackDistance = initialMaxAttackDistance;
        MaxMoveDistance = initialMaxMoveDistance;
    }

    public void TakeDamage(int Damage)
    {
        BroadcastMessage("DamageDealt", Damage);
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - Damage, 0, maxHealthPoints);
        if (currentHealthPoints <= 0) {
            CurrentTile.ObjectMovedOffTile();
            Destroy(gameObject);
        }
    }

    public void AttackObject(GameObject obj)
    {
        AttackTurnUsed = true;
        MaxAttackDistance = 0;
        StateChange(CardState.Attack);
        ObjectAttacking = obj;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.LookAt(ObjectAttacking.transform);
        this.GetComponent<ThirdPersonCharacter>().Attack();
    }


    public void DealDamage()
    {

        Component damageableComponent = ObjectAttacking.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(AttackDamage);
            ObjectAttacking.GetComponent<ThirdPersonCharacter>().Hit(this.transform);
        }
    }


    // MOVEMENT
    public void OnCurrentTile(EnviromentTile tileTransform) { CurrentTile = tileTransform;}

    public List<EnviromentTile> MakePath(EnviromentTile EndTile){ return (terrainControl.FindTilesBetween(CurrentTile, EndTile, MaxMoveDistance));}
    public List<EnviromentTile> FindMoveRange() { return(terrainControl.FindMoveRange(CurrentTile, MaxMoveDistance)); }
    public List<EnviromentTile> FindAttackRange() { return (terrainControl.FindAttackRange(CurrentTile, MaxAttackDistance)); }
    public bool CheckAttackInRange(EnviromentTile AttackTile, List<EnviromentTile> AttackRange)
    {
        if (terrainControl.FindEnemyInAttackRange(AttackTile, AttackRange)) {return true;}
        return false;
    }

    public void SelectedObject()
    {
        Selected = true;
        StateChange(CardState.Move);
    }

    public void DeselectObject()
    {
        Selected = false;
    }

    public void enableMovement(List<EnviromentTile> path)
    {
        Moving = true;
        MoveTurnUsed = true;
        MaxMoveDistance = 0;
        pathTaking = path;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        aiCharacterControl.agent.stoppingDistance = .2f;
        if (MoveChangeObservers != null) MoveChangeObservers(Moving);
    }

    public void StateChange(CardState State)
    {
        cardState = State;

        switch (cardState)
        {
            case CardState.Move:
               
                CurrentTile.ChangeColor(Color.blue);
                break;
            case CardState.Attack:
               
                CurrentTile.ChangeColor(Color.red);
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
        m_Rigidbody = GetComponent<Rigidbody>();
        MaxMoveDistance = initialMaxMoveDistance;
        MaxAttackDistance = initialMaxAttackDistance;
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

                int CurrentPathLength = pathTaking.Count - 1;
                int CurrentTileIndex = terrainControl.CheckPathPosition(CurrentTile, pathTaking);
                if (CurrentTileIndex == -1) { Debug.LogError("CurrentPathIndex not in Path of pathBuilder"); }

                if (CurrentTileIndex != CurrentPathLength)
                {
                    Transform NextTileInPath = terrainControl.FindNextTileInPath(CurrentTile, pathTaking);
                    if (NextTileInPath == null) { Debug.LogError("NextTileInPath not in Path of pathBuilder"); }
                    MoveToPosition(NextTileInPath);
                }
                else
                {
                    //Once reaching destination Stop moving and send message to observers 
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