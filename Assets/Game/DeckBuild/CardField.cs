using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardField : MonoBehaviour {

    // Use this for initialization

    public GameObject[] CardSummonPositions;
    public GameObject[] CardSpellPositions;
    CardLUT cardLut;
    DeckHolder deckHolder;
    DeckBuildManager deckBuildManager;


    // Place Objects from LUT on Screen in correct positions
	void Awake () {
        cardLut = FindObjectOfType<CardLUT>();
        deckHolder = FindObjectOfType<DeckHolder>();
        deckBuildManager = FindObjectOfType<DeckBuildManager>();

        int summonIndex = 0;
        for (int i = 0; i < CardSummonPositions.Length; i++)
        {

            if (cardLut.SummonCardsActive[i] && cardLut.SummonCards.Length > i)
            {
                GameObject card = Instantiate(cardLut.SummonCards[i], CardSummonPositions[summonIndex].transform.position, Quaternion.identity);
                card.transform.SetParent(CardSummonPositions[summonIndex].transform);
                card.GetComponent<DeckBuildInterface>().SetUpCard();
                card.name = cardLut.SummonCards[i].name;
                summonIndex++;
            }
        }

        int spellIndex = 0;
        for (int i = 0; i < CardSpellPositions.Length; i++)
        {
            if (cardLut.SpellCardsActive[i] && cardLut.SpellCards.Length > i)
            {
                GameObject card = Instantiate(cardLut.SpellCards[i], CardSpellPositions[spellIndex].transform.position, Quaternion.identity);
                card.transform.SetParent(CardSpellPositions[spellIndex].transform);
                card.GetComponent<DeckBuildInterface>().SetUpCard();
                card.name = cardLut.SpellCards[i].name;
                spellIndex++;
            }
        }

        deckHolder.LoadCards();
        deckBuildManager.LoadCards();
    }
	
}
