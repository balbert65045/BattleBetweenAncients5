using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalCardArray : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject[] CardSummons;
    CardDeck cardDeck;

	void Awake () {
        cardDeck = GetComponent<CardDeck>();
        int[] CardsIndex = PlayerPrefsManager.ReturnDeck();

        Debug.Log(CardsIndex[2]);
        for (int i = 0; i < CardsIndex.Length; i++)
        {
            int CardIndex = CardsIndex[i];
            cardDeck.AddCardtoDeck(CardSummons[CardIndex]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
