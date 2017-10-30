using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCount : MonoBehaviour {

    // Use this for initialization
    TurnSystem turnSystem;
    Text text;
    public Text WaveText;

    int index = 0;
    bool finalWave = false;

	void Start () {
        turnSystem = FindObjectOfType<TurnSystem>();
        turnSystem.TurnListenerObserver += UpdateTurn;
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void UpdateTurn () {
        //TO DO make this not in the update method 

        // int TurnLeft = turnSystem.MaxTurns - turnSystem.TurnCount;
        int TurnLeft =  turnSystem.TurnCount;
        text.text = TurnLeft.ToString();

        if (turnSystem.AiWavesTimes[index] < turnSystem.TurnCount)
        {
            if (index >= turnSystem.AiWavesTimes.Count - 1)
            {
                finalWave = true;
            }
            else
            {
                index++;
            }
        }

        if (finalWave)
        {
            WaveText.text = "Final Wave";
        }
        else
        {
            WaveText.text = "Wave " + index;
        }
    }
}
