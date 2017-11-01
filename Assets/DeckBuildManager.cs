using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuildManager : MonoBehaviour {

    // Use this for initialization
    LevelManager levelmanager;
    DeckHolder deckHolder;
    List<Card> CurrentCardsSaved;
    QuestionScreen questionScreen;
    CardSummon[] summonCardsOut;
    CardSpell[] spellCardsOut;



    private void Start()
    {
        levelmanager = FindObjectOfType<LevelManager>();
        deckHolder = FindObjectOfType<DeckHolder>();
        questionScreen = FindObjectOfType<QuestionScreen>();
        questionScreen.gameObject.SetActive(false);

        // TO DO may need to organize when to grab these cards in case they are disabled before reaching them 
    }

    public void LoadCards()
    {
        summonCardsOut = FindObjectsOfType<CardSummon>();
        spellCardsOut = FindObjectsOfType<CardSpell>();
    }




    public void CheckForSave()
    {
        List<Card> cardsInCurrentDeck = deckHolder.DeckCards;
        CurrentCardsSaved = CheckSavedCards();

        
        bool DecksEqual = CompareDecks(cardsInCurrentDeck, CurrentCardsSaved);
        if (DecksEqual)
        {
            levelmanager.LoadNextLevel();
        }
        else
        {
            questionScreen.gameObject.SetActive(true);
        }
        
    }

    bool CompareDecks(List<Card> Deck1, List<Card> Deck2)
    {
        if (Deck1.Count != Deck2.Count)
        {
            return false;
        }

        for (int i = 0; i < Deck1.Count; i++)
        {
            if (Deck1[i] != Deck2[i])
            {
                return false;
            }
        }
        return true;
    }

    List<Card> CheckSavedCards()
    {

        List<Card> cards = new List<Card>();    

        int[] CardsIndex = PlayerPrefsManager.ReturnDeckIndex();
        string[] CardsType = PlayerPrefsManager.ReturnDeckType();
       

        CardLUT cardLUT = FindObjectOfType<CardLUT>();

        if (CardsIndex.Length == 20)
        {
            for (int i = 0; i < CardsIndex.Length; i++)
            {
                GameObject CardFound = null;
                if (CardsType[i] == "CardSummon")
                {
                    GameObject card = cardLUT.SummonCards[CardsIndex[i]];
                    Debug.Log(card.GetComponent<CardSummon>().cardSummonName);
                    foreach (CardSummon summonCardOut in summonCardsOut)
                    {
                        Debug.Log(summonCardOut.cardSummonName);
                        if (summonCardOut.cardSummonName == card.GetComponent<CardSummon>().cardSummonName)
                        {
                            
                            CardFound = summonCardOut.gameObject;
                        }
                    }
                }
                else if (CardsType[i] == "CardSpell")
                {
                    GameObject card = cardLUT.SpellCards[CardsIndex[i]];
                    foreach (CardSpell spellCardOut in spellCardsOut)
                    {
                        if (spellCardOut.cardSpellName == card.GetComponent<CardSpell>().cardSpellName)
                        {
                            Debug.Log(spellCardOut);
                            CardFound = spellCardOut.gameObject;
                        }
                    }
                }
                else
                {
                    Debug.LogError("Card Stored neither Summon nor spell");
                }

                Debug.Log(CardsIndex[i]);
                Debug.Log(CardFound);
            cards.Add(CardFound.GetComponent<Card>());

            }
        }
        return (cards);
    }

}
