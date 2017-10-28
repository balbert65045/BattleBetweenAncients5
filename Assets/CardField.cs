using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardField : MonoBehaviour {

    // Use this for initialization

    public GameObject[] CardPositions;
    CardLUT cardLut;


	void Awake () {
        cardLut = FindObjectOfType<CardLUT>();
        
        for (int i = 0; i < CardPositions.Length; i++)
        {
            GameObject card = Instantiate(cardLut.Cards[i], CardPositions[i].transform.position, Quaternion.identity);
            card.transform.SetParent(CardPositions[i].transform);
            card.GetComponent<DeckBuildInterface>().SetUpCard();
            card.name = cardLut.Cards[i].name;
        }
          
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
