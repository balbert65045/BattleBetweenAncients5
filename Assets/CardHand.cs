using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour {

    // Use this for initialization
    public GameObject[] CardPositions;
    private CardDeck cardDeck;

	void Start () {
        cardDeck = FindObjectOfType<CardDeck>();
        Redraw();

    }

    public void Redraw()
    {
        foreach (GameObject cardPostion in CardPositions)
        {
            // Make sure there is a card to draw 
            if (cardDeck.CardsLeft > 0)
            {
                if (cardPostion.GetComponentInChildren<CreatorButton>() == null)
                {
                    GameObject newCard = cardDeck.PickCard();
                    GameObject CardinHand = Instantiate(newCard, cardPostion.transform);
                    CardinHand.transform.position = cardPostion.transform.position;
                }
            }
        }
    }

}
