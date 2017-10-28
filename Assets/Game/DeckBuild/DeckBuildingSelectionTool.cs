using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildingSelectionTool : MonoBehaviour {


    DeckHolder deckHolder;

    Card SelectedCard;
    GameObject SelectedCardObject;

	void Start () {
        deckHolder = FindObjectOfType<DeckHolder>();
    }
	
	// Update is called once per frame
	void Update () {

        // Grab the card is mouse over top of a card
        if (Input.GetButtonDown("pointer1"))
        {

            Card[] Cards = FindObjectsOfType<Card>();
            foreach (Card card in Cards)
            {
                if (card.CheckMousePositionOnButton())
                {
                    if (card.GetComponent<DeckBuildInterface>().GetQty > 0)
                    {
                        SelectedCard = card;
                        SelectedCardObject = Instantiate(card.gameObject, Input.mousePosition, Quaternion.identity);
                        SelectedCardObject.transform.SetParent(this.transform);
                        SelectedCardObject.GetComponentInChildren<Button>().gameObject.SetActive(false);
                        SelectedCardObject.GetComponentInChildren<Quantity>().gameObject.SetActive(false);
                    }
                }
            }
        }

        // Add the card to the deck if mouse over top of deckholder
        else if (Input.GetButtonUp("pointer1"))
        {
            if (SelectedCard != null)
            {
                if (deckHolder.CheckMousePositionOnButton())
                {
                    deckHolder.AddCard(SelectedCard);
                }

                Destroy(SelectedCardObject);
                SelectedCard = null;
                SelectedCardObject = null;
            }
        }

        //Move card with the mouse
        if (SelectedCard != null)
        {
            SelectedCardObject.transform.position = Input.mousePosition;
        }

    }

}
