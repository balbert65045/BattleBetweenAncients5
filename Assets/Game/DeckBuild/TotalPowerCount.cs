using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalPowerCount : MonoBehaviour {

    // Use this for initialization
    Text TotalPowerText;
    PowerAmount[] PowerAmountArray;

	
    public void UpdatePower()
    {
        int currentPower = 0;
        PowerAmountArray = FindObjectsOfType<PowerAmount>();
        foreach (PowerAmount PA in PowerAmountArray)
        {
            currentPower += PA.Power;
        }
        TotalPowerText = GetComponent<Text>();
        TotalPowerText.text = currentPower.ToString();

    }
}
