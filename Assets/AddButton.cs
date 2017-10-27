using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButton : MonoBehaviour {

    // Use this for initialization
    Button button;
    DeckHolder deckHolder;
	void Awake () {
        button = GetComponent<Button>();
        deckHolder = FindObjectOfType<DeckHolder>();

        //Button needs to be set up 
        button.onClick.AddListener(() => { deckHolder.AddCard(GetComponentInParent<Card>()); });

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
