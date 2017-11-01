using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerAmount : MonoBehaviour {

    Text powerText;
    public int Power = 0;
   
    //Fix this!!!
    private void Awake()
    {
    //    powerText = GetComponent<Text>();
    //    powerText.text = "";
       
    }

    public void SetPower(int CardPower)
    {
        powerText = GetComponent<Text>();
        Power = CardPower;
        powerText.text = Power.ToString();
    }

    public void ResetPower()
    {
        powerText = GetComponent<Text>();
        Power = 0;
       powerText.text = "";
        
    }

}
