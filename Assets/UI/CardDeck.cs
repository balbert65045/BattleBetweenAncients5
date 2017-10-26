using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour {

    public List<GameObject> CardsInDeck;
    public int CardsLeft { get { return (CardsInDeck.Count); } }
    public Text text;

    void Awake()
    {
        CardsInDeck.Clear();
    }

    public void AddCardtoDeck(GameObject Card)
    {
        
        CardsInDeck.Add(Card);
    }

    public GameObject PickCard()
    {
        int RandomNumber = Random.Range(0, CardsInDeck.Count - 1);
        GameObject CardPicked = CardsInDeck[RandomNumber];
        CardsInDeck.Remove(CardPicked);
        return (CardPicked);
    }

	// Use this for initialization
	void Start () {
        text.text = CardsInDeck.Count.ToString();

    }
	
	// Update is called once per frame
	void Update () {
        text.text = CardsInDeck.Count.ToString();
    }
}
