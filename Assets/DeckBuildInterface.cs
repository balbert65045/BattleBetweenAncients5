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
    Text QuantityText;

    [SerializeField]
     GameObject ObjectHolding;
    Stats stats;

    // Use this for initialization
    void Start () {
        if (GetComponentInChildren<Quantity>() != null)
        {
            QuantityText = GetComponentInChildren<Quantity>().GetComponent<Text>();
            QuantityText.text = Qty.ToString();
        }

        button = GetComponentInChildren<Button>();
        if (GetComponentInChildren<Stats>())
        {
            stats = GetComponentInChildren<Stats>();
            stats.SetStats(ObjectHolding.GetComponent<CardObject>(), GetComponent<Card>().GetPowerAmount);
        }
    }
	
    public void AdjustQuantity(int amount)
    {
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
