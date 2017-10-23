using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraMove : MonoBehaviour {

    private CardObject cardObjectSelected;
    private TurnSystem turnSystem;
    private AIControl aiControl;
    //    private Spawner playerSpawner;
    private Mage mageSpawner;

    [SerializeField]
    float TransitionTime = 20f;
    [SerializeField]
    float zoomSensitivity = 10f;
    [SerializeField]
    float InitZOffset = 10f;
    [SerializeField]
    float InnitYOffset = 20f;


    // NOTE possibly add a snap to object ability in future
    private void Start()
    {

        //Spawner[] spawners = FindObjectsOfType<Spawner>();
        //foreach (Spawner spawner in spawners)
        //{
        //    if (spawner.cardType == CardType.Player)
        //    {
        //        playerSpawner = spawner;
        //    }
        //}

        turnSystem = FindObjectOfType<TurnSystem>();
        aiControl = FindObjectOfType<AIControl>();
        mageSpawner = FindObjectOfType<Mage>();
        transform.position = new Vector3(mageSpawner.transform.position.x, mageSpawner.transform.position.y + InnitYOffset, mageSpawner.transform.position.z - InitZOffset);
    }

    // Update is called once per frame
    void LateUpdate () {

        {
            if (turnSystem.AITurn)
            {
                Vector3 AIObjectPosition = aiControl.SelectedCardObject.transform.position;
                Vector3 newPosition = new Vector3(AIObjectPosition.x, transform.position.y, AIObjectPosition.z - InitZOffset);
                transform.position = Vector3.Lerp(transform.position, newPosition, TransitionTime * Time.deltaTime);
            }
            else
            {
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");
                float z = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");

                float VerticalPosition = Mathf.Clamp(transform.position.y - z * zoomSensitivity, 10, 30);
                Vector3 newPosition = new Vector3(transform.position.x + h, VerticalPosition, transform.position.z + v);
                transform.position = Vector3.Lerp(transform.position, newPosition, TransitionTime * Time.deltaTime);
            }
        }
    }
}
