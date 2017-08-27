using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour {

    public CardType cardType;

    //float StoppingDistance = .1f;
    public float RemainingDistance;

    public bool Selected = false;
    public bool Moving = false;
    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;
    PathBuilder pathBuilder;

    RaycastHit m_hit;
    PlayerObjectCreator CurrentTile;
    public PlayerObjectCreator GetCurrentTile { get { return CurrentTile; } }


    public void OnCurrentTile(PlayerObjectCreator tileTransform)
    {
        CurrentTile = tileTransform;
    }

    public void SelectedObject()
    {
        Selected = true;
        objectController.SelectedObjectinPlay(this);
    }
    
    public void DeselectObject()
    {
        Selected = false;
        objectController.SelectedObjectinPlay(null);
    }


    // Use this for initialization
    void Start () {
        aiCharacterControl = GetComponent<AICharacterControl>();
        objectController = FindObjectOfType<ObjectController>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        pathBuilder = FindObjectOfType<PathBuilder>();

    }
	
	// Update is called once per frame
	void Update () {
        if (CrossPlatformInputManager.GetButtonDown("pointer2") && Selected)
        {
            Debug.Log("Button Noticed");
            var hit = cameraRaycaster.RaycastForLayer(Layer.LevelTerrain);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
                // Check if their is another object on the tile/ if the tile is open
                if (m_hit.transform.GetComponent<PlayerObjectCreator>().cardType == CardType.Open)
                {
                    pathBuilder.DeselectPath();
                    DeselectObject();
                    Moving = true;
                }
            }
        }

        if (Moving)
        {
            // Move Character to next tile in Tile Path once reaching destination
            RemainingDistance = (transform.position - CurrentTile.transform.position).magnitude;
        //    float RemainingDistance = (transform.position - CurrentTile.transform.position).magnitude;
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
                    Moving = false;
                }
            }
           

        }

       

    }


    void MoveToPosition(Transform newTansform)
    {
        // Move Off the current tile 
        CurrentTile.ObjectMovedOffTile();
        Debug.Log("Attempting to move to position" + newTansform);


       

        aiCharacterControl.SetTarget(newTansform);

        // Tile Change
        OnCurrentTile(newTansform.GetComponent<PlayerObjectCreator>());
        CurrentTile.ObjectMovecOnTile(this.gameObject);

        // Make it so the Object has to be reselected to move again
        //DeselectObject();

    }
}
