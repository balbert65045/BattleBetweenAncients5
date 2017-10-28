using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPowerDisplay : MonoBehaviour {

    // Use this for initialization
    Card creatorButton;
    Text text;

	void Start () {
        creatorButton = GetComponentInParent<Card>();
        text = GetComponent<Text>();
        text.text = creatorButton.GetPowerAmount.ToString();

    }
	
}
