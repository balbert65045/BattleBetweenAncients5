using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour {

    public List<GameObject> CardsInDeck;
    public int CardsLeft { get { return (CardsInDeck.Count); } }
    public Text text;

    CardLUT cardLUT;

    void Awake()
    {
        CardsInDeck.Clear();
        int[] CardsIndex = PlayerPrefsManager.ReturnDeckIndex();
        string[] CardsType = PlayerPrefsManager.ReturnDeckType();
        cardLUT = FindObjectOfType<CardLUT>();
        for (int i = 0; i < CardsIndex.Length; i++)
        {
            if (CardsType[i] == "CardSummon")
            {
                int CardIndex = CardsIndex[i];
                AddCardtoDeck(cardLUT.SummonCards[CardIndex]);
            }
            else if (CardsType[i] == "CardSpell")
            {
                int CardIndex = CardsIndex[i];
                AddCardtoDeck(cardLUT.SpellCards[CardIndex]);
            }
            else
            {
                Debug.LogError("Card Stored neither Summon nor spell");
            }
        }



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
       // CardPicked.GetComponent<DeckBuildInterface>().SetUpCard();
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
