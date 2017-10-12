using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Card : MonoBehaviour {

    public CardUse cardUse;

    //public int type;
    public GameObject Object;
    public GameObject ImageObject;

    private CardHand cardHand;
    public bool active;

    [SerializeField]
    int PowerAmount = 1;
    public int GetPowerAmount { get { return (PowerAmount); } }



	// Use this for initialization
	void Start () {
        cardHand = GetComponentInParent<CardHand>();
      
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
        cardHand.ActiveObject(Object, ImageObject, this);
        cardHand.DeactivateotherButton(this);

    }

    // Deactivates Object Reference
    public void Deactivate()
    {
        active = false;
    }

}
