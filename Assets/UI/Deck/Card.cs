using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Card : MonoBehaviour {


    private CardHand cardHand;
    public bool active;

    [SerializeField]
    int PowerAmount = 1;

    public CardName cardName;
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
            //Determines if in Level or DeckBuilder scene
            if (cardHand != null)
            {
                SelectCard();
            }
            return true;
            }
        return false;
    }

    void SelectCard()
    {
        if (GetComponent<CardSummon>() != null)
        {
                GrabObject(GetComponent<CardSummon>());
        }
       else if (GetComponent<CardSpell>() != null)
        {
                GrabSpell(GetComponent<CardSpell>());
        }
        else
        {
            Debug.LogWarning("Card has no component to use. ex: Spell or Summon");
        }
    }


    //Grabs the object refference 
     void GrabObject(CardSummon SummonComponent)
    {
        
        active = true;
        GameObject Object = SummonComponent.SummonObject;
        GameObject ImageObject = SummonComponent.SummonImageObject;
        cardHand.ActiveObject(Object, ImageObject, this);
        cardHand.DeactivateotherButton(this);

    }

    void GrabSpell(CardSpell SpellComponent)
    {
        active = true;
        cardHand.ActiveSpell(this);
        cardHand.DeactivateotherButton(this);
    }


    // Deactivates Object Reference
    public void Deactivate()
    {
        active = false;
    }

}
