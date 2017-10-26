using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerAmount : MonoBehaviour {

    Text powerText;
    public int Power;
   

    private void Start()
    {
        powerText = GetComponent<Text>();
        powerText.text = "";
       
    }

    public void SetPower(Card card)
    {
        Power = card.GetPowerAmount;
        powerText.text = Power.ToString();
       

    }

    public void ResetPower()
    {
        Power = 0;
       powerText.text = "";
        
    }

}
