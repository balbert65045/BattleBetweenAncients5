using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class EnviromentTile : MonoBehaviour {

    [SerializeField]
    float SnapDistance = 5f;

    public GameObject ParentforObjects;

    //public CardType cardType;

    public CardType cardType;
    public GameObject ObjectHeld;
    public Color MatColorOriginal { get; private set; }

    public int X;
    public int Z;
    //public int X { get; private set; }
    //public int Z { get; private set; }

    private CardHand PlayerObjectHolder;
    private Camera cam;
    private CameraRaycaster cameraRaycaster;

   


	// Use this for initialization
	void Awake () {
        PlayerObjectHolder = FindObjectOfType<CardHand>();
        cam = FindObjectOfType<CameraControl>().GetComponent<Camera>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();

        //if (cardType == null) { cardType = CardType.Open; }
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
        cardType = newItem.GetComponent<CardObject>().cardType;
        Quaternion StartRot = Quaternion.identity;
        if (cardType == CardType.Enemy) { StartRot = Quaternion.Euler(0, 180, 0); }

        ObjectHeld = Instantiate(newItem, location, StartRot, ParentforObjects.transform);
        ObjectHeld.name = newItem.name;
        ObjectHeld.GetComponent<CardObject>().OnCurrentTile(this);
       

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
