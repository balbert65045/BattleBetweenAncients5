using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour {

    public Image objectImage;
    public GameObject objectName;
    public Image MoveIcon;
    public Image AttackIcon;

    public Text DamageOfSelected;
    public Text DamageModifier;
    public Text MoveofSelected;
    public Text MoveModifier;
    public Text RangeofSelected;
    public Text RangeModifier;
    public Text HealthofSelected;
    public Text HealthModifier;

    private CardObject selectedCardObject;
	// Use this for initialization

    public void SetObject(CardObject cardObject)
    {
        if (cardObject != null)
        {
            if (selectedCardObject != null) { selectedCardObject.StateChangeObservers -= StateChange; }
            selectedCardObject = cardObject;
            selectedCardObject.StateChangeObservers += StateChange;
            objectImage.sprite = cardObject.CardImage;
            objectName.GetComponent<Text>().text = cardObject.gameObject.name;
          
            MoveofSelected.GetComponent<Text>().text = cardObject.GetMoveDistance.ToString();
            if (cardObject.GetMoveModifier > 0)
            {
                MoveModifier.GetComponent<Text>().text = "(+" + cardObject.GetMoveModifier.ToString() +")";
                MoveModifier.GetComponent<Text>().color = Color.green;
            }
            else if (cardObject.GetMoveModifier < 0)
            {
                MoveModifier.GetComponent<Text>().text = "(" + cardObject.GetMoveModifier.ToString() + ")";
                MoveModifier.GetComponent<Text>().color = Color.red;
            }
            else { MoveModifier.GetComponent<Text>().text = ""; }
            
            RangeofSelected.GetComponent<Text>().text = cardObject.GetAttackDistance.ToString();
            if (cardObject.GetAttackDistanceModifier > 0)
            {
                RangeModifier.GetComponent<Text>().text = "(+" + cardObject.GetAttackDistanceModifier.ToString() + ")";
                RangeModifier.GetComponent<Text>().color = Color.green;
            }
            else if (cardObject.GetAttackDistanceModifier < 0)
            {
                RangeModifier.GetComponent<Text>().text = "(" + cardObject.GetAttackDistanceModifier.ToString() + ")";
                RangeModifier.GetComponent<Text>().color = Color.red;
            }
            else { RangeModifier.GetComponent<Text>().text = ""; }

            DamageOfSelected.GetComponent<Text>().text = cardObject.getDamagMin.ToString() + " - " + cardObject.getDamageMax.ToString();
            if (cardObject.GetDamageModifier > 0)
            {
                DamageModifier.GetComponent<Text>().text = "(+" + cardObject.GetDamageModifier.ToString() + ")";
                DamageModifier.GetComponent<Text>().color = Color.green;
            }
            else if(cardObject.GetDamageModifier < 0)
            {
                DamageModifier.GetComponent<Text>().text = "(" + cardObject.GetDamageModifier.ToString() + ")";
                DamageModifier.GetComponent<Text>().color = Color.red;
            }
            else { DamageModifier.GetComponent<Text>().text = "" ; }


            HealthofSelected.GetComponent<Text>().text = cardObject.getCurrentHealth.ToString();



            // Starts in Move state
            MoveIcon.color = Color.blue;
        }
        else
        {
            selectedCardObject.StateChangeObservers -= StateChange;
        }
    }
    public void StateChange(CardState state)
    {
        switch (state)
        {
            case CardState.Move:
                if (selectedCardObject.MoveTurnUsed) { MoveIcon.color = Color.gray; }
                else { MoveIcon.color = Color.blue; }
                if (selectedCardObject.AttackTurnUsed) { AttackIcon.color = Color.gray; }
                else { AttackIcon.color = Color.white; }
                break;
            case CardState.Attack:
                if (selectedCardObject.MoveTurnUsed) { MoveIcon.color = Color.gray; }
                else { MoveIcon.color = Color.white; }
                if (selectedCardObject.AttackTurnUsed) { AttackIcon.color = Color.gray; }
                else { AttackIcon.color = Color.red; }
                break;
            default:
                return;
        }
          

    }
}
