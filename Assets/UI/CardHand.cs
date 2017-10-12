using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour {

    public Card[] cards;
    public GameObject ReadyObject;
    public GameObject ReadyImage;
    public Card CardUsing;

    public GameObject[] CardPositions;
    private CardDeck cardDeck;

    // Use this for initialization
    void Start()
    {
        cards = GetComponentsInChildren<Card>();
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
                if (cardPostion.GetComponentInChildren<Card>() == null)
                {
                    GameObject newCard = cardDeck.PickCard();
                    GameObject CardinHand = Instantiate(newCard, cardPostion.transform);
                    CardinHand.transform.position = cardPostion.transform.position;
                }
            }
        }
    }

    public void DestroyCardUsed()
    {
        Destroy(CardUsing.gameObject);
        CardUsing = null;
        ReadyObject = null;
        ReadyImage = null;
    }

    public void DeactivateotherButton(Card ActiveCard)
    {
        foreach (Card card in cards){
            if (card != ActiveCard)
            {
                card.Deactivate();
            }
        }
    }

    public void ActiveObject(GameObject Obj, GameObject ObjImage, Card card)
    {
        CardUsing = card;
        ReadyObject = Obj;
        ReadyImage = ObjImage;
    }

    public void Clear()
    {
        ReadyObject = null;
        ReadyImage = null;
        CardUsing = null;
    }


}
