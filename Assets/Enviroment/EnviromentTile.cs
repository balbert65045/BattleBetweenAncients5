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


   


	// Use this for initialization
	void Awake () {

        //if (cardType == null) { cardType = CardType.Open; }
        MatColorOriginal = GetComponent<MeshRenderer>().material.color;

        

        // Find Position in Grid
        X = Mathf.RoundToInt(transform.position.x / SnapDistance);
        Z = Mathf.RoundToInt(transform.position.z / SnapDistance);


        //Finds what objects have been placed on top of each tile at start
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, Vector3.up, out hit, 3f);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z));
        if (hasHit)
        {
            if (hit.transform.GetComponent<Spawner>() != null)
            {
                ObjectHeld = hit.transform.gameObject;
                cardType = hit.transform.GetComponent<Spawner>().cardType;
            }

            else if (hit.transform.GetComponent<rock>() != null)
            {
                ObjectHeld = hit.transform.gameObject;
                cardType = CardType.Terrain;
            }
        }

    }

    private void Start()
    {
        if (ObjectHeld != null)
        {
            CenterObject();
        }
    }

    void CenterObject()
    {
        ObjectHeld.transform.position = new Vector3(transform.position.x, ObjectHeld.transform.position.y, transform.position.z);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (ObjectHeld == null)
        {
            if (collision.transform.GetComponent<CardObject>())
            {
                ObjectHeld = collision.transform.gameObject;
                cardType = collision.transform.GetComponent<CardObject>().cardType;
                ObjectHeld.GetComponent<CardObject>().OnCurrentTile(this);
                CenterObject();
            }
        }
    }
}
