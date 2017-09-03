using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour {

    public Image objectImage;
    public GameObject objectName;
    public Image MoveIcon;
    public Image AttackIcon;

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
