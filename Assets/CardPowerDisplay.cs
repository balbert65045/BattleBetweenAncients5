using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPowerDisplay : MonoBehaviour {

    // Use this for initialization
    CreatorButton creatorButton;
    Text text;

	void Start () {
        creatorButton = GetComponentInParent<CreatorButton>();
        text = GetComponent<Text>();
        text.text = creatorButton.GetPowerAmount.ToString();

    }
	
}
