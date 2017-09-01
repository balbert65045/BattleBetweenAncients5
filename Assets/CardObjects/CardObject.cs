using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour, IDamageable
{

    [SerializeField]
    int MaxMoveDistance = 4;
    public int GetMaxMoveDistance { get { return MaxMoveDistance; } }
    [SerializeField]
    int MaxAttackDistance = 1;
    public int GetMaxAttackDistance { get { return MaxAttackDistance; } }
    [SerializeField]
    int AttackDamage = 2;

    public CardType cardType;
    public CardState cardState = CardState.Move;

    public Sprite CardImage;

    public float RemainingDistance { get; private set; }
    public bool Selected { get; private set; }
    public bool Moving { get; private set; }
    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;
    TerrainControl pathBuilder;

    public delegate void OnMoveChange(bool inMoveState); // declare delegate type
    public event OnMoveChange MoveChangeObservers; //instantiate an observer set

    public delegate void OnStateChange(CardState state);
    public event OnStateChange StateChangeObservers;


    RaycastHit m_hit;
    EnviromentTile CurrentTile;
    public EnviromentTile GetCurrentTile { get { return CurrentTile; } }

    [SerializeField]
    int maxHealthPoints = 100;
    int currentHealthPoints;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

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

    public void enableMovement()
    {
        Moving = true;
        if (MoveChangeObservers != null) MoveChangeObservers(Moving);
    }

    public void StateChange(CardState State)
    {
        cardState = State;
        if (StateChangeObservers != null) StateChangeObservers(cardState);
    }




    // Use this for initialization
    void Start () {
        aiCharacterControl = GetComponent<AICharacterControl>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        pathBuilder = FindObjectOfType<TerrainControl>();
        currentHealthPoints = maxHealthPoints;

    }
	
	// Update is called once per frame
	void Update () {
       

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
