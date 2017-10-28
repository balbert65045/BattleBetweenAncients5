using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButton : MonoBehaviour {

    // Use this for initialization
    Button button;
	void Awake () {
        button = GetComponent<Button>();

        //Button needs to be set up 
        button.onClick.AddListener(AddCard);

    }
	
	// Update is called once per frame
	void AddCard()
    {

        DeckHolder DH = FindObjectOfType<DeckHolder>();
        DH.AddCard(GetComponentInParent<Card>());
    }
}
