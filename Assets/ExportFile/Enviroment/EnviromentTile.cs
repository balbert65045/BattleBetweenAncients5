using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EnviromentTile : MonoBehaviour {

    [SerializeField]
    float SnapDistance = 5f;

    public CardType cardType { get; private set; }
    public GameObject ObjectHeld { get; private set; }
    public Color MatColorOriginal { get; private set; }
    public int X { get; private set; }
    public int Z { get; private set; }

    public GameObject ParentforObjects;


    private PlayerObjectHolder PlayerObjectHolder;
    private Camera cam;
    private CameraRaycaster cameraRaycaster;

   


	// Use this for initialization
	void Awake () {
        PlayerObjectHolder = FindObjectOfType<PlayerObjectHolder>();
        cam = FindObjectOfType<CameraControl>().GetComponent<Camera>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();

        cardType = CardType.Open;
        MatColorOriginal = GetComponent<MeshRenderer>().material.color;

        // Find Position in Grid
        X = Mathf.RoundToInt(transform.position.x / SnapDistance);
        Z = Mathf.RoundToInt(transform.position.z / SnapDistance);

    }


    public void OnItemMake(GameObject newItem)
    {
        if (newItem.GetComponent<CardObject>() == null)
        {
            Debug.LogError("Item Passed does not have item script attached");
        }
        Vector3 location = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        ObjectHeld = Instantiate(newItem, location, Quaternion.identity, ParentforObjects.transform);
        ObjectHeld.GetComponent<CardObject>().OnCurrentTile(this);
        cardType = newItem.GetComponent<CardObject>().cardType;

    }

    public void ObjectMovecOnTile(GameObject obj)
    {
        ObjectHeld = obj;
        cardType = obj.GetComponent<CardObject>().cardType;
    }

    public void ObjectMovedOffTile()
    {
        ObjectHeld = null;
        ChangeColor(MatColorOriginal);
        cardType = CardType.Open;
    }


    public void ChangeColor(Color color)
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat.color = color;
    }

    public void DestroyImage()
    {
        cardType = CardType.Open;
        Destroy(ObjectHeld);
    }

}
