using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour {

    // Use this for initialization
    public Text buttonText;
    public Button button;
    float AITimeTurn = 3f;
    float TimeWhenPressed;
    bool pressed = false;

    PlayerController playerController;
    CardHand cardHand;
    CardCreator cardCreator;
    AIControl aiControl;

    private bool PlayerTurn = true;
    private bool AITurn = false;

    public void EndTurn(int player)
    {
        if (player == 1)
        {
            PlayerTurn = false;
            playerController.DisableTools();

            AITurn = true;
            aiControl.Active();

            button.interactable = false;
            buttonText.text = "Waiting for Enemy turn";
        }
        else if (player == 2)
        {
            PlayerTurn = true;
            playerController.ResetTools();
            cardHand.Redraw();
            cardCreator.ResetTurn();

            button.interactable = true;
            buttonText.text = "End Turn";

            AITurn = false;
        }

    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        aiControl = FindObjectOfType<AIControl>();
        cardHand = FindObjectOfType<CardHand>();
        cardCreator = FindObjectOfType<CardCreator>();
    }


}
