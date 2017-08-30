using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour {

    [SerializeField]
    int MaxMoveDistance = 4;
    public int GetMaxMoveDistance { get { return MaxMoveDistance; } }

    public CardType cardType;

    public float RemainingDistance { get; private set; }
    public bool Selected { get; private set; }
    public bool Moving { get; private set; }
    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;
    PathBuilder pathBuilder;

    public delegate void OnMoveChange(bool inMoveState); // declare delegate type
    public event OnMoveChange MoveChangeObservers; //instantiate an observer set

    RaycastHit m_hit;
    EnviromentTile CurrentTile;
    public EnviromentTile GetCurrentTile { get { return CurrentTile; } }


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

    }
	
	// Update is called once per frame
	void Update () {
        // Move on right cloick
        if (CrossPlatformInputManager.GetButtonDown("pointer2") && Selected)
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
