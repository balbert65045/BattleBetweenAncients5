using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerCounter : MonoBehaviour {

    int currentPowerHolding = 1;
    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = currentPowerHolding.ToString();

    }
	
    public void AddPower(int amount)
    {
        currentPowerHolding += amount;
        text.text = currentPowerHolding.ToString();
    }

    public void RemovePower(int amount)
    {
        currentPowerHolding -= amount;
        text.text = currentPowerHolding.ToString();
    }

    public bool PowerQuery(int amount)
    {
        if (amount > currentPowerHolding)
        {
            return false;
        }
        return true;
    }



	// Update is called once per frame
	void Update () {
		
	}
}
