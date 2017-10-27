using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildInterface : MonoBehaviour {


    //For DeckBuilding
    [SerializeField]
    int Qty = 0;

    public int GetQty { get { return Qty; } }
    Button button;
    public Text QuantityText;
    Stats stats;

    // Use this for initialization
    public void SetUpCard () {
        //if (GetComponentInChildren<Quantity>() != null)
        //{
        //    QuantityText = GetComponentInChildren<Quantity>().GetComponent<Text>();
        //    QuantityText.text = Qty.ToString();
        //}

        QuantityText.text = Qty.ToString();

        button = GetComponentInChildren<Button>();
        if (GetComponentInChildren<Stats>())
        {

            stats = GetComponentInChildren<Stats>();
            stats.SetStats(GetComponent<CardSummon>().SummonObject.GetComponent<CardObject>(), GetComponent<Card>().GetPowerAmount);
        }


        //Determines if in Level Scene or deck building scene
        if (FindObjectOfType<CardHand>() != null)
        {
            QuantityText.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }

    }
	
    public void AdjustQuantity(int amount)
    {
        Debug.Log(QuantityText);
        Debug.Log(this.name);
        Qty += amount;
        QuantityText.text = Qty.ToString();
        if (Qty <= 0)
        {
            button.interactable = false;
        } 
        else
        {
            button.interactable = true;
        }
    }
}
