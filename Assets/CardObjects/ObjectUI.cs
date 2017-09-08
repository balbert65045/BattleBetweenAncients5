using UnityEngine;

// Add a UI Socket transform to your enemy
// Attack this script to the socket
// Link to a canvas prefab that contains NPC UI
public class ObjectUI : MonoBehaviour {

    // Works around Unity 5.5's lack of nested prefabs
    [Tooltip("The UI canvas prefab")]
    [SerializeField]
    GameObject enemyCanvasPrefab = null;
    [SerializeField]
    float Height = 1.5f;
    [SerializeField]
    float ZOffset = 1.5f;

    Camera cameraToLookAt;

    // Use this for initialization 
    void Start()
    {
        cameraToLookAt = Camera.main;
        Instantiate(enemyCanvasPrefab, transform.position, transform.rotation, transform);
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        //    transform.LookAt(cameraToLookAt.transform);
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + Height, transform.parent.position.z + ZOffset);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }
}