using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildingSelectionTool : MonoBehaviour {

    // Use this for initialization
    Image CardImageHolding;

    DeckHolder deckHolder;
    public Transform ParentTransform;

    Card[] Cards;
    Card SelectedCard;
    GameObject SelectedCardObject;

	void Start () {
        deckHolder = FindObjectOfType<DeckHolder>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("pointer1"))
        {

            Cards = FindObjectsOfType<Card>();
            foreach (Card card in Cards)
            {
                if (card.CheckMousePositionOnButton())
                {
                    if (card.GetComponent<DeckBuildInterface>().GetQty > 0)
                    {
                        SelectedCard = card;
                        SelectedCardObject = Instantiate(SelectedCard.gameObject, Input.mousePosition, Quaternion.identity);
                        SelectedCardObject.transform.parent = ParentTransform;
                        SelectedCardObject.GetComponentInChildren<Button>().gameObject.SetActive(false);
                        SelectedCardObject.GetComponentInChildren<Quantity>().gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (Input.GetButtonUp("pointer1"))
        {
            if (SelectedCard != null)
            {
                Debug.Log(deckHolder.CheckMousePositionOnButton());
                if (deckHolder.CheckMousePositionOnButton())
                {
                    deckHolder.AddCard(SelectedCard);
                }

                Destroy(SelectedCardObject);
                SelectedCard = null;
                SelectedCardObject = null;
            }
        }

        if (SelectedCard != null)
        {
            SelectedCardObject.transform.position = Input.mousePosition;
        }

    }

}
