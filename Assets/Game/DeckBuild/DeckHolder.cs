using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckHolder : MonoBehaviour {

    // Use this for initialization


    public CardPosition[] CardPositionArray;
    public List<Card> DeckCards;

    int Index = 0;
    GraphicRaycaster myGraphicsRaycaster;
    TotalPowerCount totalPowerCount;
    CardLUT cardLUT;


    private void Start()
    {
       
        myGraphicsRaycaster = FindObjectOfType<GraphicRaycaster>();
       
    }

    // Load the cards that are saved in PlayerPrefs and link them to the cards available 
    // This is caused for the CardField to organize flow 
    public void LoadCards()
    {
        int[] CardsIndex = PlayerPrefsManager.ReturnDeckIndex();
        string[] CardsType = PlayerPrefsManager.ReturnDeckType();
        CardSummon[] summonCardsOut = FindObjectsOfType<CardSummon>();
        CardSpell[] spellCardsOut = FindObjectsOfType<CardSpell>();

        cardLUT = FindObjectOfType<CardLUT>();
        totalPowerCount = FindObjectOfType<TotalPowerCount>();

        if (CardsIndex.Length == 20)
        {
            for (int i = 0; i < CardsIndex.Length; i++)
            {
                GameObject CardFound = null;
                if (CardsType[i] == "CardSummon")
                {
                    GameObject card = cardLUT.SummonCards[CardsIndex[i]];
                    foreach (CardSummon summonCardOut in summonCardsOut)
                    {
                        if (summonCardOut.cardSummonName == card.GetComponent<CardSummon>().cardSummonName)
                        {
                            Debug.Log(summonCardOut);
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

                Debug.Log(CardsType[i]);
                Debug.Log(CardsIndex[i]);
                Debug.Log(CardFound);
                AddCard(CardFound.GetComponent<Card>());
            }
        }
        else
        {
            foreach (CardPosition CP in CardPositionArray)
            {
                CP.GetComponent<Text>().text = "";
            }
        }
    }

    // Check the position by graphic raycasting and returning if hit DeckHolder
    public bool CheckMousePositionOnButton()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        myGraphicsRaycaster.Raycast(ped, results);
       
       foreach (RaycastResult RR in results)
        {
            if (RR.gameObject.GetComponent<DeckHolder>())
            {
                return true;
            }
        }
        return false;
    }


    // Add Card to the Deck being built
    public void AddCard(Card card)
    {
        if (DeckCards.Count < CardPositionArray.Length)
        {
            DeckCards.Add(card);
            CardPositionArray[Index].GetComponent<Text>().text = card.name;

           // Debug.Log(card.GetPowerAmount);
            CardPositionArray[Index].GetComponentInChildren<PowerAmount>().SetPower(card.GetPowerAmount);
            Index++;
            card.GetComponent<DeckBuildInterface>().AdjustQuantity(-1);
            totalPowerCount.UpdatePower();
        }
    }

    // Remove Card from the Deck being built
    public void RemoveCard(int indexR)
    {
        Debug.Log(DeckCards.Count);
        Debug.Log(indexR);
        if (DeckCards.Count > indexR)
        {
            DeckCards[indexR].GetComponent<DeckBuildInterface>().AdjustQuantity(1);
            DeckCards.Remove(DeckCards[indexR]);
            ResetCards();
            Index--;
        }
    }

    //Make sure that cards move up and power is adjusted for the deck 
    void ResetCards()
    {
        for (int i = 0; i < DeckCards.Count; i++)
        {
            CardPositionArray[i].GetComponent<Text>().text = DeckCards[i].name;
            CardPositionArray[i].GetComponentInChildren<PowerAmount>().SetPower(DeckCards[i].GetPowerAmount);
        }
        CardPositionArray[DeckCards.Count].GetComponent<Text>().text = "";
        CardPositionArray[DeckCards.Count].GetComponentInChildren<PowerAmount>().ResetPower();
        totalPowerCount.UpdatePower();
    }

    //Save Deck to Player Prefs
    public void SaveDeck()
    {
        if (DeckCards.Count < 20)
        {
            Debug.LogWarning("NOT ENOUGH CARDS IN DECK");
        }
        else
        {
            int[] CardNameArray = new int[20];
            string[] cardType = new string[20];
            for (int i = 0; i < 20; i++)
            {
                if (DeckCards[i].GetComponent<CardSummon>())
                {
                    Debug.Log((int)DeckCards[i].GetComponent<CardSummon>().cardSummonName);
                    CardNameArray[i] = (int)DeckCards[i].GetComponent<CardSummon>().cardSummonName;
                    cardType[i] = "CardSummon";
                }
                else if (DeckCards[i].GetComponent<CardSpell>())
                {
                    CardNameArray[i] = (int)DeckCards[i].GetComponent<CardSpell>().cardSpellName;
                    cardType[i] = "CardSpell";
                }
                else
                {
                    Debug.LogError("card is not a summon or spell");
                }
            }
            PlayerPrefsManager.SetDeck(CardNameArray, cardType);
            Debug.Log("Saved Deck");
        }
    }

}
