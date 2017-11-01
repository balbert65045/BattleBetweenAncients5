using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildInterface : MonoBehaviour {


    //For DeckBuilding
    [SerializeField]
    int Qty = 0;

    public int GetQty { get { return Qty; } }
    public Button button;
    public Text QuantityText;


    // Use this for initialization
    public void SetUpCard () {

        QuantityText.text = Qty.ToString();

        if (GetComponentInChildren<Stats>())
        {

            Stats stats = GetComponentInChildren<Stats>();
            stats.SetStats(GetComponent<CardSummon>().SummonObject.GetComponent<CardObject>(), GetComponent<Card>().GetPowerAmount);
        }

        else if (GetComponentInChildren<SpellStats>())
        {
            Debug.Log("LookingforStats");
            SpellStats spellStats = GetComponentInChildren<SpellStats>();
            spellStats.SetStats(GetComponent<CardSpell>(), GetComponent<Card>().GetPowerAmount);
        }

            //Determines if in Level Scene or deck building scene
            if (FindObjectOfType<CardHand>() != null)
        {
            QuantityText.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
        else
        {
            QuantityText.gameObject.SetActive(true);
            button.gameObject.SetActive(true);
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
