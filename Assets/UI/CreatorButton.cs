using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CreatorButton : MonoBehaviour {

    //public int type;
    public GameObject Object;
    public GameObject ImageObject;
    private PlayerObjectHolder OBJHolder;
    public int type; 
    public bool active;
	// Use this for initialization
	void Start () {
        OBJHolder = GetComponentInParent<PlayerObjectHolder>();
      
    }
	


    // Checks if pointer is over their button
    public bool CheckMousePositionOnButton()
    {
        float Xpos = (transform.GetComponent<RectTransform>().position.x);
        float Ypos = (transform.GetComponent<RectTransform>().position.y);

        float deltaX = (transform.GetComponent<RectTransform>().sizeDelta.x);
        float deltaY = (transform.GetComponent<RectTransform>().sizeDelta.y);

        if (Input.mousePosition.x > (Xpos - deltaX/2) && Input.mousePosition.x < (Xpos + deltaX / 2) && 
            Input.mousePosition.y > (Ypos - deltaY/ 2) && Input.mousePosition.y < (Ypos + deltaY / 2))
            {
            GrabObject();
            return true;
            }
        return false;
    }

    //Grabs the object refference 
    public void GrabObject()
    {
        
        active = true;
        OBJHolder.ActiveObject(Object, ImageObject, this);
        OBJHolder.DeactivateotherButton(type);

    }

    // Deactivates Object Reference
    public void Deactivate()
    {
        active = false;
    }

}
