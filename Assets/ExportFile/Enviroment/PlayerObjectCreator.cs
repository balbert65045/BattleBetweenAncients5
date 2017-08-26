using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerObjectCreator : MonoBehaviour {

    [SerializeField]
    float SnapDistance = 5f;

    private PlayerObjectHolder PlayerObjectHolder;
    public GameObject ParentforObjects;

    public bool PlayerOnTile = false;
    public bool ObjectValid = true;


    public GameObject ObjectHeld;
    public CardType cardType;

    public bool TileSelected = false;

    public int X;
    public int Z;

    private Camera cam;
    private CameraRaycaster cameraRaycaster;

    public Color MatColorOriginal;


	// Use this for initialization
	void Awake () {
        PlayerObjectHolder = FindObjectOfType<PlayerObjectHolder>();
        cam = FindObjectOfType<CameraControl>().GetComponent<Camera>();
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cardType = CardType.Open;
        MatColorOriginal = GetComponent<MeshRenderer>().material.color;
        X = Mathf.RoundToInt(transform.position.x / SnapDistance);
        Z = Mathf.RoundToInt(transform.position.z / SnapDistance);

    }

    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update () {
		
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
   
    public void ObjectHeldDeselected()
    {
        if (ObjectHeld != null)
        {
            if (ObjectHeld.GetComponent<CardObject>() == null)
            {
                Debug.LogError("No CardObject attached to ObjectHeld");
            }
            ObjectHeld.GetComponent<CardObject>().DeselectObject();
            ChangeColor(MatColorOriginal);
            TileSelected = false;
        }
    }

    public void ObjectHeldSelected()
    {
        if (ObjectHeld != null)
        {
            if (ObjectHeld.GetComponent<CardObject>() == null)
            {
                Debug.LogError("No CardObject attached to ObjectHeld");
            }
            Debug.Log("TileSelected");
            ObjectHeld.GetComponent<CardObject>().SelectedObject();
            TileSelected = true;
            ChangeColor(Color.blue);

        }
        else
        {
            Debug.Log("No Object to Select");
        }
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


    public void TogglePlacement(bool value)
    {
        ObjectValid = value;
    }

}
