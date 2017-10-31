using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour, IDamageable
{
    // Movement 
    public int MaxMoveDistance { get; private set; }
    //[SerializeField]
    //int initialMaxMoveDistance = 4;
    public int initialMaxMoveDistance = 4;
    public int GetMoveDistance { get { return MaxMoveDistance; } }
    int MoveModifier = 0;
    int MoveModifierDuration = 0;
    public int GetMoveModifier { get { return MoveModifier; } }
    public void ChangeMoveDistance(int amount, int duration)
    {
        MoveModifier = amount;
        MoveModifierDuration = duration;
        if (!MoveTurnUsed)
        {
            MaxMoveDistance = initialMaxMoveDistance + MoveModifier;
            if (MaxMoveDistance < 0) { MaxMoveDistance = 0; }
        }
    }

    // Attack Range
    public int MaxAttackDistance { get; private set; }
    //[SerializeField]
    //int initialMaxAttackDistance = 1;
    public int initialMaxAttackDistance = 1;
    public int GetAttackDistance { get { return MaxAttackDistance; } }
    int AttackDistanceModifier = 0;
    int AttackDistanceModifierDuration = 0;
    public int GetAttackDistanceModifier { get { return AttackDistanceModifier; } }
    public void ChangeAttackDistance(int amount, int duration)
    {
        AttackDistanceModifier = amount;
        AttackDistanceModifierDuration = duration;
        if (!AttackTurnUsed)
        {
            MaxAttackDistance = initialMaxAttackDistance + AttackDistanceModifier;
            if (MaxAttackDistance < 0) { MaxAttackDistance = 0; }
        }
    }

    // Damage
    int AttackDamageMin;
    //[SerializeField]
    //int initAttackDamageMin = 2;
    public int initAttackDamageMin = 2;
    public int getDamagMin { get { return AttackDamageMin; } }

    int AttackDamageMax;
    //[SerializeField]
    //int initAttackDamageMax = 8;
    public int initAttackDamageMax = 8;
    public int getDamageMax { get { return AttackDamageMax; } }

    int DamageModifier = 0;
    int DamageModifierDuration = 0;
    public int GetDamageModifier { get { return DamageModifier; } }
    public void ChangeAttackDamage(int amount, int duration)
    {
        DamageModifier = amount;
        DamageModifierDuration = duration;
        AttackDamageMin = initAttackDamageMin + DamageModifier;
        if (AttackDamageMin < 0) { AttackDamageMin = 0; }
        AttackDamageMax = initAttackDamageMax + DamageModifier;
        if (AttackDamageMax < 0) { AttackDamageMax = 0; }
    }


    public CardType cardType;
    public CardState cardState = CardState.Move;

    public Sprite CardImage;

    public bool Selected { get; private set; }
    public bool Moving { get; private set; }

    public bool MoveTurnUsed { get; private set; }
    public bool AttackTurnUsed { get; private set; }

    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    private Rigidbody m_Rigidbody;
    TerrainControl terrainControl;
    float RemainingDistance;

    public delegate void OnMoveChange(bool inMoveState, CardObject cardObject); // declare delegate type
    public event OnMoveChange MoveChangeObservers; //instantiate an observer set

    public delegate void OnStateChange(CardState state);
    public event OnStateChange StateChangeObservers;

    public delegate void OnCombatChange(bool inCombatState, CardObject cardObject); // declare delegate type
    public event OnCombatChange CombatChangeObservers; //instantiate an observer set

    public delegate void OnDeathChange(CardObject cardObject);
    public event OnDeathChange DeathChangeObservers;

    public CombatType CurrentCommbatState;
    

    private EnviromentTile CurrentTile;
    public EnviromentTile GetCurrentTile { get { return CurrentTile; } }
    private List<EnviromentTile> pathTaking;


    private GameObject ObjectAttacking;
    private GameObject ObjectAgainst;
    int damageTakeninCombat;

    
    //public List<EnviromentTile> pathFollowing;

    // DAMAGE and HEEALTH
    //[SerializeField]
    //int maxHealthPoints = 100;
    public int maxHealthPoints = 100;
    int currentHealthPoints;
    public float getCurrentHealth { get { return (float)currentHealthPoints; } }

    public void HealObject(int amount)
    {
        BroadcastMessage("Heal", amount);
        currentHealthPoints += amount;
        if (currentHealthPoints > maxHealthPoints)
        {
            currentHealthPoints = maxHealthPoints;
        }
    }


    bool dead = false;
    [SerializeField]
    float deadDestroyTime = 1f;
    float MomentDead;

   


    // Reset Moving and Attacking ability
    public void ResetAbilities()
    {
        MoveTurnUsed = false;
        AttackTurnUsed = false;

        MoveModifierDuration -= 1;
        if (MoveModifierDuration <= 0)
        {
            MoveModifier = 0;
            MoveModifierDuration = 0;
        }
        MaxMoveDistance = initialMaxMoveDistance + MoveModifier;
        if (MaxMoveDistance < 0) { MaxMoveDistance = 0; }

        AttackDistanceModifierDuration -= 1;
        if (AttackDistanceModifierDuration <= 0)
        {
            AttackDistanceModifier = 0;
            AttackDistanceModifierDuration = 0;
        }
        MaxAttackDistance = initialMaxAttackDistance + AttackDistanceModifier;
        if (MaxAttackDistance < 0) { MaxAttackDistance = 0; }

        DamageModifierDuration -= 1;
        if (DamageModifierDuration <= 0)
        {
            DamageModifier = 0;
            DamageModifierDuration = 0;
        }
        AttackDamageMin = initAttackDamageMin + DamageModifier;
        if (AttackDamageMin < 0) { AttackDamageMin = 0; }
        AttackDamageMax = initAttackDamageMax + DamageModifier;
        if (AttackDamageMax < 0) { AttackDamageMax = 0; }




    }

    public void EngageCombat(CombatType combatType, GameObject obj)
    {
        CurrentCommbatState = combatType;
        ObjectAgainst = obj;
        // if the object attacking is another card tell it to defend
       
        if (CurrentCommbatState == CombatType.Attack)
        {
            if (ObjectAgainst.GetComponent<CardObject>() != null) { ObjectAgainst.GetComponent<CardObject>().EngageCombat(CombatType.Defend, gameObject); }
            AttackObject(obj);
        }
    }

    public void AttackObject(GameObject obj)
    {
        AttackTurnUsed = true;
        MoveTurnUsed = true;
        MaxMoveDistance = 0;
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
            int damage = Random.Range(AttackDamageMin, AttackDamageMax + 1);
            (damageableComponent as IDamageable).TakeDamage(damage, this.transform);
        }
    }

    public void TakeDamage(int Damage, Transform attackerTransform)
    {
        damageTakeninCombat = Damage;
        GetComponent<ThirdPersonCharacter>().Hit(attackerTransform);
    }


    public void Retalliate()
    {
        if (CurrentCommbatState == CombatType.Defend)
        {
            //Only retaliate if in range
            if (terrainControl.FindEnemyInAttackRange(ObjectAgainst.GetComponent<CardObject>().GetCurrentTile, terrainControl.FindAttackRange(CurrentTile, initialMaxAttackDistance)))
            {
                AttackObject(ObjectAgainst);
                return;
            }
        }
        ObjectAgainst.GetComponent<CardObject>().CombatOver();
        CombatOver();
        
    }

    public void CombatOver()
    {
        CurrentCommbatState = CombatType.OutOfCombat;
        BroadcastMessage("DamageDealt", damageTakeninCombat);
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damageTakeninCombat, 0, maxHealthPoints);
        damageTakeninCombat = 0;
       
        if (currentHealthPoints <= 0)
        {
            GetComponent<ThirdPersonCharacter>().Dead(ObjectAgainst.transform);
            MomentDead = Time.time;
            dead = true;
            CurrentTile.ObjectMovedOffTile();
            if (DeathChangeObservers != null) DeathChangeObservers(this);
        }
        else
        {
            if (CombatChangeObservers != null) { CombatChangeObservers(false, this); }
        }
    }








    // MOVEMENT
    public void OnCurrentTile(EnviromentTile tileTransform) { CurrentTile = tileTransform;}

    public List<EnviromentTile> MakePath(EnviromentTile EndTile){ return (terrainControl.FindTilesBetween(CurrentTile, EndTile, MaxMoveDistance));}
    public List<EnviromentTile> FindMoveRange() { return (terrainControl.FindMoveRange(CurrentTile, MaxMoveDistance)); }
    public List<EnviromentTile> FindAttackRange() {return (terrainControl.FindAttackRange(CurrentTile, MaxAttackDistance));}
    public List<EnviromentTile> FindAttackRangeAround(EnviromentTile AttackTile, int AttackDistance) { return(terrainControl.FindAttackRange(AttackTile, AttackDistance)); }
    public bool CheckAttackInRange(EnviromentTile AttackTile, List<EnviromentTile> AttackRange)
    {
        if (terrainControl.FindEnemyInAttackRange(AttackTile, AttackRange)) {return true;}
        return false;
    }

    public List<EnviromentTile> FindTilesAround( int distance) { return (terrainControl.FindMoveRange(CurrentTile, distance)); }
    public int FindTileDistance(EnviromentTile Endtile) { return (terrainControl.FindTileDistance(CurrentTile, Endtile).Count); }
    public bool CheckIfPathAvailable(EnviromentTile Endtile){return (terrainControl.CheckIfPathAvailable(CurrentTile, Endtile));}


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
     //   aiCharacterControl.agent.stoppingDistance = cardStoppingDistance;
        if (MoveChangeObservers != null) MoveChangeObservers(Moving, this);
    }

    public void StateChange(CardState State)
    {
        cardState = State;
        if (StateChangeObservers != null) StateChangeObservers(cardState);
    }




    // awake needs to be used in order to be called as soon as instantiated
    void Awake()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        terrainControl = FindObjectOfType<TerrainControl>();
        currentHealthPoints = maxHealthPoints;
        m_Rigidbody = GetComponent<Rigidbody>();
        MaxMoveDistance = initialMaxMoveDistance;
        MaxAttackDistance = initialMaxAttackDistance;
        AttackDamageMin = initAttackDamageMin;
        AttackDamageMax = initAttackDamageMax;
        CurrentCommbatState = CombatType.OutOfCombat;
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
               // Debug.Log("Destination reached");
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
                    if (MoveChangeObservers != null) MoveChangeObservers(Moving, this);
                }
            }
        }

        if (dead)
        {
            if (Time.time > deadDestroyTime + MomentDead)
            {
                if (GetComponent<Mage>()) { GetComponent<Mage>().Death(); }
                Destroy(gameObject);
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