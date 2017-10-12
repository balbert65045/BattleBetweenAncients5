using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCount : MonoBehaviour {

    // Use this for initialization
    public TurnSystem turnSystem;
    Text text;

	void Start () {
        turnSystem = FindObjectOfType<TurnSystem>();
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        int TurnLeft = turnSystem.MaxTurns - turnSystem.TurnCount;
        text.text = TurnLeft.ToString();

    }
}
