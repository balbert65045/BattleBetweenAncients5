using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckHolder : MonoBehaviour {

    // Use this for initialization

    int Index = 0;
    public List<Card> DeckCards;
    GraphicRaycaster myGraphicsRaycaster;

    public CardPosition[] CardPositionArray;


    private void Start()
    {

        myGraphicsRaycaster = FindObjectOfType<GraphicRaycaster>();
        foreach (CardPosition CP in CardPositionArray)
        {
            CP.GetComponent<Text>().text = "";
        }
    }

    public bool CheckMousePositionOnButton()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        myGraphicsRaycaster.Raycast(ped, results);
       
       foreach (RaycastResult RR in results)
        {
            if (RR.gameObject.GetComponent<DeckHolder>())
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        Debug.DrawLine(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - 10), new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10));
    }


    // TO DO make stop if reached max card amount 
    public void AddCard(Card card)
    {
        DeckCards.Add(card);
        CardPositionArray[Index].GetComponent<Text>().text = card.name;
        Index++;
        card.GetComponent<DeckBuildInterface>().AdjustQuantity(-1);
    }

    public void RemoveCard(int indexR)
    {
        Debug.Log(DeckCards.Count);
        Debug.Log(indexR);
        if (DeckCards.Count > indexR)
        {
            DeckCards[indexR].GetComponent<DeckBuildInterface>().AdjustQuantity(1);
            DeckCards.Remove(DeckCards[indexR]);
            ResetCards();
                Index--;
           
        }
    }

    void ResetCards()
    {
        for (int i = 0; i < DeckCards.Count; i++)
        {
            CardPositionArray[i].GetComponent<Text>().text = DeckCards[i].name;
        }
        CardPositionArray[DeckCards.Count].GetComponent<Text>().text = "";

        //if (indexR < DeckCards.Count - 1)
        //{
        //    for (int i = indexR; i < DeckCards.Count - 1; i++)
        //    {
        //      //  CardPositionArray[indexR].GetComponent<Text>().text = DeckCards[]
        //    }
        //}
    }

}
