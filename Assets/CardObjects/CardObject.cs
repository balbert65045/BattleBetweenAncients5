using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class CardObject : MonoBehaviour {

    public CardType cardType;

    bool Selected = false;
    AICharacterControl aiCharacterControl;
    ObjectController objectController;
    CameraRaycaster cameraRaycaster;

    RaycastHit m_hit;
    PlayerObjectCreator CurrentTile;

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
                    MoveToPosition(m_hit.transform);
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
        DeselectObject();

    }
}
