﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardField : MonoBehaviour {

    // Use this for initialization

    public GameObject[] CardSummonPositions;
    public GameObject[] CardSpellPositions;
    CardLUT cardLut;


	void Awake () {
        cardLut = FindObjectOfType<CardLUT>();
        
        for (int i = 0; i < CardSummonPositions.Length; i++)
        {
            GameObject card = Instantiate(cardLut.SummonCards[i], CardSummonPositions[i].transform.position, Quaternion.identity);
            card.transform.SetParent(CardSummonPositions[i].transform);
            card.GetComponent<DeckBuildInterface>().SetUpCard();
            card.name = cardLut.SummonCards[i].name;
        }

        for (int i = 0; i < CardSpellPositions.Length; i++)
        {
            GameObject card = Instantiate(cardLut.SpellCards[i], CardSpellPositions[i].transform.position, Quaternion.identity);
            card.transform.SetParent(CardSpellPositions[i].transform);
            card.GetComponent<DeckBuildInterface>().SetUpCard();
            card.name = cardLut.SpellCards[i].name;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
