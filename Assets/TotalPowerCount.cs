using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalPowerCount : MonoBehaviour {

    // Use this for initialization
    Text TotalPowerText;
    PowerAmount[] PowerAmountArray;
    

	void Awake () {
        TotalPowerText = GetComponent<Text>();
        TotalPowerText.text = 0.ToString();
        PowerAmountArray = FindObjectsOfType<PowerAmount>();

    }
	
    public void UpdatePower()
    {
        int currentPower = 0;
        foreach (PowerAmount PA in PowerAmountArray)
        {
            currentPower += PA.Power;
        }
        TotalPowerText.text = currentPower.ToString();

    }
}
