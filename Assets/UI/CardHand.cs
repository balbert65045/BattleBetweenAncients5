using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour {

    public CreatorButton[] PlayerObjects;
    public GameObject ReadyObject;
    public GameObject ReadyImage;
    public CreatorButton CardUsing;

    public GameObject[] CardPositions;
    private CardDeck cardDeck;

    // Use this for initialization
    void Start()
    {
        PlayerObjects = GetComponentsInChildren<CreatorButton>();
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

    public void DestroyCardUsed()
    {
        Destroy(CardUsing.gameObject);
        CardUsing = null;
        ReadyObject = null;
        ReadyImage = null;
    }

    public void DeactivateotherButton(int type)
    {
        foreach (CreatorButton card in PlayerObjects){
            if (card.type != type)
            {
                card.Deactivate();
            }
        }
    }

    public void ActiveObject(GameObject Obj, GameObject ObjImage, CreatorButton card)
    {
        CardUsing = card;
        ReadyObject = Obj;
        ReadyImage = ObjImage;
    }

    public void ClearObject()
    {
        ReadyObject = null;
    }


}
